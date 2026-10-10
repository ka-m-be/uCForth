\ monwin.fs - tests of the package MON-WIN32 (asm/monitor/monwin.fs), only on Windows
\
\ Runs on Windows (or in Wine), after the tester, TEXT, X86, WINDOWS,
\ MONITOR, DISX86 and MON-WIN32 (asm/monitor/README.md, "Test").

ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO X86  ALSO WINDOWS
ALSO MONITOR  ALSO MON-WIN32  DECIMAL

\ Memory that runs: VirtualAlloc gives whole pages
T{ 64 EXEC-ALLOT  DUP 0<>  SWAP $FFF AND -> TRUE 0 }T

\ Code of our own, with a register and an argument: EAX = EBX + x1
64 EXEC-ALLOT CONSTANT W-ADD
W-ADD ORG  EAX EBX MOV,  EAX 4 [ESP] ADD,  RET,  PLACE-CODE
T{ 3 SET-REG EBX  7 1 W-ADD CALL-AT  GET-REG EAX  GET-REG EBX -> 10 3 }T

\ The registers that the code does not change stay, and the stack of the VM
64 EXEC-ALLOT CONSTANT W-RET
W-RET ORG  RET,  PLACE-CODE
T{ 11 SET-REG ESI  12 SET-REG EDI  13 SET-REG EBP  21 SET-REG ECX  22 SET-REG EDX
   0 W-RET CALL-AT  GET-REG ESI GET-REG EDI GET-REG EBP GET-REG ECX GET-REG EDX -> 11 12 13 21 22 }T
T{ 1 2 3  0 W-RET CALL-AT -> 1 2 3 }T

\ The flags: the carry after STC and after CLC
64 EXEC-ALLOT CONSTANT W-STC   W-STC ORG  STC,  RET,  PLACE-CODE
64 EXEC-ALLOT CONSTANT W-CLC   W-CLC ORG  CLC,  RET,  PLACE-CODE
T{ 0 W-STC CALL-AT  GET-REG EFL 1 AND  0 W-CLC CALL-AT  GET-REG EFL 1 AND -> 1 0 }T

\ Two arguments in the convention of C: x1 - x2
64 EXEC-ALLOT CONSTANT W-SUB
W-SUB ORG  EAX 4 [ESP] MOV,  EAX 8 [ESP] SUB,  RET,  PLACE-CODE
T{ 50 8 2 W-SUB CALL-AT  GET-REG EAX -> 42 }T

\ A function of Windows (stdcall): lstrlenA
CREATE W-TEXT 8 ALLOT  S" uCForth" W-TEXT SWAP MOVE  0 W-TEXT 7 + C!
: W-LSTRLEN ( -- a )   S" KERNEL32.dll" S" lstrlenA" FIND-FUNCTION ;
T{ W-TEXT >CHIP 1 W-LSTRLEN CALL-AT  GET-REG EAX -> 7 }T

\ An exception in the call: the error -9; then a call goes as before
64 EXEC-ALLOT CONSTANT W-BAD
W-BAD ORG  EAX 0 [] MOV,  RET,  PLACE-CODE
T{ 0 W-BAD ' CALL-AT CATCH NIP NIP -> -9 }T
T{ 3 SET-REG EBX  5 1 W-ADD CALL-AT  GET-REG EAX -> 8 }T

\ Memory at any address; DISASM reads the code with X-C@
T{ W-TEXT X-C@  W-TEXT >CHIP X-C@ -> 'u' 'u' }T
T{ W-ADD DECODE W-ADD -  TXT COUNT S" mov eax, ebx" S= -> 2 TRUE }T
T{ W-SUB DECODE DECODE DROP  TXT COUNT S" sub eax, dword ptr [esp + 0x8]" S= -> TRUE }T

CR .REGS
W-ADD 3 DISASM DROP
W-ADD 16 DUMPX CR
S" monwin" .RESULTS
