\ send.fs - package SEND: send source text to a target, line by line
\
\ A target with a serial console (a board, or QEMU) reads each line with
\ ACCEPT. The terminal sends the next line only after the prompt
\ (doc/vm.md, section 9). This package is such a terminal: it sends a
\ line with CR at the end, then shows what comes back until the prompt
\ "ok> " or "c> " comes. It counts the lines that contain "error:".
\
\ The line to the target is two files: one to write, one to read. For
\ QEMU with "-serial pipe:ser" these are the FIFOs ser.in and ser.out.
\
\   a1 u1 a2 u2 CONNECT     a1 u1: the file to write, a2 u2: to read;
\                           then wait for the first prompt
\   a u SEND-LINE           send one line, wait for the prompt
\   a u SEND-FILE           send each line of the file a u
\   ERRORS ( -- n )         the lines with "error:" so far
\
\ Use: see targets/rpi4/test.fs.

PACKAGE SEND  ALSO FILES   \ lib/package.fs

PUBLIC
VARIABLE ERRORS

PRIVATE
VARIABLE TX-FID   VARIABLE RX-FID
CREATE C1 1 ALLOT
CREATE LAST 8 ALLOT              \ the last 8 bytes that came back
CREATE LINE 256 ALLOT            \ the line that is sent now
CREATE SRC 256 ALLOT   VARIABLE SRC-FID
CREATE ERR 6 ALLOT               \ the text "error:"
S" error:" ERR SWAP MOVE
VARIABLE ERR#                    \ how many bytes of "error:" came in order

: RX ( -- c )   \ the next byte from the target
  C1 1 RX-FID @ READ-FILE THROW  0= ABORT" the target closed the line"  C1 C@ ;
: REMEMBER ( c -- )   LAST 1+ LAST 7 MOVE  LAST 7 + C! ;
: ENDS? ( a u -- f )   \ the last bytes are the text a u
  >R  LAST 8 + R@ -  R> SAME? ;
: CHECK-ERROR ( c -- )   \ count "error:"
  DUP ERR ERR# @ + C@ = IF
    DROP  1 ERR# +!  ERR# @ 6 = IF 1 ERRORS +!  0 ERR# ! THEN
  ELSE  ERR C@ = IF 1 ELSE 0 THEN ERR# !  THEN ;
: PROMPT? ( -- f )   S" ok> " ENDS?  S" c> " ENDS? OR ;

PUBLIC
: WAIT-PROMPT ( -- )   \ show the bytes from the target until the prompt
  BEGIN  RX  DUP EMIT  DUP CHECK-ERROR  REMEMBER  PROMPT? UNTIL ;

: CONNECT ( a1 u1 a2 u2 -- )
  R/O OPEN-FILE THROW RX-FID !  W/O OPEN-FILE THROW TX-FID !
  LAST 8 0 FILL  0 ERRORS !  0 ERR# !  WAIT-PROMPT ;

: SEND-LINE ( a u -- )
  255 MIN  DUP >R  LINE SWAP MOVE  13 LINE R@ + C!
  LINE R> 1+ TX-FID @ WRITE-FILE THROW  LAST 8 0 FILL  WAIT-PROMPT ;

: SEND-FILE ( a u -- )
  R/O OPEN-FILE THROW SRC-FID !
  BEGIN  SRC 255 SRC-FID @ READ-LINE THROW  WHILE  SRC SWAP SEND-LINE  REPEAT
  DROP  SRC-FID @ CLOSE-FILE THROW ;

END-PACKAGE
