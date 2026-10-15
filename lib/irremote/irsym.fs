\ irsym.fs - package IR-SYMBOLS: IR times as a table of durations and a hex digit for each
\
\ A frame of a remote control has only a few different times: the header,
\ the mark of a bit, the spaces of a 0 and of a 1, the gap. So it can go as
\ a table of at most 16 durations in us and, for each time, one hex digit:
\ its place in the table. A frame of RTY3 (371 times) has 6 durations; as
\ JSON it takes 412 bytes instead of 1 549 for the times themselves.
\
\   t m a >SYMBOLS ( -- n true | false )   the m times at t as m hex digits
\                     at a, and the table of their n durations (DURATIONS);
\                     false: more than 16 durations
\   DURATIONS ( -- a )   the table of the last >SYMBOLS: n cells, in us
\   ta n sa u t SYMBOLS> ( -- m true | false )   the u hex digits at sa,
\                     with the table of n durations at ta, as m = u times
\                     at t; false: a digit names no duration of the table
\
\ How >SYMBOLS makes the table: the marks (the times at the places 0, 2,
\ 4 ...) and the spaces (1, 3 ...) have durations of their own, for they
\ come in turn. A time belongs to the first duration of its kind that is
\ at most 20 % (at least 80 us) away from the first time of that duration;
\ else it begins a new one. At the end each duration is the mean of its
\ times, rounded. The digits are 0-9 and A-F; SYMBOLS> takes a-f too. Runs on every
\ target.
\
\ Load: ALSO FILES S" lib/irremote/irsym.fs" INCLUDED (on the host), or in
\ ucf term: #include lib/irremote/irsym.fs (on a board). Example:
\
\   ALSO IR-SYMBOLS
\   CREATE F  9000 , 4500 , 560 , 560 , 560 , 1690 , 580 ,
\   CREATE S  8 ALLOT   CREATE BACK  8 CELLS ALLOT
\   F 7 S >SYMBOLS . .  S 7 TYPE         \ -1 5 0123242
\   DURATIONS 5 S 7 BACK SYMBOLS> . .    \ -1 7: BACK holds the times again,
\                                        \ the marks of the bits as 567

PACKAGE IR-SYMBOLS   \ lib/package.fs

16 CONSTANT #MAX                       \ the durations, at most
VARIABLE #USED                         \ the durations so far
#MAX CELLS BUFFER: TABLE               \ the first time of each duration, at the end their mean
#MAX BUFFER: KINDS                     \ the kind of each: 0 marks, 1 spaces
#MAX CELLS BUFFER: COUNTS              \ the number of its times
#MAX 2* CELLS BUFFER: SUMS             \ their sum: two cells, the low one first

: ALIKE? ( us ref -- f )   \ us is at most 20 % (at least 80 us) away from ref
  DUP 5 / 80 MAX  >R  - ABS  R> 1+ < ;
: DURATION? ( us kind -- i | -1 )   \ the first duration of this kind that is alike
  #USED @ 0 ?DO
    DUP I KINDS + C@ = IF  OVER I CELLS TABLE + @ ALIKE? IF  2DROP I UNLOOP EXIT  THEN  THEN
  LOOP  2DROP -1 ;
: NEW ( us kind -- i | -1 )   \ a new duration, if there is room
  #USED @ #MAX = IF  2DROP -1 EXIT  THEN
  #USED @ KINDS + C!  #USED @ CELLS TABLE + !  #USED @  1 #USED +! ;
: SYMBOL ( us kind -- i | -1 )   \ the duration of the time us
  2DUP DURATION? DUP 0< IF  DROP NEW  ELSE  NIP NIP  THEN ;
: ADD ( us i -- )   \ the time us belongs to the duration i
  1 OVER CELLS COUNTS + +!
  2* CELLS SUMS +  DUP >R @  OVER +  DUP R@ !        ( us low )
  U> IF  1 R@ CELL+ +!  THEN  R> DROP ;               \ the carry
: MEAN ( i -- us )   \ the mean of the times of the duration i, rounded
  DUP CELLS COUNTS + @ >R  2* CELLS SUMS +  DUP @ SWAP CELL+ @   ( low high )
  R@ UM/MOD  SWAP 2* R> < IF EXIT THEN  1+ ;
: MEANS ( -- )   \ each duration becomes the mean of its times
  #USED @ 0 ?DO  I MEAN  I CELLS TABLE + !  LOOP ;

VARIABLE TA   VARIABLE TN   VARIABLE OUT  \ SYMBOLS>: the table, its size, the times
: TIME-OF ( c -- us true | false )   \ the duration that the digit c names
  16 DIGIT?  OVER TN @ U< AND IF  CELLS TA @ + @ TRUE  ELSE  DROP FALSE  THEN ;

PUBLIC
: DURATIONS ( -- a )   TABLE ;
: >SYMBOLS ( t m a -- n true | false )
  0 #USED !  COUNTS #MAX CELLS 0 FILL  SUMS #MAX 2* CELLS 0 FILL
  SWAP 0 ?DO                                         ( t a )
    OVER I CELLS + @  DUP I 1 AND SYMBOL              ( t a us i )
    DUP 0< IF  2DROP 2DROP UNLOOP FALSE EXIT  THEN
    TUCK ADD  DIGIT  OVER I + C!
  LOOP  2DROP  MEANS  #USED @ TRUE ;
: SYMBOLS> ( ta n sa u t -- m true | false )
  OUT !  >R >R  TN ! TA !  R> R>                      ( sa u )
  TUCK 0 ?DO                                         ( u sa )
    DUP I + C@ TIME-OF 0= IF  2DROP UNLOOP FALSE EXIT  THEN
    OUT @ I CELLS + !
  LOOP  DROP TRUE ;

END-PACKAGE
