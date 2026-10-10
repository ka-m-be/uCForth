\ bios.fs - package BIOS: the HAL words of uCForth on a PC without an operating system
\
\ The kernel of targets/pc-bios has these primitives; this package gives
\ them their names (doc/vm.md, section 8). The image for the PC has it
\ (targets/pc-bios/README.md, "Build"). On another system the words
\ give -21 (unsupported operation); on DOS the package DOS has them.
\
\   x1 ... xn n id SYSCALL ( -- x )   INT id with x1 ... xn in AX BX CX DX SI DI
\                                     (n at most 6); x is AX after the call
\   REGS ( -- a )                     the registers after SYSCALL: AX BX CX DX
\                                     SI DI, the flags, ES; one cell each
\   port PC@ ( -- c )   c port PC! ( -- )     a byte from or to a port
\   port PW@ ( -- x )   x port PW! ( -- )     a word from or to a port
\   x1 ... xn n id BIOS ( -- ax f )   INT id; f true: the carry is set (an error)
\
\ DS and ES are the segment of the VM, so an address in BX, DX, SI or DI
\ is a VM address. The call runs on a stack of its own, not on the data
\ stack of the VM. Example: the memory of the PC in KB (INT 12h):
\   ALSO BIOS  0 $12 BIOS DROP .

PACKAGE BIOS   \ lib/package.fs

PRIVATE
: PRIM ( n "name" -- )   PARSE-NAME HEADER ,  REVEAL ;   \ a header for the primitive n

PUBLIC
100 PRIM SYSCALL ( x1 ... xn n id -- x )
110 PRIM PC@     ( port -- c )
111 PRIM PC!     ( c port -- )
112 PRIM PW@     ( port -- x )
113 PRIM PW!     ( x port -- )
$FFF0 CONSTANT REGS   \ AX BX CX DX SI DI flags ES after SYSCALL (doc/vm.md, section 3: free)
: BIOS ( x1 ... xn n id -- ax f )   SYSCALL  REGS 12 + @ 1 AND 0<> ;

END-PACKAGE
