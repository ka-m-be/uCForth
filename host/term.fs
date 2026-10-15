\ term.fs - package TERM: the console to uCForth on a board
\
\ TERMINAL shows what the board sends and sends each typed line. It runs
\ the commands #include #require #when #reset #help #bye itself. a u
\ TERMINAL-INCLUDE does the same, after #include of the file a u (this is
\ ucf term PORT FILE). #include keeps
\ the handshake of the console (doc/vm.md, section 9): the next line goes
\ out when the prompt is there. Ctrl-C goes to the board as the byte 3:
\ the board stops the word that runs; a file that loads stops too.
\ The prompt of the board shows as "board ok> " (or "board c> "): so it
\ differs from the prompt "ok> " of the host.
\ Needs FILES TEXT SERIAL.

PACKAGE TERM  ALSO FILES  ALSO TEXT  ALSO SERIAL   \ lib/package.fs

\ ---- what the board sends ----------------------------------------------

256 BUFFER: IN-LINE   VARIABLE IN-N     \ the current line from the board
VARIABLE SHOWN          \ so many bytes of IN-LINE show already
VARIABLE BYTE-AT        \ MILLIS when the last byte came
256 BUFFER: ANSWER                      \ counted string: the last complete line
VARIABLE PROMPTED       \ the board waits at a prompt: 1 "ok> ", 2 "c> ", 0 no
VARIABLE RUNNING        \ a line went out, and its prompt has not come yet
VARIABLE QUIET          \ loading: prompts stay hidden, lines show when they end
VARIABLE MUTE           \ lines do not show at all
VARIABLE FAILED         \ a line began with "error:"
VARIABLE SYNCED         \ the line of READY came back
VARIABLE SKIP-LF        \ Ctrl-C went out: the new line of the board comes here already

: .PROMPT ( -- )   \ the prompt of the board, with the mark "board "
  PROMPTED @ IF ." board " THEN
  PROMPTED @ 1 = IF ." ok> " THEN  PROMPTED @ 2 = IF ." c> " THEN ;
: ERROR-LINE? ( a u -- f )  -LEADING  6 MIN  S" error:" S= ;
: SYNC-LINE? ( a u -- f )   \ the line ends with the mark of READY
  DUP 6 < IF 2DROP FALSE EXIT THEN  + 6 -  6 S" ~sync~" S= ;
: LINE-END ( -- )   \ a line from the board is complete
  IN-LINE IN-N @  2DUP ANSWER PLACE  0 IN-N !  0 SHOWN !
  2DUP ERROR-LINE? IF FAILED ON! THEN
  2DUP SYNC-LINE? IF  2DROP SYNCED ON!  0 PROMPTED !  EXIT  THEN
  QUIET @ MUTE @ 0= AND IF TYPE CR ELSE 2DROP THEN ;
: PROMPT? ( -- n )   \ 1: IN-LINE holds "ok> ", 2: "c> ", 0: neither
  IN-LINE IN-N @ S" ok> " S= IF 1 EXIT THEN
  IN-LINE IN-N @ S" c> " S= IF 2 EXIT THEN  0 ;
: PREFIX? ( a u a2 u2 -- f )   \ a u is the start of a2 u2
  >R OVER R> > IF 2DROP DROP FALSE EXIT THEN  OVER S= ;
: PROMPT-START? ( -- f )   \ the line so far can still become a prompt
  IN-LINE IN-N @ S" ok> " PREFIX?  IN-LINE IN-N @ S" c> " PREFIX?  OR ;
: SHOW-REST ( -- )   \ the bytes of the line that do not show yet
  IN-LINE SHOWN @ +  IN-N @ SHOWN @ -  TYPE  IN-N @ SHOWN ! ;
: FROM-BOARD ( c -- )   \ at the keyboard each byte shows at once, but a prompt only when complete
  DUP 13 = IF DROP EXIT THEN
  SKIP-LF @ IF  SKIP-LF OFF!  DUP 10 = IF DROP EXIT THEN  THEN
  MILLIS BYTE-AT !
  DUP 10 = IF  QUIET @ IF DROP ELSE SHOW-REST EMIT THEN  LINE-END EXIT  THEN
  IN-N @ 256 < 0= IF  QUIET @ IF DROP ELSE EMIT THEN  EXIT  THEN
  IN-LINE IN-N @ + C!  1 IN-N +!
  PROMPT? ?DUP IF
    PROMPTED !  RUNNING OFF!  QUIET @ 0= IF  SHOWN @ IF SHOW-REST ELSE .PROMPT THEN  THEN
    0 IN-N !  0 SHOWN !  EXIT
  THEN
  QUIET @ IF EXIT THEN
  SHOWN @ 0= PROMPT-START? AND IF EXIT THEN     \ perhaps a prompt: it waits
  SHOW-REST ;
: HELD? ( -- f )   \ the start of a line waits for 50 ms: it is no prompt
  QUIET @ 0=  IN-N @ SHOWN @ > AND  MILLIS BYTE-AT @ - 50 > AND ;
: POLL-BOARD ( -- f )   \ take one byte from the board; false if none is there
  RECV DUP 0< IF DROP FALSE EXIT THEN  FROM-BOARD TRUE ;

\ ---- lines to the board ------------------------------------------------

260 BUFFER: OUT-LINE
PUBLIC
10000 CONSTANT PATIENCE                 \ ms: so long a line may run until its prompt
PRIVATE
VARIABLE SENT-AT                        \ MILLIS when the last line went out
: SEND-LINE ( a u -- )   \ a line to the board, with CR at the end
  DUP 256 > ABORT" the line is too long (at most 256 characters)"
  TUCK OUT-LINE SWAP MOVE  13 OVER OUT-LINE + C!
  0 PROMPTED !  FAILED OFF!  RUNNING ON!  MILLIS SENT-AT !  OUT-LINE SWAP 1+ SEND ;
: WAIT-PROMPT ( -- f )   \ show the output until the prompt comes; false after PATIENCE
  MILLIS PATIENCE +
  BEGIN PROMPTED @ 0= WHILE
    POLL-BOARD 0= IF  DUP MILLIS - 0< IF DROP FALSE EXIT THEN  1 MS  THEN
  REPEAT  DROP TRUE ;
: READY ( -- )   \ wait until the board has done all lines and waits at a prompt
  QUIET @ >R  QUIET ON!  SYNCED OFF!
  S" .( ~sync~)" SEND-LINE                 \ each prompt before its answer is old
  BEGIN
    WAIT-PROMPT 0= IF  R> QUIET !  TRUE ABORT" no prompt from the board"  THEN
    SYNCED @ 0=
  WHILE  0 PROMPTED !  REPEAT  R> QUIET ! ;
: SHOW ( ms -- )   \ show what the board sends for ms
  MILLIS +  BEGIN POLL-BOARD 0= IF 1 MS THEN  DUP MILLIS - 0< UNTIL  DROP ;
256 BUFFER: QUERY
: DEFINED? ( a u -- f )   \ ask the board if it knows the word a u
  S" [DEFINED] " QUERY PLACE  QUERY +PLACE  S"  ." QUERY +PLACE  READY
  QUIET @ >R  QUIET ON!  MUTE ON!
  QUERY COUNT SEND-LINE  WAIT-PROMPT  MUTE OFF!  R> QUIET !
  0= ABORT" no prompt from the board"
  FAILED @ ABORT" the board does not know [DEFINED]"
  ANSWER COUNT -TRAILING S" 0" S= 0= ;

\ ---- files for #include --------------------------------------------------

8 CONSTANT NESTED-MAX                    \ an application, a board, a front file (#when) ...
VARIABLE NESTED                          \ files that load now, one in the other
NESTED-MAX CELLS BUFFER: FIDS
NESTED-MAX CELLS BUFFER: LINE#S
NESTED-MAX 256 * BUFFER: NAMES           \ counted strings
256 BUFFER: PATH                        \ counted string
260 BUFFER: TEXT-LINE

: CUR ( a -- a' )  NESTED @ 1- CELLS + ;  \ the entry of the file that loads now
: FILE-NAME ( -- a u )  NAMES NESTED @ 1- 256 * + COUNT ;
: DIR ( a u -- a u' )   \ the directory part of a path, with the last '/'
  BEGIN DUP WHILE  2DUP + 1- C@ '/' = IF EXIT THEN  1- REPEAT ;
: >PATH ( a u -- )   \ the name of a file, relative to the file that loads now
  0 PATH C!
  DUP IF  OVER C@ '/' <> NESTED @ 0<> AND IF FILE-NAME DIR PATH +PLACE THEN  THEN
  PATH +PLACE ;
: PUSH-FILE ( a u -- )   \ open the file a u; it loads next
  NESTED @ NESTED-MAX = ABORT" #include nests too deep"
  >PATH  PATH COUNT R/O OPEN-FILE IF  DROP S" cannot open " PATH COUNT FAIL  THEN
  1 NESTED +!  FIDS CUR !  0 LINE#S CUR !
  PATH COUNT  NAMES NESTED @ 1- 256 * + PLACE ;
: POP-FILE ( -- )  FIDS CUR @ CLOSE-FILE DROP  -1 NESTED +! ;
: STOP-LOADING ( -- )   \ name the place, close all files
  ."   " FILE-NAME TYPE ." :" LINE#S CUR @ N>S TYPE ." : loading stopped" CR
  BEGIN NESTED @ WHILE POP-FILE REPEAT ;
: NEXT-LINE ( -- a u true | false )   \ the next line of the file that loads now
  TEXT-LINE 257 FIDS CUR @ READ-LINE ABORT" cannot read the file"
  0= IF DROP FALSE EXIT THEN
  1 LINE#S CUR +!
  DUP 256 > ABORT" the line is too long (at most 256 characters)"
  TEXT-LINE SWAP TRUE ;

\ ---- commands of term ---------------------------------------------------

VARIABLE REST-A   VARIABLE REST-U       \ the rest of a command line
VARIABLE DONE                           \ #bye came
: REST ( -- a u )  REST-A @ REST-U @ ;
: NEXT-WORD ( -- a u )  REST SPLIT  2SWAP REST-U ! REST-A ! ;
: .HELP ( -- )
  ." #include FILE        send the file, a line after each prompt" CR
  ." #require WORD FILE   the same, if the board does not know WORD" CR
  ." #when WORD FILE      the same, if the board knows WORD" CR
  ." #reset               restart the board over RTS" CR
  ." Ctrl-C               stop the word that runs on the board" CR
  ." #bye                 end term" CR ;
: REQUIRE ( -- )   \ #require WORD FILE
  NEXT-WORD  REST -TRAILING  2SWAP DEFINED? IF 2DROP ELSE PUSH-FILE THEN ;
: WHEN ( -- )   \ #when WORD FILE
  NEXT-WORD  REST -TRAILING  2SWAP DEFINED? IF PUSH-FILE ELSE 2DROP THEN ;
: RESET-BOARD ( -- )   \ a line that runs ends first (FREEZE!); then the start, up to its prompt
  LINES? 0= ABORT" the port has no DTR and RTS"
  RUNNING @ IF WAIT-PROMPT DROP THEN
  PULSE-RTS  0 PROMPTED !  RUNNING OFF!  WAIT-PROMPT DROP ;      \ each at most PATIENCE
: COMMAND? ( a u -- f )   \ run the line if it is a command of term; false if not
  -LEADING  DUP 0= IF 2DROP FALSE EXIT THEN
  OVER C@ '#' <> IF 2DROP FALSE EXIT THEN
  1 /STRING  REST-U ! REST-A !  NEXT-WORD
  2DUP S" include" S= IF 2DROP REST -TRAILING PUSH-FILE TRUE EXIT THEN
  2DUP S" require" S= IF 2DROP REQUIRE TRUE EXIT THEN
  2DUP S" when"    S= IF 2DROP WHEN TRUE EXIT THEN
  2DUP S" reset"   S= IF 2DROP RESET-BOARD TRUE EXIT THEN
  2DUP S" help"    S= IF 2DROP .HELP TRUE EXIT THEN
  2DUP S" bye"     S= IF 2DROP 100 SHOW  DONE ON! TRUE EXIT THEN
  2DROP FALSE ;

: INCLUDE-STEP ( -- )   \ one line of the file that loads now
  NEXT-LINE 0= IF POP-FILE EXIT THEN
  2DUP -LEADING NIP 0= IF 2DROP EXIT THEN          \ blank lines stay here
  2DUP COMMAND? IF 2DROP EXIT THEN
  SEND-LINE
  WAIT-PROMPT 0= IF ."   error: no prompt from the board for 10 s" CR STOP-LOADING EXIT THEN
  FAILED @ IF STOP-LOADING THEN ;
CREATE CTRL-C  3 C,
: BREAK-BOARD ( -- )   \ the board stops the word that runs
  CTRL-C 1 SEND  SKIP-LF ON! ;          \ the break of the host has written a new line
: STOPPED ( n -- )   \ an error while files load; Ctrl-C (-28) goes to the board too
  DUP -28 = IF BREAK-BOARD THEN  .ERROR STOP-LOADING ;
: LOAD ( -- )   \ load the files that #include opened; show the prompt again
  QUIET ON!  ['] READY CATCH ?DUP IF STOPPED THEN
  BEGIN NESTED @ WHILE
    ['] INCLUDE-STEP CATCH ?DUP IF STOPPED THEN
  REPEAT
  QUIET OFF!  .PROMPT ;

\ ---- the terminal ---------------------------------------------------------

260 BUFFER: TYPED   VARIABLE TYPED-N    \ the line that the user types
: TYPED? ( -- a u true | false )   \ collect keys; true when a line is complete
  BEGIN KEY? WHILE
    KEY  DUP 10 = IF DROP  0 OUT !  TYPED TYPED-N @  0 TYPED-N !  TRUE EXIT THEN
    DUP 13 = IF DROP ELSE
      TYPED-N @ 257 < IF TYPED TYPED-N @ + C!  1 TYPED-N +! ELSE DROP THEN
    THEN
  REPEAT  FALSE ;
: TYPED-LINE ( a u -- )   \ a command of term, or a line for the board
  2DUP COMMAND? IF  2DROP  NESTED @ IF LOAD EXIT THEN  DONE @ 0= IF .PROMPT THEN  EXIT  THEN
  SEND-LINE ;
: TAKE-KEYS? ( -- f )   \ at the prompt, or the board runs a word for 0.5 s
  PROMPTED @  MILLIS SENT-AT @ - 500 > OR ;
: STEP ( -- )   \ show what the board sends, else take what the user types
  POLL-BOARD IF EXIT THEN
  HELD? IF SHOW-REST THEN
  TAKE-KEYS? IF  TYPED? IF                     \ Ctrl-C in a command (#reset waits) goes to the board too
    ['] TYPED-LINE CATCH ?DUP IF  DUP -28 = IF BREAK-BOARD THEN  .ERROR 2DROP  QUIET OFF!  MUTE OFF!  .PROMPT  THEN  EXIT
  THEN THEN  1 MS ;
: ?BREAK ( n -- )   \ Ctrl-C (-28) goes to the board; another error goes on
  DUP -28 <> IF THROW THEN  DROP  BREAK-BOARD  0 TYPED-N ! ;
: TERM-START ( -- )
  0 NESTED !  QUIET OFF!  MUTE OFF!  DONE OFF!  SKIP-LF OFF!
  0 PROMPTED !  RUNNING OFF!  0 IN-N !  0 SHOWN !  0 TYPED-N !  MILLIS SENT-AT !      \ a prompt may come first
  ." term on " PORT$ TYPE ." : #help shows the commands" CR ;
: TERM-RUN ( -- )
  BEGIN  ['] STEP CATCH ?DUP IF ?BREAK THEN  DONE @ UNTIL
  OUT @ IF CR THEN  ." term ends: back on the host" CR ;
PUBLIC
: TERMINAL ( -- )   \ the console to the board at the open port, until #bye
  TERM-START TERM-RUN ;
: TERMINAL-INCLUDE ( a u -- )   \ the same, after #include of the file a u
  TERM-START  ['] PUSH-FILE CATCH ?DUP IF  .ERROR 2DROP  ELSE LOAD THEN  TERM-RUN ;

END-PACKAGE
