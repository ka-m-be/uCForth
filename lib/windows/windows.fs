\ windows.fs - package WINDOWS: the functions of Windows for an application
\
\ uCForth for Windows (targets/win32) can call each function of each DLL
\ of Windows. The program has two HAL words for it; this package gives
\ them their headers, and words that find a function by its name:
\
\   x1 .. xn n a SYSCALL ( -- x )   call the function at the address a
\                                   with n arguments, x1 is the first;
\                                   x is its result (EAX)
\   a >CHIP ( -- a' )               the address for Windows of the VM
\                                   address a; any other address stays
\   a1 u1 a2 u2 FIND-FUNCTION ( -- a )   the address of the function
\                                   a2 u2 in the DLL a1 u1; 0: not there
\   LIBRARY name                    the DLL for the next WINAPI:
\   n WINAPI: name                  a word name ( x1 .. xn -- x ) that
\                                   calls the function name of the DLL;
\                                   the first call finds the function
\   Z" text" ( -- a )               in a definition: the text with a
\                                   byte 0 at the end, as Windows wants it
\
\ Each address that goes to Windows needs >CHIP. Example (Windows shows
\ a window with a text and the button OK):
\   ALSO WINDOWS
\   LIBRARY USER32.dll
\   4 WINAPI: MessageBoxA
\   : HELLO  0 Z" Hello from uCForth" >CHIP Z" uCForth" >CHIP 0 MessageBoxA DROP ;
\
\ Load it on Windows only: another system answers -21 (unsupported
\ operation). targets/win32/README.md, section "Functions of Windows",
\ tells more.

PACKAGE WINDOWS   \ lib/package.fs

PUBLIC
PARSE-NAME SYSCALL HEADER  100 ,  REVEAL
PARSE-NAME >CHIP   HEADER  101 ,  REVEAL

PRIVATE
DICT-END       CONSTANT 'LOADLIBRARYA     \ the HAL puts the addresses of these
DICT-END CELL+ CONSTANT 'GETPROCADDRESS   \ functions of KERNEL32.dll here
256 BUFFER: ZNAME                         \ a name for Windows, with a byte 0
: >ZNAME ( a u -- a' )   255 MIN  DUP ZNAME + 0 SWAP C!  ZNAME SWAP MOVE  ZNAME >CHIP ;
VARIABLE DLL-NAME                         \ LIBRARY: the counted name of the DLL

PUBLIC
: FIND-FUNCTION ( a1 u1 a2 u2 -- a )   \ the function a2 u2 of the DLL a1 u1, or 0
  >R >R  >ZNAME 1 'LOADLIBRARYA @ SYSCALL  R> R> ROT     ( a2 u2 h )
  DUP 0= IF NIP NIP EXIT THEN
  ROT ROT >ZNAME 2 'GETPROCADDRESS @ SYSCALL ;

: LIBRARY ( "name" -- )   \ the DLL for the next WINAPI:
  HERE DLL-NAME !  PARSE-NAME S, ;

PRIVATE
\ A word of WINAPI: has four parts: n, the address of a cell in the RAM
\ data for the address of the function (0: not found yet), the address
\ of the name of the DLL, and the name of the function.
: LOOKUP ( a -- a' )   \ find the function of the word with the data a, and keep its address
  >R  R@ 2 CELLS + @ COUNT  R@ 3 CELLS + COUNT  FIND-FUNCTION
  DUP 0= IF  DROP R> 3 CELLS + COUNT -13 NAME-THROW  THEN
  DUP R> CELL+ @ ! ;

PUBLIC
: WINAPI: ( n "name" -- )   \ name ( x1 .. xn -- x ) calls the function name of the DLL
  DLL-NAME @ 0= ABORT" WINAPI: needs LIBRARY before it"
  >IN @ >R  CREATE  R> >IN !
  ,  CELL VALLOT  0 OVER !  ,  DLL-NAME @ ,  PARSE-NAME S,
  DOES>  DUP CELL+ @ @  ?DUP 0= IF DUP LOOKUP THEN  SWAP @ SWAP SYSCALL ;

: Z" ( "text" -- )   \ in a definition: ( -- a ), the text with a byte 0 at the end
  34 PARSE  POSTPONE (S")  DUP 1+ C,  HERE SWAP DUP ALLOT MOVE  0 C,  ALIGN
  POSTPONE DROP ;  IMMEDIATE COMPILE-ONLY

END-PACKAGE
