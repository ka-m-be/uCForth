\ asm.fs - tests of the package ASM on a PC (lib/pc/asm.fs): CODE words
\
\ Runs on DOS and on a PC without DOS, after the tester, TEXT, I8086 and
\ ASM (targets/dos/README.md and targets/pc-bios/README.md, "Test").

ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO ASM
DECIMAL

CODE PC-3+ ( n -- n+3 )   AX POP,  AX 3 ADDI,  AX PUSH,  NEXT,  END-CODE
T{ 4 PC-3+ -> 7 }T
T{ -5 PC-3+ -> -2 }T

\ While the code runs, CS is the segment of the VM, the same as DS
CODE PC-CS ( -- seg )   CS PUSH,  NEXT,  END-CODE
CODE PC-DS ( -- seg )   DS PUSH,  NEXT,  END-CODE
T{ PC-CS PC-DS = -> TRUE }T

\ A loop with a structure: the sum of the u bytes at a
CODE PC-SUM ( a u -- n )
  CX POP,  DI POP,  AX AX XOR,  DX DX XOR,
  CX CX OR,  NZ IF,
    BEGIN,  DL 0 [DI] MOV,  AX DX ADD,  DI INC,  CX DEC,  Z UNTIL,
  THEN,
  AX PUSH,  NEXT,  END-CODE
T{ S" ABC" PC-SUM -> 198 }T
T{ HERE 0 PC-SUM -> 0 }T

\ A local label, and the code of a word at its VM address
CODE PC-MAX ( n1 n2 -- n )   AX POP,  DX POP,  AX DX CMP,  L1 JGE,  AX DX MOV,  LABEL L1  AX PUSH,  NEXT,  END-CODE
T{ 3 9 PC-MAX -> 9 }T
T{ 9 3 PC-MAX -> 9 }T
T{ -1 -7 PC-MAX -> -1 }T
T{ ' PC-3+ @ -> 66 }T                 \ the code field DOCODE
T{ ' PC-3+ CELL+ C@ -> $58 }T         \ the code follows it: POP AX

\ In a colon word, with EXECUTE, and the return stack afterwards
: PC-TWICE ( n -- n+6 )   PC-3+ PC-3+ ;
T{ 1 PC-TWICE -> 7 }T
T{ 5 ' PC-3+ EXECUTE -> 8 }T
T{ RP@ 1 PC-3+ DROP RP@ = -> TRUE }T
: PC-LOOP ( -- n )   0  10 0 DO  I PC-3+ +  LOOP ;   \ inside a loop: its cells on the return stack stay
T{ PC-LOOP -> 75 }T

S" asm" .RESULTS
