\ frames.fs - package BRIDGE-FRAMES: the IR frames of the bridge in JSON
\
\ The texts of the messages events and send of the bridge (bridge.fs,
\ apps/irbridge/README.md): a frame heard as a table of durations and a
\ hex digit for each time (IR-SYMBOLS), and a request by its schema: raw
\ (the same form, or the times), pd (the pulse distance code of IRCODE)
\ and rty3 (the state of the air conditioner, RTY3). The state goes into
\ the status as "ac".
\
\   t m FRAME-FIELDS ( -- )   into the object that JSON writes now: "t" and
\                    "s", or "times" for a frame of more than 16 durations
\   pa pu REQUEST-FRAME ( -- t m ior )   the frame of the request pa pu:
\                    m times at t; ior: 0 or a code
\   AC-FIELDS ( -- )   "ac":{"power":...,"mode":...,"temp":...,"fan":...,
\                    "flap":...,"clock":"hh:mm"}, the state of RTY3; "clock"
\                    only when it is set
\
\ The codes: BRIDGE-NO-FRAME (-480) a field of the frame is missing;
\ BRIDGE-UNKNOWN (-481) no such schema; BRIDGE-BAD-FIELD (-482) a field does
\ not fit: a value out of range, a digit outside the table, a shape without
\ 6 or 7 times, blocks with fewer bytes than hex. A field of another kind
\ (a text for a number ...) gives the code of JSON. A request for rty3
\ changes the state only if all its fields fit.
\
\ Load it after JSON, AC-BASE, IRCODE, RTY3 and IR-SYMBOLS, before
\ bridge.fs (apps/irbridge/app.fs does it). Test on the host:
\ apps/irbridge/test/bridge.fs.

PACKAGE BRIDGE-FRAMES  ALSO JSON  ALSO TEXT  ALSO AC-BASE  ALSO IRCODE  ALSO RTY3  ALSO IR-SYMBOLS

PUBLIC
-480 CONSTANT BRIDGE-NO-FRAME
-481 CONSTANT BRIDGE-UNKNOWN
-482 CONSTANT BRIDGE-BAD-FIELD

PRIVATE
400 CONSTANT MAX-TIMES   MAX-TIMES CELLS BUFFER: TIMES     \ the times of a request
MAX-TIMES BUFFER: SYMS                                     \ the hex digits of a frame: "s"
16 CELLS BUFFER: REQ-TABLE                                 \ the durations of a request: "t"
16 BUFFER: REQ-TEXT                                        \ a short text of a request
24 CONSTANT MAX-BYTES   MAX-BYTES BUFFER: PD-BYTES         \ pd: "hex"; 24 bytes give at most 400 times
7 CELLS BUFFER: PD-SHAPE                                   \ pd: hm hs bm one zero gap blocks, as IRCODE
4 CONSTANT MAX-BLOCKS   MAX-BLOCKS CELLS BUFFER: PD-COUNTS \ pd: "blocks"
MAX-BLOCKS 1+ BUFFER: PD-BLOCKS                            \ the same as IRCODE takes it: a counted list
VARIABLE END-MARK                                          \ pd: the last light; 0: that of a bit
VARIABLE TRYING                                            \ rty3: the first pass only checks

\ ---- the fields of a request
: ?BAD ( f -- )   IF  BRIDGE-BAD-FIELD THROW  THEN ;
: HAS? ( pa pu ka ku -- pa pu va vu true | pa pu false )   >R >R  2DUP  R> R> JSON-FIELD ;
: NEEDS ( pa pu ka ku -- va vu )   JSON-FIELD 0= IF  BRIDGE-NO-FRAME THROW  THEN ;
: TEXT-OF ( va vu -- a u )   REQ-TEXT 16 JSON>TEXT THROW ;
: NUMBER-OF ( va vu -- n )   JSON>NUMBER THROW ;

\ ---- raw: "t" and "s", or "times"
: RAW-FRAME ( pa pu -- t m )
  S" s" HAS? IF
    SYMS MAX-TIMES JSON>TEXT THROW  2SWAP  S" t" NEEDS            ( sa su va vu )
    REQ-TABLE 16 JSON>NUMBERS THROW  2SWAP                         ( ta n sa su )
    TIMES SYMBOLS> 0= ?BAD  TIMES SWAP EXIT
  THEN
  S" times" NEEDS  TIMES MAX-TIMES JSON>NUMBERS THROW ;

\ ---- pd: the pulse distance code of IRCODE
: PD-SHAPE! ( pa pu -- )   \ "shape": hm hs bm one zero gap, and the last light as a 7th
  S" shape" NEEDS  PD-SHAPE 7 JSON>NUMBERS THROW  NIP              ( n )
  DUP 6 8 WITHIN 0= ?BAD
  7 = IF  PD-SHAPE 6 CELLS + @  ELSE  0  THEN  END-MARK !
  PD-BLOCKS PD-SHAPE 6 CELLS + ! ;
: PD-BLOCKS! ( pa pu k -- )   \ "blocks": the bytes of each block; without it one block of all k
  >R  S" blocks" JSON-FIELD 0= IF  1 PD-BLOCKS C!  R> PD-BLOCKS 1+ C!  EXIT  THEN
  PD-COUNTS MAX-BLOCKS JSON>NUMBERS THROW                         ( a n )
  DUP PD-BLOCKS C!  0 SWAP 0 ?DO                                  ( a sum )
    OVER I CELLS + @  DUP 1 MAX-BYTES 1+ WITHIN 0= ?BAD
    DUP PD-BLOCKS 1+ I + C!  +
  LOOP  NIP  R> < ?BAD ;                                          \ the blocks take all bytes
: PD-FRAME ( pa pu -- t m )
  2DUP PD-SHAPE!
  2DUP S" hex" NEEDS  PD-BYTES MAX-BYTES JSON>HEX THROW  NIP       ( pa pu k )
  DUP 0= ?BAD  DUP >R  PD-BLOCKS!  R>                              ( k )
  PD-SHAPE SHAPE-NOW !  PD-BYTES SWAP TIMES >TIMINGS               ( m )
  END-MARK @ ?DUP IF  OVER 1- CELLS TIMES + !  THEN  TIMES SWAP ;

\ ---- rty3: the state of the air conditioner (RTY3)
: >MODE ( va vu -- mode )   \ "heat" ... "fan"
  TEXT-OF  FAN 1+ 0 DO  2DUP I MODE-NAME STR= IF  2DROP I UNLOOP EXIT  THEN  LOOP  TRUE ?BAD ;
: 2DIGITS ( a -- n )   2 10 DIGITS 1 <> ?BAD ;
: >CLOCK ( va vu -- min )   \ "14:05"
  TEXT-OF  5 <>  OVER 2 + C@ ':' <> OR ?BAD
  DUP 2DIGITS  SWAP 3 + 2DIGITS  ?TIME  SWAP 60 * + ;
: SET ( x xt -- )   TRYING @ IF  2DROP  ELSE  EXECUTE  THEN ;
: AC-REQUEST ( pa pu -- pa pu )   \ each field: read and check it; set it unless TRYING
  S" clock" HAS? IF  >CLOCK  ['] SET-CLOCK SET  THEN
  S" mode" HAS? IF  >MODE  ['] AC-MODE! SET  THEN
  S" temp" HAS? IF  NUMBER-OF ?AC-TEMP  ['] AC-TEMP! SET  THEN
  S" fan" HAS? IF  NUMBER-OF ?AC-FAN  ['] AC-FAN! SET  THEN
  S" flap" HAS? IF  NUMBER-OF ?AC-FLAP  ['] AC-FLAP! SET  THEN
  S" power" HAS? IF  JSON>FLAG THROW  ['] AC-POWER! SET  THEN ;
: RTY3-FRAME ( pa pu -- t m )   \ first all fields are checked, then they change the state
  TRYING ON!  AC-REQUEST  TRYING OFF!  AC-REQUEST  2DROP  AC-TIMINGS ;

\ ---- the schemas
: SCHEMA ( pa pu -- t m )   \ the frame by "schema"; without it raw
  S" schema" HAS? 0= IF  RAW-FRAME EXIT  THEN
  TEXT-OF                                                          ( pa pu sa su )
  2DUP S" raw" STR= IF  2DROP RAW-FRAME EXIT  THEN
  2DUP S" pd" STR= IF  2DROP PD-FRAME EXIT  THEN
  S" rty3" STR= IF  RTY3-FRAME EXIT  THEN
  BRIDGE-UNKNOWN THROW ;

PUBLIC
: FRAME-FIELDS ( t m -- )
  2DUP SYMS >SYMBOLS IF                                            ( t m n )
    DURATIONS SWAP S" t" JSON-NUMBERS  NIP  SYMS SWAP S" s" JSON-TEXT
  ELSE  S" times" JSON-NUMBERS  THEN ;
: REQUEST-FRAME ( pa pu -- t m ior )   \ an ABORT" of RTY3 or AC-BASE (-2): -482
  ['] SCHEMA CATCH ?DUP IF  NIP NIP  DUP -2 = IF  DROP BRIDGE-BAD-FIELD  THEN  0 0 ROT  EXIT  THEN  0 ;
: AC-FIELDS ( -- )
  S" ac" JSON-OBJECT
  AC-POWER@ S" power" JSON-FLAG  AC-MODE@ MODE-NAME S" mode" JSON-TEXT
  AC-TEMP@ S" temp" JSON-NUMBER  AC-FAN@ S" fan" JSON-NUMBER  AC-FLAP@ S" flap" JSON-NUMBER
  CLOCK-SET @ IF  NOW HM>S S" clock" JSON-TEXT  THEN  JSON-END ;

END-PACKAGE
