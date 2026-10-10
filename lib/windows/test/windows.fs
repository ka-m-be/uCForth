\ windows.fs - tests of the package WINDOWS (lib/windows/windows.fs), only on Windows
\
\ Load (on Windows, in the folder ucf), after the package FILES and the
\ tester:
\   C:\ucf>build\ucf-win32
\   ok> ALSO FILES  S" forth/test/tester.fs" INCLUDED
\   ok> ALSO FILES  S" lib/windows/test/windows.fs" INCLUDED
\ targets/win32/README.md runs it in Wine too.

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/windows/windows.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO WINDOWS  ALSO INTERNALS
DECIMAL

\ >CHIP: the memory of the VM begins at $10010000 (targets/win32/build.fs)
T{ 0 >CHIP  $3FFFF >CHIP  $40000 >CHIP -> $10010000 $1004FFFF $40000 }T

\ FIND-FUNCTION and SYSCALL
: LSTRLEN ( -- a )   S" KERNEL32.dll" S" lstrlenA" FIND-FUNCTION ;
: NONE-1 ( -- a )    S" KERNEL32.dll" S" NoSuchFunction" FIND-FUNCTION ;
: NONE-2 ( -- a )    S" NOSUCH.dll" S" lstrlenA" FIND-FUNCTION ;
T{ LSTRLEN 0= -> FALSE }T
T{ NONE-1 NONE-2 -> 0 0 }T
CREATE BUF 32 ALLOT
T{ S" abc" BUF SWAP MOVE  0 BUF 3 + C!  BUF >CHIP 1 LSTRLEN SYSCALL -> 3 }T
T{ BUF >CHIP 0  2 LSTRLEN SYSCALL -> 3 }T     \ one argument too many: ESP comes back

\ WINAPI: and Z"
LIBRARY KERNEL32.dll
1 WINAPI: lstrlenA
0 WINAPI: GetCurrentProcessId
3 WINAPI: RtlMoveMemory
0 WINAPI: NoSuchFunction
LIBRARY USER32.dll
4 WINAPI: wsprintfA                           \ C: the caller removes the arguments

: T-LEN ( -- n )     Z" uCForth" >CHIP lstrlenA ;
: T-PRINT ( -- n )   BUF >CHIP Z" %d-%d" >CHIP 12 34 wsprintfA ;
: T-MOVE ( -- )      BUF 16 + >CHIP  BUF >CHIP  6 RtlMoveMemory DROP ;
T{ T-LEN -> 7 }T
T{ GetCurrentProcessId 0= -> FALSE }T
T{ T-PRINT T-PRINT -> 5 5 }T
T{ BUF S" 12-34" DROP 5 SAME?  BUF 5 + C@ -> TRUE 0 }T
T{ T-MOVE  BUF 16 + S" 12-34" DROP 5 SAME?  BUF 21 + C@ -> TRUE 0 }T

\ Errors: a function that is not there; a fault in a function; fewer
\ cells than n. Then the VM works on.
: T-FAULT ( -- )   BUF >CHIP 1 4 RtlMoveMemory DROP ;   \ it reads at the address 1
: T-UNDER ( -- )   1000 LSTRLEN SYSCALL ;
T{ ' NoSuchFunction CATCH -> -13 }T
T{ ' T-FAULT CATCH -> -9 }T
T{ ' T-UNDER CATCH -> -4 }T
T{ T-LEN -> 7 }T

S" windows" .RESULTS
