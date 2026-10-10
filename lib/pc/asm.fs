\ asm.fs - package ASM: CODE words on a PC (DOS, or without an operating system)
\
\ The assembler I8086 (asm/8086.fs) puts machine code into the
\ dictionary, where the CPU runs it (code stays where it lies):
\
\   CODE name ( -- )     a new word; the assembler follows
\   END-CODE ( -- )      the end of the word
\   NEXT, ( -- )         the end of the code: back to the kernel
\   xt >CODE ( -- a )    the address of the code of a CODE word: a vector
\                        of an interrupt can point to VM-SEG:a
\
\ Between them: the instructions of I8086, the labels L1 to L9 and the
\ structures IF, ... THEN, and BEGIN, ... UNTIL, (asm/README.md). The
\ registers of the VM (kernels/8086.fs): SI is IP, SP the data stack
\ (POP, and PUSH, take and give the cells of Forth), BP the return
\ stack; AX BX CX DX DI are free. DS, ES and SS hold the segment of the
\ VM and must hold it again at NEXT,. While the code runs, CS is the
\ segment of the VM too: a VM address is also the address of the code
\ for the CPU, and "CS:" reaches the memory of the VM when DS holds
\ another segment.
\
\   CODE 3+ ( n -- n+3 )   AX POP,  AX 3 ADDI,  AX PUSH,  NEXT,  END-CODE
\
\ A CODE word has the code field 66 (doc/vm.md, section 7). DOCODE of
\ the kernel puts the way back to the kernel on the return stack (two
\ cells), and NEXT, takes it from there: so a CODE word leaves BP as
\ it found it. Ctrl-C and Ctrl-Break come at the next primitive after
\ NEXT, (the break changes the table of the kernel): a CODE word that
\ never ends cannot be stopped.
\
\ After an error in the assembler, the search order still has I8086 on
\ top: PREVIOUS.
\
\ Needs TEXT (lib/text.fs) and I8086 (asm/8086.fs). On DOS:
\   ALSO FILES  INCLUDE lib/text.fs  INCLUDE asm/8086.fs  INCLUDE lib/pc/asm.fs
\ On a PC without DOS, in ucf term (targets/pc-bios/README.md):
\   #include lib/text.fs
\   #include asm/8086.fs
\   #include lib/pc/asm.fs

PACKAGE ASM  ALSO I8086   \ lib/package.fs

66 CONSTANT DOCODE                  \ the code field of a CODE word
: ASSEMBLE ( -- )   \ the next code at HERE; the CPU runs it at the same address
  0  DP  DICT-END  IN-PLACE ;

PUBLIC
: NEXT, ( -- )   \ back to the kernel: the far address that DOCODE put on the return stack
  BP 4 ADDI,  -4 [BP] JMP-FAR-IND, ;
: CODE ( "name" -- )   PARSE-NAME HEADER  DOCODE ,  ASSEMBLE  ALSO I8086 ;
: END-CODE ( -- )   CHECK-LABELS  ALIGN  PREVIOUS  REVEAL ;
: >CODE ( xt -- a )   CELL+ ;   \ the code follows the code field

END-PACKAGE
