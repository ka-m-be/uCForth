\ asm.fs - package ASM: CODE words on the board (ESP32)
\
\ The assembler XTENSA (asm/xtensa.fs) makes the machine code of a CODE
\ word. The ESP32 runs no code in the RAM for data, where the dictionary
\ is. So the code goes to SRAM0, the RAM of the instruction bus, after
\ the code of the kernel, and the cell after the code field holds its
\ address there (the primitive 66 of targets/esp32/hal.fs). The words
\ are those of the ESP32-S3 (lib/esp32/esp32s3/asm.fs):
\
\   CODE name ( -- )     a new word; the assembler follows
\   END-CODE ( -- )      the end of the word
\   xt >CODE ( -- a )    the address of the machine code of a CODE word
\
\ Between them: the instructions of XTENSA, the registers of the VM (MEM
\ TBL IP W SP RP), the macros NEXT, PUSH, POP, TOS@, TOS!, >CHIP, and
\ CONST, and the labels L1 to L9, as on the ESP32-S3:
\
\   CODE 3+ ( n -- n+3 )   A2 TOS@,  A2 A2 3 ADDI,  A2 TOS!,  NEXT,  END-CODE
\
\ As the handler of an interrupt (INT!, package INTR) a CODE word ends
\ with RET, instead: see lib/esp32/intr.fs.
\
\ The code goes first to a buffer of 2 KB (so a CODE word has at most 2
\ KB of code). END-CODE copies it to SRAM0, a cell at a time: the CPU
\ writes SRAM0 only in cells. The code of the CODE words starts at
\ CODE-START and ends at CODE-HERE (package BOOTIMG); SRAM0 has room up
\ to $400A_0000. FREEZE saves this code with the code of the kernel.
\ MARKER gives the dictionary back, not SRAM0: the code of the words
\ that it forgets stays there until the next start.
\
\ Needs TEXT (lib/text.fs), XTENSA (asm/xtensa.fs), ROM and BOOTIMG. On
\ the board, in ucf term:
\   #require FAIL lib/text.fs
\   #require ADD, asm/xtensa.fs
\   #include lib/esp32/asm.fs        (chooses lib/esp32/esp32/asm.fs)

PACKAGE ASM  ALSO XTENSA  ALSO ROM  ALSO BOOTIMG   \ lib/package.fs

\ VM addresses 0 to $FFFF lie in another part of the RAM (asm/xtensa.fs,
\ DATA-OFF; targets/esp32/README.md, "Memory"): >CHIP, needs the offset.
\ The frozen part of the dictionary can lie in the flash (FLASH-DICT?):
\ NEXT, and >CHIP, read its end from the table of the primitives.
0 >CHIP  $10000 >CHIP $10000 -  -  DATA-OFF !
TRUE FLASH-DICT? !

66 CONSTANT DOCODE                  \ the code field of a CODE word
$400A0000 CONSTANT SRAM0-END        \ the end of SRAM0 on the instruction bus
2048 CONSTANT #BUF
#BUF BUFFER: BUF                    \ the code of the CODE word that comes
VARIABLE BP                         \ the next byte in BUF
: IHERE ( -- a )   CODE-HERE @ ?DUP 0= IF  CODE-START  THEN ;
: ASSEMBLE ( -- )   \ the next code to BUF; the CPU runs it at IHERE
  BUF BP !  IHERE BUF -  BP  BUF #BUF +  IN-PLACE ;
: PLACE ( -- )   \ the code from BUF to SRAM0 at IHERE, a cell at a time
  BP @ BUF -  3 + -4 AND                            ( u )
  DUP IHERE + SRAM0-END U> ABORT" ASM: SRAM0 is full"
  DUP 0 ?DO  BUF I + @  IHERE I + !  4 +LOOP
  IHERE + CODE-HERE ! ;
: (CODE) ( -- )   DOCODE ,  IHERE ,  ASSEMBLE ;     \ after the header

PARSE-NAME ISYNC HEADER  (CODE)  ISYNC, NEXT,  PLACE  REVEAL

PUBLIC
: CODE ( "name" -- )   PARSE-NAME HEADER  (CODE)  ALSO XTENSA ;
: END-CODE ( -- )   CHECK-LABELS  PLACE  PREVIOUS  ISYNC  REVEAL ;
: >CODE ( xt -- a )   CELL+ @ ;

END-PACKAGE
