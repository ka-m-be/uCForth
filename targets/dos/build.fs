\ build.fs - build uCForth for MS-DOS: build/ucf.com, kernel.com, ucf.scr, ucf.lst
\
\ Use (in the folder ucf), with a bootstrap VM with 16-bit cells (the
\ default), for example:
\   build/ucf-c host/files.fs [APP ...] targets/dos/build.fs </dev/null
\ On DOS the same with UCF.COM (the files of uCForth on the disk):
\   UCF HOST\FILES.FS TARGETS\DOS\BUILD.FS
\
\ 1. FREEZE: the running system is the image: base, the package FILES
\    and the application files before build.fs. It goes to ucf.hex.
\ 2. The assembler (asm/8086.fs) makes the kernel (kernels/8086.fs) and
\    the HAL of DOS (hal.fs) at $100, the address of a .COM program.
\    LISTED loads these two files line by line and notes the address of
\    each line, for the listing.
\ 3. The files, all in build/:
\    kernel.com  the kernel alone: the bootstrap VM without Genesis; it
\                needs "-image FILE"
\    ucf.scr     the same as a script for DEBUG: "DEBUG < UCF.SCR" makes
\                UCF.COM on a DOS computer, with no other program
\    ucf.lst     the listing: address, bytes and source line of the kernel
\    ucf.com     the kernel and the image: the program for DOS
\ bootstrap/dos/ keeps a copy of ucf.scr and ucf.lst (its README).

FREEZE

ONLY FORTH DEFINITIONS  ALSO FILES  S" lib/text.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" lib/ihex.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" host/image.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" asm/8086.fs" INCLUDED
ONLY FORTH  ALSO FILES  ALSO TEXT  ALSO IHEX  ALSO IMAGE  ALSO I8086  ALSO INTERNALS
VOCABULARY DOS-BUILD  ALSO DOS-BUILD DEFINITIONS

\ ==== LISTED: load a file and note the address of each line ============

1100 CONSTANT #LISTED
CREATE LINE-ADDR  #LISTED CELLS ALLOT      \ THERE at the start of each line
VARIABLE #LINES   0 #LINES !
CREATE LSAVE 256 ALLOT                     \ the line of build.fs that runs LISTED
VARIABLE LFID   VARIABLE LNO   CREATE LNAME 64 ALLOT

: LISTED-LINES ( -- )   \ interpret the lines of the file LFID
  BEGIN  TIB 256 LFID @ READ-LINE THROW  WHILE
    #LINES @ #LISTED < 0= ABORT" too many lines for the listing"
    THERE  LINE-ADDR #LINES @ CELLS + !  1 #LINES +!  1 LNO +!
    #TIB !  0 >IN !  INTERPRET
  REPEAT  DROP ;
: LISTED ( a u -- )   \ load the file a u as INCLUDED does; note THERE for each line
  2DUP 63 MIN LNAME PLACE  R/O OPEN-FILE THROW LFID !  0 LNO !
  TIB LSAVE 256 MOVE  #TIB @ >R  >IN @ >R
  ['] LISTED-LINES CATCH
  LFID @ CLOSE-FILE DROP
  LSAVE TIB 256 MOVE  R> >IN !  R> #TIB !
  ?DUP IF
    DUP -1 <> IF DUP .ERROR THEN  DROP
    ."   in " LNAME COUNT TYPE ." , line " LNO @ 0 .R CR  -1 THROW
  THEN ;

\ ==== The code ==========================================================

$100 ORG
S" kernels/8086.fs" LISTED
S" targets/dos/hal.fs" LISTED
FILL-TABLE  CHECK-LABELS
THERE CONSTANT CODE-END                    \ the first address after the kernel
TDP @ CONSTANT CODE-SIZE

\ ==== The check for the script: it follows the kernel ===================
\ DEBUG runs it with G. It adds the bytes of the kernel (x = x rol 1 +
\ byte), then shows a message; CX gets the length of the kernel, or 0
\ if a byte is wrong. W then writes CX bytes to UCF.COM.

: ROL1 ( x -- x' )   DUP 2* SWAP 15 RSHIFT OR  $FFFF AND ;
: SUM-OF-KERNEL ( -- x )
  0  CODE-SIZE 0 DO  ROL1  TARGET I + C@ +  $FFFF AND  LOOP ;

LABEL CHECK
  SI $100 MOVI,  CX CODE-SIZE MOVI,  DX DX XOR,  AH AH XOR,
  BEGIN,  DX 1 ROL,  LODSB,  DX AX ADD,  LOOP,
  DX SUM-OF-KERNEL CMPI,
  Z IF,  DX L1 MOVI,  CX CODE-SIZE MOVI,  ELSE,  DX L2 MOVI,  CX CX XOR,  THEN,
  AH 9 MOVI,  $21 INT,  BX BX XOR,  3 INT,
LABEL L1  S" UCF.COM: the check sum is right." TS,  13 TC, 10 TC,  '$' TC,
LABEL L2  S" UCF.COM: a byte is wrong; UCF.COM gets 0 bytes. Check the script." TS,
          13 TC, 10 TC,  '$' TC,
CHECK-LABELS
THERE CONSTANT CHECK-END

\ ==== Writing text files ================================================

CREATE CRLF 13 C, 10 C,
: PUT-LINE ( a u -- )   PUT  CRLF 2 PUT ;
CREATE HBUF 8 ALLOT
: HEX$ ( u n -- a u )   \ u as n hex digits
  >R  HBUF R@ +  R@ 0 DO  1- OVER 15 AND DIGIT OVER C!  SWAP 4 RSHIFT SWAP  LOOP  NIP R> ;
CREATE OBUF 64 ALLOT   VARIABLE O#
: O-START ( -- )   0 O# ! ;
: O+ ( a u -- )   TUCK OBUF O# @ + SWAP MOVE  O# +! ;
: O-C ( c -- )   OBUF O# @ + C!  1 O# +! ;
: O-TAB ( n -- )   BEGIN O# @ OVER < WHILE 32 O-C REPEAT DROP ;
: O-TEXT ( -- a u )   OBUF O# @ ;

\ ==== kernel.com and ucf.scr ============================================

: WRITE-KERNEL ( -- )
  S" build/kernel.com" WRITE-START  TARGET CODE-SIZE PUT  WRITE-END ;

VARIABLE SCR-LINES
: E-LINES ( ta u -- )   \ lines "E address bytes" for u bytes from ta, 16 on a line
  BEGIN  DUP WHILE
    O-START  S" E " O+  OVER 4 HEX$ O+
    DUP 16 MIN 0 DO  32 O-C  OVER I + TC@ 2 HEX$ O+  LOOP
    O-TEXT PUT-LINE  1 SCR-LINES +!
    DUP 16 MIN TUCK - >R + R>
  REPEAT  2DROP ;
: WRITE-SCRIPT ( -- )
  S" build/ucf.scr" WRITE-START  0 SCR-LINES !
  S" N UCF.COM" PUT-LINE
  $100 CODE-SIZE E-LINES
  CODE-END CHECK-END CODE-END - E-LINES
  O-START  S" G=" O+  CODE-END 4 HEX$ O+  O-TEXT PUT-LINE
  S" W" PUT-LINE  S" Q" PUT-LINE  4 SCR-LINES +!
  WRITE-END ;

\ ==== ucf.lst: the listing ==============================================
\ Each line: the address and up to 6 bytes (hex), then the source line.
\ More bytes of one source line follow on lines of their own.

VARIABLE LI                                \ the number of the line in LINE-ADDR
CREATE LBUF 258 ALLOT
: LINE-END ( i -- ta )   1+ DUP #LINES @ < IF CELLS LINE-ADDR + @ ELSE DROP CODE-END THEN ;
: PREFIX ( ta n -- )   \ the address and n bytes (at most 6) in OBUF, 26 columns
  O-START  DUP 0= IF 2DROP 26 O-TAB EXIT THEN
  OVER 4 HEX$ O+  32 O-C
  0 DO  32 O-C  DUP I + TC@ 2 HEX$ O+  LOOP  DROP  26 O-TAB ;
: LIST-FILE ( a u -- )   \ the lines of the file a u, each with its code
  O-START  S" \ ==== " O+  2DUP O+  O-TEXT PUT-LINE
  R/O OPEN-FILE THROW LFID !
  BEGIN  LBUF 256 LFID @ READ-LINE THROW  WHILE           ( u )
    LI @ CELLS LINE-ADDR + @  LI @ LINE-END  OVER -        ( u ta n )
    2DUP 6 MIN PREFIX  O-TEXT PUT  ROT LBUF SWAP PUT-LINE  ( ta n )
    BEGIN  6 - DUP 0 > WHILE
      SWAP 6 + SWAP  2DUP 6 MIN PREFIX  O-TEXT -TRAILING PUT-LINE
    REPEAT  2DROP
    1 LI +!
  REPEAT  DROP  LFID @ CLOSE-FILE THROW ;
: WRITE-LISTING ( -- )
  S" build/ucf.lst" WRITE-START  0 LI !
  S" \ ucf.lst - the listing of UCF.COM, the kernel of uCForth for DOS" PUT-LINE
  S" \" PUT-LINE
  S" \ Each line: the address in the segment of the program and the bytes" PUT-LINE
  S" \ there (hex), then the line of the source that made them. DEBUG shows" PUT-LINE
  S" \ the same: U 0100 the instructions, D 0100 the bytes. The assembler" PUT-LINE
  S" \ is asm/8086.fs; asm/README.md explains how to read it." PUT-LINE
  S" " PUT-LINE
  S" kernels/8086.fs" LIST-FILE  S" " PUT-LINE
  S" targets/dos/hal.fs" LIST-FILE
  WRITE-END ;

\ ==== ucf.com: the kernel and the image ================================
\ After the code: SYS ($100-$1FF), the RAM data ($200 up to VP), the
\ dictionary (S0 up to DP). COPY-IMAGE in hal.fs takes them from there.

VARIABLE D-END   VARIABLE DICT-END'
: RANGES ( -- )   \ D-END: the end of the RAM data; DICT-END': the end of the dictionary
  0 D-END !  0 DICT-END' !  IMAGE-OPEN
  BEGIN IMAGE-RECORD WHILE                             ( n addr )
    DUP S0 U< IF  + D-END @ UMAX D-END !  ELSE  + DICT-END' @ UMAX DICT-END' !  THEN
  REPEAT  IMAGE-CLOSE ;
VARIABLE A-LO   VARIABLE A-HI   VARIABLE A-POS
: PUT-AREA ( lo hi -- )   \ the image from address lo up to hi, as one block
  A-HI !  A-LO !  WRITTEN A-POS !  IMAGE-OPEN
  BEGIN IMAGE-RECORD WHILE                             ( n addr )
    DUP A-LO @ A-HI @ WITHIN IF
      A-LO @ -  A-POS @ +  PUT-ZEROS  IHEX-BUF SWAP PUT
    ELSE  2DROP  THEN
  REPEAT  IMAGE-CLOSE
  A-POS @ A-HI @ + A-LO @ -  PUT-ZEROS ;
: IMAGE-BYTES ( -- u )   256  D-END @ DATA-START - +  DICT-END' @ S0 - + ;
: WRITE-COM ( -- )
  RANGES
  IMAGE-OPEN  BOOT-XT @ XT-BOOT T!  QUIT-XT @ XT-QUIT T!  THROW-XT @ XT-THROW T!
  IMAGE-PRIMS  DUP PRIMS$ TC!  0 ?DO  DUP I + C@  PRIMS$ 1+ I + TC!  LOOP  DROP
  IMAGE-CLOSE
  IMAGE-BYTES  DUP IMAGE-LEN T!
  LINE-BUF CODE-END - U< 0= ABORT" the image is too big for UCF.COM"
  S" build/ucf.com" WRITE-START  TARGET CODE-SIZE PUT
  SYS-START DATA-START PUT-AREA
  DATA-START D-END @ PUT-AREA
  S0 DICT-END' @ PUT-AREA
  WRITE-END ;

: BUILD ( -- )
  WRITE-KERNEL  WRITE-SCRIPT  WRITE-LISTING  WRITE-COM
  ."   code: " CODE-SIZE . ." bytes at $100, image: " IMAGE-BYTES . ." bytes" CR
  ."   written: build/ucf.com (" WRITTEN 0 .R ."  bytes), build/kernel.com ("
  CODE-SIZE 0 .R ."  bytes)," CR
  ."   build/ucf.scr (" SCR-LINES @ 0 .R ."  lines), build/ucf.lst (" #LINES @ 0 .R ."  lines of source)" CR ;

BUILD
