\ dos.fs - package DOS: the HAL words of uCForth for MS-DOS
\
\ The kernel of targets/dos has these primitives; this package gives
\ them their names (doc/vm.md, section 8). Load it on DOS only: another
\ system answers -21 (unsupported operation). A PC without DOS has the
\ same primitives in the package BIOS (lib/pc/bios.fs).
\
\   x1 ... xn n id SYSCALL ( -- x )   INT id with x1 ... xn in AX BX CX DX SI DI
\                                     (n at most 6); x is AX after the call
\   REGS ( -- a )                     the registers after SYSCALL: AX BX CX DX
\                                     SI DI, the flags, ES; one cell each
\   port PC@ ( -- c )   c port PC! ( -- )     a byte from or to a port
\   port PW@ ( -- x )   x port PW! ( -- )     a word from or to a port
\   x1 ... xn n DOS ( -- ax f )       INT 21h; f true: the carry is set (an error)
\
\ DS and ES are the segment of the VM, so an address in DX, SI or DI is
\ a VM address: "S" NAME" ..." gives one. Example: the version of DOS:
\   ALSO DOS  $3000 1 DOS DROP  DUP 255 AND . 8 RSHIFT .

PACKAGE DOS   \ lib/package.fs

PRIVATE
: PRIM ( n "name" -- )   PARSE-NAME HEADER ,  REVEAL ;   \ a header for the primitive n

PUBLIC
100 PRIM SYSCALL ( x1 ... xn n id -- x )
110 PRIM PC@     ( port -- c )
111 PRIM PC!     ( c port -- )
112 PRIM PW@     ( port -- x )
113 PRIM PW!     ( x port -- )
$FFF0 CONSTANT REGS   \ AX BX CX DX SI DI flags ES after SYSCALL (doc/vm.md, section 3: free)
: DOS ( x1 ... xn n -- ax f )   $21 SYSCALL  REGS 12 + @ 1 AND 0<> ;

END-PACKAGE
