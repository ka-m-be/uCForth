\ json.fs - package JSON: an object written into a buffer, and single fields read from a text
\
\ For the messages of the IR bridge (orga/Archiv/008_Vorschlag_J_JSON.md) and other
\ small programs that talk JSON over MQTT or HTTP. The writer puts one
\ object into a buffer of the caller, field after field. The reader needs
\ no tree and no memory per message: JSON-FIELD finds the value of a name in
\ an object, and the words JSON>... turn that value into a number, a flag,
\ a text, cells or bytes. As in MQTT, the fixed argument is on top: the name
\ of a field above its value.
\
\ Writing:
\   a u JSON-INTO ( -- )            a new object in the buffer a of u bytes
\   n ka ku JSON-NUMBER ( -- )      "name":n
\   sa su ka ku JSON-TEXT ( -- )    "name":"text"
\   f ka ku JSON-FLAG ( -- )        "name":true or false
\   a n ka ku JSON-NUMBERS ( -- )   "name":[...]: n cells at a
\   a n ka ku JSON-HEX ( -- )       "name":"...": n bytes at a, two hex digits each
\   ka ku JSON-OBJECT ( -- )        "name":{ ; JSON-END ( -- ) closes it
\   JSON-DONE ( -- a u ior )        all open objects closed: the text; 0, or
\                                   JSON-FULL (the buffer was too small)
\ Reading:
\   a u JSON-CHECK ( -- ior )       0: a u is valid JSON within the limits
\   a u ka ku JSON-FIELD ( -- va vu true | false )   the value of the name
\                                   ka ku in the top level of the object a u;
\                                   for an object inside: JSON-FIELD on its value
\   va vu JSON>NUMBER ( -- n ior )  a whole number
\   va vu JSON>FLAG ( -- f ior )    true or false
\   va vu b m JSON>TEXT ( -- b u ior )      a text, at most m bytes at b
\   va vu b m JSON>NUMBERS ( -- b n ior )   whole numbers, at most m cells at b
\   va vu b m JSON>HEX ( -- b n ior )       a hex text, at most m bytes at b
\
\ The limits: a text of at most 4096 bytes; at most 8 objects and arrays
\ inside each other; whole numbers in the range of a cell (32 bits:
\ -2147483647 to 2147483647; 16 bits: -32767 to 32767). A number with a
\ fraction or an exponent is valid JSON, but JSON>NUMBER gives JSON-LIMIT.
\ A text goes through as it is (UTF-8); JSON>TEXT knows \uXXXX up to $7F
\ only. If a name comes twice, the first counts. Between the parts of a
\ text each character below 33 is white space, as for -LEADING (JSON itself
\ knows only space, tab, CR and LF). The writer escapes " and \ and the
\ control characters (\b \f \n \r \t, the others as \u00XX), and it writes
\ hex digits in capitals; the reader takes both.
\
\ The codes: JSON-FULL (-470) the buffer is full; JSON-INVALID (-471) no
\ valid JSON; JSON-TYPE (-472) a value of another kind (a text, not a
\ number ...); JSON-LIMIT (-473) outside the limits above.
\
\ Example:
\   CREATE OUT 100 ALLOT
\   OUT 100 JSON-INTO  7 S" id" JSON-NUMBER  0 S" code" JSON-NUMBER
\   JSON-DONE . TYPE                \ 0 {"id":7,"code":0}
\ The tests: lib/test/json.fs.

PACKAGE JSON ALSO TEXT

PUBLIC
-470 CONSTANT JSON-FULL
-471 CONSTANT JSON-INVALID
-472 CONSTANT JSON-TYPE
-473 CONSTANT JSON-LIMIT
PRIVATE
4096 CONSTANT MAX-TEXT
8 CONSTANT MAX-DEPTH
-1 1 RSHIFT CONSTANT MAX-N               \ the largest number of a cell
CREATE ESCAPES  '"' C, '\' C, '/' C, 'b' C, 'f' C, 'n' C, 'r' C, 't' C,   \ the letters after a \
: SHORT ( -- a )   ESCAPES 3 + ;         \ b f n r t: they stand for the control characters
CREATE CONTROL  8 C, 12 C, 10 C, 13 C, 9 C,
CREATE OTHERS  '"' C, '{' C, '[' C, 't' C, 'f' C, 'n' C,   \ the first characters of other values than numbers
: OFFSET ( c a n -- i )   DUP >R ROT SCAN NIP R> SWAP - ;   \ the place of c in the n bytes at a; n: not there
: HEX-DIGIT ( u -- c )   DUP 10 < IF '0' ELSE 55 THEN + ;   \ 0 to 9, then A to F

\ ---- writing
VARIABLE OUT-A  VARIABLE OUT-U  VARIABLE OUT-N   \ the buffer, its size, the bytes in it
VARIABLE FULL   VARIABLE TOO-DEEP
VARIABLE DEEP                             \ the objects open, the first one too
VARIABLE COMMA                            \ a field came: the next one needs a comma
: +C ( c -- )   \ a byte into the buffer; without room FULL
  OUT-N @ OUT-U @ < IF  OUT-A @ OUT-N @ + C!  1 OUT-N +!  ELSE  DROP FULL ON!  THEN ;
: +S ( a u -- )   BOUNDS ?DO  I C@ +C  LOOP ;
: +N ( n -- )   DUP ABS 0 <# #S ROT SIGN #> +S ;
: +HEX ( c -- )   DUP 4 RSHIFT HEX-DIGIT +C  15 AND HEX-DIGIT +C ;   \ a byte as two hex digits
: +ESCAPED ( c -- )   \ a byte of a text, as JSON writes it
  DUP ESCAPES 2 OFFSET 2 < IF  '\' +C +C  EXIT  THEN          \ " and \
  DUP 32 < 0= IF  +C EXIT  THEN
  '\' +C  DUP CONTROL 5 OFFSET  DUP 5 < IF  NIP SHORT + C@ +C  EXIT  THEN  DROP
  'u' +C '0' +C '0' +C  +HEX ;
: +QUOTED ( a u -- )   '"' +C  BOUNDS ?DO  I C@ +ESCAPED  LOOP  '"' +C ;
: +NAME ( ka ku -- )   \ a comma before each field but the first of an object; then "name":
  COMMA @ IF ',' +C THEN  COMMA ON!  +QUOTED  ':' +C ;

PUBLIC
: JSON-INTO ( a u -- )
  OUT-U ! OUT-A !  0 OUT-N !  FULL OFF!  TOO-DEEP OFF!  1 DEEP !  COMMA OFF!  '{' +C ;
: JSON-NUMBER ( n ka ku -- )   +NAME +N ;
: JSON-TEXT ( sa su ka ku -- )   +NAME +QUOTED ;
: JSON-FLAG ( f ka ku -- )   +NAME IF S" true" ELSE S" false" THEN +S ;
: JSON-NUMBERS ( a n ka ku -- )   \ the first number, then a comma before each one more
  +NAME '[' +C  CELLS BOUNDS  2DUP <> IF  DUP @ +N  CELL+  THEN
  ?DO  ',' +C  I @ +N  CELL +LOOP  ']' +C ;
: JSON-HEX ( a n ka ku -- )   +NAME '"' +C  BOUNDS ?DO  I C@ +HEX  LOOP  '"' +C ;
: JSON-OBJECT ( ka ku -- )
  +NAME '{' +C  COMMA OFF!  1 DEEP +!  DEEP @ MAX-DEPTH > IF TOO-DEEP ON! THEN ;
: JSON-END ( -- )   DEEP @ 1 > IF  '}' +C  -1 DEEP +!  COMMA ON!  THEN ;
: JSON-DONE ( -- a u ior )
  BEGIN DEEP @ WHILE  '}' +C  -1 DEEP +!  REPEAT
  OUT-A @ OUT-N @  FULL @ IF JSON-FULL ELSE TOO-DEEP @ IF JSON-LIMIT ELSE 0 THEN THEN ;
PRIVATE

\ ---- reading: each word goes along the text a u, and THROWs on an error
VARIABLE LEVEL                            \ objects and arrays open while a value is skipped
: BAD ( -- )   JSON-INVALID THROW ;
: NEXT? ( a u -- a u c )   DUP IF OVER C@ ELSE 0 THEN ;   \ the next character; 0 at the end
: PEEK ( a u -- a u c )   DUP 0= IF BAD THEN  OVER C@ ;
: TAKE ( a u -- a' u' c )   PEEK >R 1/STRING R> ;
: EXPECT ( a u c -- a' u' )   >R PEEK R> <> IF BAD THEN  1/STRING ;
: OPEN ( va vu c -- a u )   \ the value begins with c: past it; else JSON-TYPE
  >R -LEADING NEXT? R> <> IF JSON-TYPE THROW THEN  1/STRING ;
: DIGIT-CHAR? ( c -- f )   '0' '9' 1+ WITHIN ;
: HEX-VALUE ( c -- u f )   \ the value of a hex digit, in capitals or not
  DUP DIGIT-CHAR? IF  '0' - TRUE EXIT  THEN
  32 OR  DUP 'a' 'f' 1+ WITHIN IF  87 - TRUE EXIT  THEN  FALSE ;
: HEX4 ( a u -- a' u' x )   \ the 4 hex digits after \u
  0  4 0 DO  >R  PEEK HEX-VALUE 0= IF BAD THEN  R> 4 LSHIFT OR  >R 1/STRING R>  LOOP ;
: SKIP-ESCAPE ( a u -- a' u' )   \ after the \ in a text
  TAKE  DUP 'u' = IF  DROP HEX4 DROP  EXIT  THEN
  ESCAPES 8 OFFSET 8 < 0= IF BAD THEN ;
: SKIP-TEXT ( a u -- a' u' )   \ after the " of a text: up to its end, the " too
  BEGIN
    TAKE  DUP '"' = IF  DROP EXIT  THEN
    DUP 32 < IF BAD THEN
    '\' = IF SKIP-ESCAPE THEN
  AGAIN ;
: SKIP-DIGITS ( a u -- a' u' n )   \ n digits
  0 >R  BEGIN  NEXT? DIGIT-CHAR?  WHILE  1/STRING  R> 1+ >R  REPEAT  R> ;
: DIGITS+ ( a u -- a' u' )   SKIP-DIGITS 0= IF BAD THEN ;   \ at least one digit
: SKIP-NUMBER ( a u -- a' u' )   \ -? (0 | 1-9 digits) (. digits)? (e (+ | -)? digits)?
  NEXT? '-' = IF 1/STRING THEN
  NEXT? '0' = IF  1/STRING  ELSE  DIGITS+  THEN
  NEXT? '.' = IF  1/STRING DIGITS+  THEN
  NEXT? 32 OR 'e' = IF
    1/STRING  NEXT? DUP '+' = SWAP '-' = OR IF 1/STRING THEN  DIGITS+
  THEN ;
VARIABLE KW-A  VARIABLE KW-U
: KEYWORD ( a u ka ku -- a' u' )   \ the text begins with ka ku: true, false or null
  KW-U ! KW-A !  2DUP KW-U @ MIN  KW-A @ KW-U @ STR= 0= IF BAD THEN  KW-U @ /STRING ;
DEFER SKIP-VALUE ( a u -- a' u' )
: DEEPER ( -- )   1 LEVEL +!  LEVEL @ MAX-DEPTH > IF JSON-LIMIT THROW THEN ;
: SKIP-MEMBERS ( a u -- a' u' )   \ after the { of an object: "name":value, ... and the }
  DEEPER  -LEADING  PEEK '}' = IF  1/STRING  -1 LEVEL +!  EXIT  THEN
  BEGIN
    -LEADING '"' EXPECT SKIP-TEXT  -LEADING ':' EXPECT  SKIP-VALUE  -LEADING
    PEEK ',' = WHILE  1/STRING
  REPEAT
  '}' EXPECT  -1 LEVEL +! ;
: SKIP-ELEMENTS ( a u -- a' u' )   \ after the [ of an array: value, ... and the ]
  DEEPER  -LEADING  PEEK ']' = IF  1/STRING  -1 LEVEL +!  EXIT  THEN
  BEGIN  SKIP-VALUE -LEADING  PEEK ',' = WHILE  1/STRING  REPEAT
  ']' EXPECT  -1 LEVEL +! ;
: (SKIP-VALUE) ( a u -- a' u' )
  -LEADING PEEK CASE
    '"' OF  1/STRING SKIP-TEXT  ENDOF
    '{' OF  1/STRING SKIP-MEMBERS  ENDOF
    '[' OF  1/STRING SKIP-ELEMENTS  ENDOF
    't' OF  S" true" KEYWORD  ENDOF
    'f' OF  S" false" KEYWORD  ENDOF
    'n' OF  S" null" KEYWORD  ENDOF
    >R SKIP-NUMBER R>
  ENDCASE ;
' (SKIP-VALUE) IS SKIP-VALUE
: SIZE? ( u -- )   MAX-TEXT > IF JSON-LIMIT THROW THEN ;
: END? ( a u -- )   -LEADING NIP IF BAD THEN ;   \ after a value only white space
: WHOLE ( a u -- )   DUP SIZE?  0 LEVEL !  SKIP-VALUE END? ;   \ one value

\ ---- a field of an object
VARIABLE KEY-A  VARIABLE KEY-U  VARIABLE VAL-A  VARIABLE VAL-U  VARIABLE MATCH  VARIABLE FOUND
: MEMBER ( a u -- a' u' )   \ "name":value; MATCH: the name is the key; VAL-A VAL-U: the value
  -LEADING '"' EXPECT  OVER >R  SKIP-TEXT
  OVER 1- R@ -  R> SWAP  KEY-A @ KEY-U @ STR= MATCH !
  -LEADING ':' EXPECT  -LEADING  OVER VAL-A !  SKIP-VALUE  OVER VAL-A @ - VAL-U ! ;
: FIELD ( a u -- )   \ through the object a u up to the member with the key: FOUND, VAL-A VAL-U
  FOUND OFF!  DUP SIZE?  0 LEVEL !
  -LEADING '{' EXPECT  -LEADING  PEEK '}' = IF  2DROP EXIT  THEN
  BEGIN
    MEMBER  MATCH @ IF  2DROP FOUND ON!  EXIT  THEN
    -LEADING  PEEK ',' = WHILE  1/STRING
  REPEAT
  '}' EXPECT 2DROP ;

\ ---- the values
: ACCUMULATE ( n c -- n' )   \ n * 10 + the digit c; JSON-LIMIT above MAX-N
  '0' -  OVER MAX-N 10 / > IF JSON-LIMIT THROW THEN
  SWAP 10 *  2DUP MAX-N SWAP - > IF JSON-LIMIT THROW THEN  + ;
: WHOLE-NUMBER ( a u -- a' u' n )   \ a whole number at the start of a u
  -LEADING  NEXT? OTHERS 6 OFFSET 6 < IF JSON-TYPE THROW THEN
  OVER >R  SKIP-NUMBER  OVER R@ -  R> SWAP              \ the number: its text below a' u'
  OVER C@ '-' = DUP >R IF 1/STRING THEN
  0 ROT ROT BOUNDS ?DO
    I C@ DIGIT-CHAR? 0= IF JSON-LIMIT THROW THEN        \ a fraction or an exponent
    I C@ ACCUMULATE
  LOOP  R> IF NEGATE THEN ;
: TRY ( va vu xt -- x ior )   CATCH  DUP IF  >R 2DROP 0 R>  THEN ;   \ an error: 0 and the code
: (JSON>NUMBER) ( va vu -- n )   WHOLE-NUMBER >R END? R> ;
: (JSON>FLAG) ( va vu -- f )
  -LEADING  NEXT? 't' = IF  S" true" KEYWORD TRUE
  ELSE  NEXT? 'f' = IF  S" false" KEYWORD FALSE  ELSE  JSON-TYPE THROW  THEN  THEN
  >R END? R> ;
VARIABLE TO-A  VARIABLE TO-M  VARIABLE TO-N     \ the place of a result: m at most, n in it
: +TO ( c -- )   TO-N @ TO-M @ < 0= IF JSON-LIMIT THROW THEN  TO-A @ TO-N @ + C!  1 TO-N +! ;
: UNESCAPE ( a u -- a' u' c )   \ after the \ in a text: the character that it stands for
  TAKE  DUP 'u' = IF  DROP HEX4  DUP $7F > IF JSON-LIMIT THROW THEN  EXIT  THEN
  DUP SHORT 5 OFFSET  DUP 5 < IF  NIP CONTROL + C@  EXIT  THEN  DROP
  DUP ESCAPES 3 OFFSET 3 < 0= IF BAD THEN ;          \ " \ / stand for themselves
: (JSON>TEXT) ( va vu -- )   \ the text, the escapes resolved, into TO-A
  '"' OPEN
  BEGIN  TAKE  DUP '"' <>  WHILE
    DUP 32 < IF BAD THEN
    DUP '\' = IF  DROP UNESCAPE  THEN  +TO
  REPEAT  DROP  END? ;
: (JSON>HEX) ( va vu -- )   \ a text of hex digits, two for each byte, into TO-A
  '"' OPEN
  BEGIN  PEEK '"' <>  WHILE
    TAKE HEX-VALUE 0= IF JSON-TYPE THROW THEN  4 LSHIFT >R
    TAKE HEX-VALUE 0= IF JSON-TYPE THROW THEN  R> OR +TO
  REPEAT  1/STRING  END? ;
: (JSON>NUMBERS) ( va vu -- )   \ an array of whole numbers into the cells at TO-A
  '[' OPEN  -LEADING
  PEEK ']' = IF  1/STRING  ELSE
    BEGIN
      WHOLE-NUMBER  TO-N @ TO-M @ < 0= IF JSON-LIMIT THROW THEN
      TO-A @ TO-N @ CELLS + !  1 TO-N +!
      -LEADING  PEEK ',' = WHILE  1/STRING
    REPEAT
    ']' EXPECT
  THEN  END? ;
: INTO ( va vu b m xt -- b n ior )   \ xt reads the value va vu into the place b m
  >R  TO-M ! TO-A ! 0 TO-N !  R> CATCH  DUP IF  >R 2DROP 0 TO-N ! R>  THEN
  >R TO-A @ TO-N @ R> ;

PUBLIC
: JSON-CHECK ( a u -- ior )   ['] WHOLE CATCH  DUP IF  >R 2DROP R>  THEN ;
: JSON-FIELD ( a u ka ku -- va vu true | false )
  KEY-U ! KEY-A !  ['] FIELD CATCH IF  2DROP FALSE EXIT  THEN
  FOUND @ IF  VAL-A @ VAL-U @ TRUE  ELSE  FALSE  THEN ;
: JSON>NUMBER ( va vu -- n ior )   ['] (JSON>NUMBER) TRY ;
: JSON>FLAG ( va vu -- f ior )   ['] (JSON>FLAG) TRY ;
: JSON>TEXT ( va vu b m -- b u ior )   ['] (JSON>TEXT) INTO ;
: JSON>HEX ( va vu b m -- b n ior )   ['] (JSON>HEX) INTO ;
: JSON>NUMBERS ( va vu b m -- b n ior )   ['] (JSON>NUMBERS) INTO ;
END-PACKAGE
