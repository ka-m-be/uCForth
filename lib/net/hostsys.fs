\ hostsys.fs - the layer of the system on macOS and Linux: which one, and load it
\
\ Loads TEXT, INET and the package of the system that runs this
\ program (SYSTEM). A package that is there already does not load
\ again. The system: on Linux, the file /proc/self/exe is this program,
\ and its ELF header names the processor (aarch64, x86-64, i386); on
\ macOS, the file SystemVersion.plist exists. Each Mac takes the same
\ package: the HAL of macos-x64 adds the class of the call.
\
\ lib/net/host.fs loads this file, then the network of the system
\ (POSIX-NET, NET, DNS). The tests of our TCP/IP over a TAP device
\ (lib/net/test/tap.fs) load it too: they need SYSTEM, but NET over our
\ TCP/IP. At the end the search order is FORTH FILES.
\
\ Use (in the folder ucf):  ALSO FILES  INCLUDE lib/net/hostsys.fs

ONLY FORTH ALSO FILES
PACKAGE HOST-NET   \ lib/package.fs

PRIVATE
20 BUFFER: HEADER-BYTES
: MACHINE ( -- n )   \ e_machine in the ELF header of this program; 0: no Linux
  S" /proc/self/exe" R/O OPEN-FILE IF DROP 0 EXIT THEN
  >R  HEADER-BYTES 20 R@ READ-FILE 2DROP  R> CLOSE-FILE DROP
  HEADER-BYTES 18 + C@  HEADER-BYTES 19 + C@ 8 LSHIFT OR ;
: MACOS? ( -- f )
  S" /System/Library/CoreServices/SystemVersion.plist" R/O OPEN-FILE
  IF DROP FALSE EXIT THEN  CLOSE-FILE DROP TRUE ;

PUBLIC
: SYSTEM-LAYER ( -- a u )   \ the file of the package of the system
  MACHINE CASE
    183 OF S" lib/linux/a64.fs" ENDOF
     62 OF S" lib/linux/x64.fs" ENDOF
      3 OF S" lib/linux/i386.fs" ENDOF
    MACOS? 0= ABORT" no network for this system (lib/net/README.md)"
    S" lib/macos/system.fs" ROT
  ENDCASE ;

END-PACKAGE

ONLY FORTH ALSO FILES  [DEFINED] TEXT    S" lib/text.fs"      ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] INET    S" lib/net/inet.fs"  ?INCLUDED
ONLY FORTH ALSO FILES  ALSO HOST-NET  [DEFINED] SYSTEM  SYSTEM-LAYER  ?INCLUDED
ONLY FORTH ALSO FILES
