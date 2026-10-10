\ asm.fs - package ASM: CODE words on the board (ESP32-S3)
\
\ The assembler XTENSA (asm/xtensa.fs) puts machine code into the
\ dictionary, where the CPU runs it (code stays where it lies):
\
\   CODE name ( -- )     a new word; the assembler follows
\   END-CODE ( -- )      the end of the word
\   xt >CODE ( -- a )    the address of the machine code of a CODE word,
\                        on the instruction bus (for SYSCALL, for example)
\
\ Between them: the instructions of XTENSA (the operands first, then the
\ instruction with a comma), the registers of the VM (MEM TBL IP W SP
\ RP), the macros NEXT, PUSH, POP, TOS@, TOS!, >CHIP, and CONST, (a
\ constant of 32 bits), and the labels L1 to L9. Forth words run there
\ too: "A2 TICKS >CHIP CONST," loads the chip address of TICKS (>CHIP:
\ package ROM).
\
\ A CODE word has the code field 66 (doc/vm.md, section 7). As a word of
\ the VM it may use A2 to A9 and ends with NEXT, (as a primitive of
\ kernels/xtensa.fs):
\
\   CODE 3+ ( n -- n+3 )   A2 TOS@,  A2 A2 3 ADDI,  A2 TOS!,  NEXT,  END-CODE
\
\ As the handler of an interrupt (INT!, package INTR) it ends with RET,
\ instead: see lib/esp32/intr.fs.
\
\ The code goes to HERE on the data bus; the CPU runs it on the
\ instruction bus (the address + $6F0000). END-CODE runs ISYNC, so the
\ fetch of instructions sees the new code. After an error in the
\ assembler, the search order still has XTENSA on top: PREVIOUS.
\
\ Needs TEXT (lib/text.fs), XTENSA (asm/xtensa.fs) and ROM. On the board,
\ in ucf term:
\   #require FAIL lib/text.fs
\   #require ADD, asm/xtensa.fs
\   #include lib/esp32/asm.fs        (chooses lib/esp32/esp32s3/asm.fs)

PACKAGE ASM  ALSO XTENSA  ALSO ROM   \ lib/package.fs

$6F0000 CONSTANT >IRAM              \ the instruction bus: data bus address + this
66 CONSTANT DOCODE                  \ the code field of a CODE word
: ASSEMBLE ( -- )   \ the next code at HERE; the CPU runs it at the chip address + >IRAM
  0 >CHIP >IRAM +  DP  DICT-END  IN-PLACE ;

PARSE-NAME ISYNC HEADER  DOCODE ,  ASSEMBLE  ISYNC, NEXT,  ALIGN  REVEAL

PUBLIC
: CODE ( "name" -- )   PARSE-NAME HEADER  DOCODE ,  ASSEMBLE  ALSO XTENSA ;
: END-CODE ( -- )   CHECK-LABELS  ALIGN  PREVIOUS  ISYNC  REVEAL ;
: >CODE ( xt -- a )   CELL+ >CHIP >IRAM + ;

END-PACKAGE
