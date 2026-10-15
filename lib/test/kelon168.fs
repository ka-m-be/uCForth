\ kelon168.fs - tests of the packages KELON168 and KELON168-READ (lib/irremote/kelon168.fs, kelonrd.fs)
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   ... host/files.fs forth/test/tester.fs lib/test/kelon168.fs
\ The expected frames F0 to F26 come from Kai's Python codec (Klima_IR,
\ clima/protocol/kelon168.py), used as an oracle. D1 is a frame of his
\ notes, D2 a frame of the tests of IRremoteESP8266 (a real remote
\ control RCH-R0Y3). The tests of the clock and the timers need 32-bit
\ cells (MILLIS).

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/irremote/acbase.fs" INCLUDED
S" lib/irremote/ircode.fs" INCLUDED
S" lib/irremote/irread.fs" INCLUDED
S" lib/irremote/kelon168.fs" INCLUDED
S" lib/irremote/kelonrd.fs" INCLUDED
S" lib/text.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO AC-BASE  ALSO AC-BASE.I
ALSO KELON168  ALSO KELON168.I  ALSO KELON168-READ  ALSO KELON168-READ.I
DECIMAL

: FRAME= ( a -- f )   21 AC-FRAME 21 STR= ;

CREATE F0  $83 C, $06 C, $04 C, $82 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $06 C, $00 C, $01 C, $00 C, $00 C, $28 C, $00 C, $29 C,
CREATE F1  $83 C, $06 C, $04 C, $82 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $06 C, $00 C, $01 C, $00 C, $00 C, $38 C, $00 C, $39 C,
CREATE F2  $83 C, $06 C, $00 C, $80 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $06 C, $00 C, $00 C, $38 C, $00 C, $3E C,
CREATE F3  $83 C, $06 C, $00 C, $70 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $F0 C, $00 C, $02 C, $00 C, $00 C, $38 C, $00 C, $3A C,
CREATE F4  $83 C, $06 C, $00 C, $02 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $82 C, $00 C, $02 C, $00 C, $00 C, $28 C, $00 C, $2A C,
CREATE F5  $83 C, $06 C, $00 C, $E2 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $62 C, $00 C, $02 C, $00 C, $00 C, $28 C, $00 C, $2A C,
CREATE F6  $83 C, $06 C, $00 C, $73 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $F3 C, $00 C, $06 C, $00 C, $00 C, $28 C, $00 C, $2E C,
CREATE F7  $83 C, $06 C, $00 C, $74 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $F4 C, $00 C, $06 C, $00 C, $00 C, $28 C, $00 C, $2E C,
CREATE F8  $83 C, $06 C, $00 C, $73 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $F3 C, $00 C, $02 C, $00 C, $00 C, $28 C, $00 C, $2A C,
CREATE F9  $83 C, $06 C, $00 C, $51 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $D1 C, $00 C, $06 C, $00 C, $00 C, $28 C, $00 C, $2E C,
CREATE F10  $83 C, $06 C, $00 C, $82 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $02 C, $00 C, $11 C, $00 C, $00 C, $28 C, $00 C, $39 C,
CREATE F11  $83 C, $06 C, $03 C, $82 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $01 C, $00 C, $11 C, $00 C, $00 C, $28 C, $00 C, $39 C,
CREATE F12  $83 C, $06 C, $03 C, $82 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $01 C, $00 C, $11 C, $00 C, $40 C, $28 C, $00 C, $79 C,
CREATE F13  $83 C, $06 C, $02 C, $82 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $11 C, $00 C, $00 C, $28 C, $00 C, $39 C,
CREATE F14  $83 C, $06 C, $01 C, $82 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $03 C, $00 C, $11 C, $00 C, $40 C, $28 C, $00 C, $79 C,
CREATE F15  $83 C, $06 C, $01 C, $82 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $03 C, $00 C, $11 C, $00 C, $00 C, $28 C, $00 C, $39 C,
CREATE F16  $83 C, $06 C, $01 C, $82 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $03 C, $00 C, $11 C, $02 C, $00 C, $28 C, $00 C, $3B C,
CREATE F17  $83 C, $06 C, $80 C, $82 C, $00 C, $00 C, $80 C, $00 C, $40 C, $00 C, $00 C, $00 C, $00 C, $C2 C, $00 C, $07 C, $00 C, $00 C, $28 C, $00 C, $2F C,
CREATE F18  $83 C, $06 C, $08 C, $82 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $0A C, $00 C, $03 C, $00 C, $00 C, $28 C, $00 C, $2B C,
CREATE F19  $83 C, $06 C, $00 C, $82 C, $00 C, $90 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $92 C, $00 C, $04 C, $00 C, $00 C, $28 C, $00 C, $2C C,
CREATE F20  $83 C, $06 C, $00 C, $82 C, $00 C, $00 C, $A0 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $22 C, $00 C, $00 C, $00 C, $00 C, $28 C, $00 C, $28 C,
CREATE F21  $83 C, $06 C, $82 C, $62 C, $00 C, $00 C, $8D C, $17 C, $40 C, $00 C, $00 C, $00 C, $00 C, $3A C, $00 C, $02 C, $00 C, $00 C, $38 C, $00 C, $3A C,
CREATE F22  $83 C, $06 C, $04 C, $80 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $04 C, $00 C, $01 C, $00 C, $00 C, $28 C, $00 C, $29 C,
CREATE F23  $83 C, $06 C, $00 C, $82 C, $00 C, $00 C, $8D C, $97 C, $16 C, $00 C, $00 C, $00 C, $00 C, $8E C, $00 C, $1D C, $00 C, $00 C, $28 C, $00 C, $35 C,
CREATE F24  $83 C, $06 C, $00 C, $82 C, $00 C, $00 C, $8D C, $17 C, $00 C, $80 C, $06 C, $1E C, $00 C, $80 C, $00 C, $05 C, $00 C, $00 C, $28 C, $00 C, $2D C,
CREATE F25  $83 C, $06 C, $00 C, $82 C, $00 C, $00 C, $8D C, $97 C, $16 C, $80 C, $06 C, $1E C, $00 C, $16 C, $00 C, $1D C, $00 C, $00 C, $28 C, $00 C, $35 C,
CREATE F26  $83 C, $06 C, $00 C, $82 C, $00 C, $00 C, $8D C, $17 C, $00 C, $00 C, $00 C, $00 C, $00 C, $18 C, $00 C, $1D C, $00 C, $00 C, $28 C, $00 C, $35 C,
CREATE F27  $83 C, $06 C, $50 C, $73 C, $00 C, $00 C, $80 C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $A3 C, $00 C, $06 C, $08 C, $00 C, $28 C, $00 C, $26 C,
CREATE D1  $83 C, $06 C, $82 C, $62 C, $00 C, $00 C, $8D C, $17 C, $40 C, $00 C, $00 C, $00 C, $00 C, $3A C, $00 C, $02 C, $00 C, $00 C, $38 C, $00 C, $3A C,
CREATE D2  $83 C, $06 C, $03 C, $70 C, $00 C, $00 C, $8D C, $17 C, $00 C, $00 C, $00 C, $80 C, $18 C, $71 C, $20 C, $00 C, $00 C, $00 C, $38 C, $00 C, $18 C,
CREATE BAD  21 ALLOT

\ ---- each AC- word gives the frame of the oracle
T{ AC-RESET  F0 FRAME= -> TRUE }T
T{ AC-RESET  TRUE AC-POWER!  F1 FRAME= -> TRUE }T
T{ AC-RESET  TRUE AC-POWER!  HEAT AC-MODE!  F2 FRAME= -> TRUE }T
T{ AC-RESET  TRUE AC-POWER!  HEAT AC-MODE!  23 AC-TEMP!  F3 FRAME= -> TRUE }T
T{ AC-RESET  16 AC-TEMP!  F4 FRAME= -> TRUE }T
T{ AC-RESET  30 AC-TEMP!  F5 FRAME= -> TRUE }T
T{ AC-RESET  21 AC-TEMP!  DRY AC-MODE!  F6 FRAME= -> TRUE }T      \ DRY and FAN carry 23 C
T{ AC-RESET  21 AC-TEMP!  FAN AC-MODE!  F7 FRAME= -> TRUE }T
T{ AC-RESET  DRY AC-MODE!  21 AC-TEMP!  F8 FRAME= -> TRUE }T
T{ AC-RESET  21 AC-TEMP!  DRY AC-MODE!  AUTO AC-MODE!  F9 FRAME= -> TRUE }T   \ 21 C again
T{ AC-RESET  0 AC-FAN!  F10 FRAME= -> TRUE }T
T{ AC-RESET  1 AC-FAN!  F11 FRAME= -> TRUE }T
T{ AC-RESET  2 AC-FAN!  F12 FRAME= -> TRUE }T
T{ AC-RESET  3 AC-FAN!  F13 FRAME= -> TRUE }T
T{ AC-RESET  4 AC-FAN!  F14 FRAME= -> TRUE }T
T{ AC-RESET  5 AC-FAN!  F15 FRAME= -> TRUE }T
T{ AC-RESET  4 AC-FAN!  16 1 FAN2-AT  F16 FRAME=  17 6 FAN2-AT  F14 FRAME= -> TRUE TRUE }T
T{ AC-RESET  TRUE AC-SWING!  F17 FRAME= -> TRUE }T
T{ AC-RESET  TRUE AC-SLEEP!  F18 FRAME= -> TRUE }T
T{ AC-RESET  TRUE AC-SUPER!  F19 FRAME= -> TRUE }T
T{ AC-RESET  FALSE AC-DISPLAY!  F20 FRAME= -> TRUE }T
T{ AC-RESET  TRUE AC-POWER!  3 AC-FAN!  TRUE AC-SWING!  13 23 AC-CLOCK!  22 AC-TEMP!  F21 FRAME= -> TRUE }T
T{ AC-RESET  TRUE AC-POWER!  HEAT AC-MODE!  TRUE AC-POWER!  FALSE AC-POWER!  F22 FRAME= -> TRUE }T

\ ---- frames of a remote control
T{ D1 AC-OK?  D2 AC-OK? -> TRUE TRUE }T
T{ AC-RESET  D1 AC-TAKE  D1 FRAME= -> TRUE }T
T{ POWER-BIT FIELD@  MODE-BITS FIELD@  TEMP-BITS FIELD@  FAN@  SWING1 FIELD@  KEY-CODE FIELD@ -> 1 2 6 3 1 2 }T
T{ NOW  SET-TEMP @ -> 803 22 }T                      \ its clock 13:23
T{ AC-RESET  D2 AC-TAKE  D2 FRAME= -> TRUE }T        \ bits outside the map stay
T{ MODE-BITS FIELD@  SET-TEMP @  FAN@  KEY-CODE FIELD@ -> 0 23 1 0 }T
T{ D2 11 UNKNOWN@  D2 12 UNKNOWN@  D2 14 UNKNOWN@  D2 3 UNKNOWN@  D1 17 UNKNOWN@ -> $80 $18 $20 0 0 }T
T{ D1 BAD 21 MOVE  BAD 13 + DUP C@ 1 XOR SWAP C!  BAD AC-OK? -> FALSE }T
T{ D1 BAD 21 MOVE  BAD 20 + DUP C@ 1 XOR SWAP C!  BAD AC-OK? -> FALSE }T
T{ D1 BAD 21 MOVE  $84 BAD C!  BAD AC-OK? -> FALSE }T
T{ AC-RESET  F27 AC-TAKE  GRADE  DRY-EXTRA FIELD@ -> -1 2 }T   \ DRY, grade -1
T{ COOL AC-MODE!  GRADE  DRY-EXTRA FIELD@  SET-TEMP @  TEMP-BITS FIELD@ -> 0 0 24 8 }T

\ ---- the state read back
T{ AC-RESET  TRUE AC-POWER!  COOL AC-MODE!  22 AC-TEMP!  4 AC-FAN!  AC-POWER@ AC-MODE@ AC-TEMP@ AC-FAN@ -> TRUE 2 22 4 }T

\ ---- times: out and back
T{ AC-RESET  TRUE AC-POWER!  3 AC-FAN!  AC-TIMINGS  NIP -> 343 }T
T{ AC-TIMINGS AC-DECODE  SWAP 21 AC-FRAME 21 STR= -> 168 TRUE }T
T{ AC-FRAME 21 AC>TIMES  NIP -> 343 }T                              \ any bytes, as AC-TIMINGS

\ ---- errors
T{ 31 ' AC-TEMP! CATCH NIP -> -2 }T
T{ 15 ' AC-TEMP! CATCH NIP -> -2 }T
T{ 6 ' AC-FAN! CATCH NIP -> -2 }T
T{ 5 ' AC-MODE! CATCH NIP -> -2 }T
T{ 24 0 ' AC-CLOCK! CATCH NIP NIP -> -2 }T
T{ AC-RESET  6 30 ' AC-ON-TIMER! CATCH NIP NIP -> -2 }T   \ no clock
T{ 21 0 ' FAN2-AT CATCH NIP NIP -> -2 }T

\ ---- the clock and the timers (32-bit cells)
: MINUTES ( n -- ms )   60000 * ;   \ no numbers above 65535: the file loads with 16-bit cells too
: MINUTE-AGO ( -- )   CLOCK-MS @ 1 MINUTES - CLOCK-MS ! ;
: CLOCK-TESTS ( -- )
  T{ AC-RESET  13 23 AC-CLOCK!  22 0 AC-OFF-TIMER!  F23 FRAME= -> TRUE }T
  T{ AC-RESET  13 23 AC-CLOCK!  6 30 AC-ON-TIMER!  F24 FRAME= -> TRUE }T
  T{ AC-RESET  13 23 AC-CLOCK!  6 30 AC-ON-TIMER!  22 0 AC-OFF-TIMER!  F25 FRAME= -> TRUE }T
  T{ AC-RESET  13 23 AC-CLOCK!  6 30 AC-ON-TIMER!  AC-NO-TIMERS  F26 FRAME= -> TRUE }T
  T{ 13 23 AC-CLOCK!  NOW  MINUTE-AGO NOW -> 803 804 }T
  T{ 23 59 AC-CLOCK!  MINUTE-AGO NOW -> 0 }T
  T{ 13 23 AC-CLOCK!  13 25 DUE MILLIS -  2 MINUTES 10 -  2 MINUTES 1+ WITHIN -> TRUE }T
  T{ 13 23 AC-CLOCK!  13 22 DUE MILLIS -  1439 MINUTES 10 -  1439 MINUTES 1+ WITHIN -> TRUE }T
  T{ AC-RESET  13 23 AC-CLOCK!  13 25 AC-OFF-TIMER!  AC-FRAME DROP  OFF-SET FIELD@ -> 1 }T
  T{ MILLIS OFF-DUE !  AC-FRAME DROP  OFF-SET FIELD@  OFF-H FIELD@  OFF-M FIELD@ -> 0 0 0 }T   \ its time came
  T{ AC-RESET  D1 AC-TAKE  CLOCK-SET @ -> TRUE }T ;
: CLOCK-TESTS? ( -- )   CELL 4 = IF CLOCK-TESTS THEN ;
CLOCK-TESTS?

S" kelon168" .RESULTS
