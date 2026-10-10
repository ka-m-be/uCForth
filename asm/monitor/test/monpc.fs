\ monpc.fs - tests of the package MON-PC (asm/monitor/monpc.fs)
\
\ Runs on DOS and on a PC without DOS, after the tester, TEXT, I8086,
\ ASM, MONITOR, DISX86 and MON-PC (asm/monitor/README.md, "Test").

ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO MONITOR  ALSO MON-PC
DECIMAL

\ Memory with segment and offset: here the segment of the VM
CREATE M-BUF  8 ALLOT
T{ $1234 VM-SEG M-BUF !L  M-BUF @ -> $1234 }T
T{ $AB VM-SEG M-BUF 2 + C!L  M-BUF 2 + C@ -> $AB }T
T{ VM-SEG M-BUF @L -> $1234 }T
T{ VM-SEG M-BUF 2 + C@L -> $AB }T

\ The BIOS: the memory in KB (INT 12h); its data area at 0040:0013 holds it too
T{ $12 INT-CALL  GET-REG AX  $40 $13 @L = -> TRUE }T
T{ GET-REG AX 0 > -> TRUE }T

\ The registers go to the code and come back; the flags too
CREATE M-CODE   $8B C, $C3 C,  $F9 C,  $CB C,          \ mov ax, bx / stc / retf
T{ $5678 SET-REG BX  0 SET-REG AX  VM-SEG M-CODE CALL-AT  GET-REG AX -> $5678 }T
T{ GET-REG FL 1 AND -> 1 }T                            \ the carry of STC
T{ GET-REG DS VM-SEG = -> TRUE }T
\ The code reads with its DS: here the VM, BX a VM address
CREATE M-CODE2  $8B C, $07 C,  $CB C,                  \ mov ax, word ptr [bx] / retf
T{ $4321 M-BUF !  M-BUF SET-REG BX  VM-SEG M-CODE2 CALL-AT  GET-REG AX -> $4321 }T
\ DS of the call: another segment, 0040 (the data of the BIOS)
T{ $40 SET-REG DS  $13 SET-REG BX  VM-SEG M-CODE2 CALL-AT  GET-REG AX  $40 $13 @L = -> TRUE }T
T{ VM-SEG SET-REG DS -> }T
\ IP, RP and the data stack are as before
T{ 1 2 3  VM-SEG M-CODE CALL-AT -> 1 2 3 }T
T{ RP@  VM-SEG M-CODE CALL-AT  RP@ = -> TRUE }T
: M-TWICE ( -- n )   VM-SEG M-CODE CALL-AT  VM-SEG M-CODE CALL-AT  7 ;
T{ M-TWICE -> 7 }T

\ The disassembler reads the code at a VM address, and with a segment
T{ M-CODE DECODE M-CODE -  TXT COUNT S" mov ax, bx" S= -> 2 TRUE }T
T{ VM-SEG M-CODE 3 DISASML  NIP M-CODE - -> 4 }T
\ The reset vector of the BIOS: a far jump
T{ $F000 $FFF0 C@L -> $EA }T
T{ $F000 $FFF0 1 DISASML -> $F000 $FFF5 }T
\ Single steps: mov ax, 1 / inc ax / inc ax / retf
CREATE M-STEPS  $B8 C, $01 C, $00 C,  $40 C,  $40 C,  $CB C,
T{ 0 SET-REG AX  VM-SEG M-STEPS DEBUG-AT  STEP  GET-REG AX  GET-REG IP M-STEPS -  STOPPED -> 1 3 1 }T
T{ GET-REG CS VM-SEG = -> TRUE }T
T{ STEP  GET-REG AX  GET-REG IP M-STEPS - -> 2 4 }T
\ A breakpoint at the RETF; afterwards the byte of the code is back
T{ VM-SEG M-STEPS 5 + BREAK-AT  GO  GET-REG AX  GET-REG IP M-STEPS -  STOPPED -> 3 5 3 }T
T{ VM-SEG M-STEPS 5 + C@L -> $CB }T
\ GO from the breakpoint: the RETF, the end of the code
T{ GO  STOPPED  GET-REG AX -> 0 3 }T
T{ ' GO CATCH -> -2 }T                                 \ no code waits
CLEAR-BREAKS
\ The last step is the RETF: the end too
T{ VM-SEG M-STEPS DEBUG-AT  STEP STEP STEP  STOPPED  STEP  STOPPED -> 1 0 }T
\ A breakpoint in a loop: mov cx, 3 / l: inc ax / loop l / retf
CREATE M-LOOP  $B9 C, $03 C, $00 C,  $40 C,  $E2 C, $FD C,  $CB C,
T{ 0 SET-REG AX  VM-SEG M-LOOP DEBUG-AT  VM-SEG M-LOOP 3 + BREAK-AT
   GO GET-REG AX  GO GET-REG AX  GO GET-REG AX  GO STOPPED GET-REG AX -> 0 1 2 0 3 }T
CLEAR-BREAKS
\ The vectors 1 and 3 are as before; IP, RP and the stack of the VM stay
T{ 0 12 @L  0 14 @L  VM-SEG M-STEPS DEBUG-AT  STEP  0 12 @L  0 14 @L  ROT =  ROT ROT = -> TRUE TRUE }T
T{ 1 2 3  VM-SEG M-STEPS DEBUG-AT  STEP STEP GO -> 1 2 3 }T
\ The flags of the code: STC, then a step
CREATE M-FLAGS  $F9 C,  $CB C,                         \ stc / retf
T{ VM-SEG M-FLAGS DEBUG-AT  STEP  GET-REG FL 1 AND  GET-REG FL $100 AND -> 1 0 }T

CR .REGS
S" monpc" .RESULTS
