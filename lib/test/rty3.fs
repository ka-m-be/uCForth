\ rty3.fs - tests of the package RTY3 (lib/irremote/rty3.fs)
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   ... host/files.fs forth/test/tester.fs lib/test/rty3.fs
\ A1 to A16 and B1 to B21 are frames of the remote control RCH-RTY3-0,
\ RAW the times of one frame: Kai's measurements on 2026-09-29 with the
\ transceiver (LISTEN and .TIMES; archive/plan/002_Plan_Fassung73.md, 23.12 and 23.13).
\ B19 is a short frame of another type ($78). The running clock needs
\ 32-bit cells.

ONLY FORTH DEFINITIONS  ALSO FILES
S" lib/irremote/ircode.fs" INCLUDED
S" lib/irremote/rty3.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO RTY3  ALSO RTY3.I
DECIMAL

CREATE A1  $60 C, $38 C, $13 C, $05 C, $FF C, $00 C, $50 C, $20 C, $DC C, $00 C, $15 C, $03 C, $00 C, $00 C, $00 C, $00 C, $03 C, $03 C, $00 C, $06 C, $00 C, $00 C, $46 C,
CREATE A2  $60 C, $38 C, $13 C, $05 C, $DA C, $00 C, $50 C, $20 C, $DC C, $00 C, $15 C, $03 C, $00 C, $00 C, $00 C, $00 C, $03 C, $03 C, $00 C, $11 C, $00 C, $00 C, $74 C,
CREATE A3  $60 C, $38 C, $13 C, $05 C, $DA C, $00 C, $00 C, $20 C, $DC C, $00 C, $15 C, $43 C, $00 C, $00 C, $00 C, $00 C, $03 C, $03 C, $00 C, $04 C, $00 C, $00 C, $71 C,
CREATE A4  $60 C, $38 C, $13 C, $05 C, $DA C, $00 C, $00 C, $20 C, $DC C, $00 C, $15 C, $43 C, $00 C, $00 C, $00 C, $00 C, $04 C, $03 C, $00 C, $07 C, $00 C, $00 C, $75 C,
CREATE A5  $60 C, $38 C, $13 C, $05 C, $DA C, $00 C, $00 C, $20 C, $DC C, $00 C, $15 C, $43 C, $00 C, $00 C, $00 C, $00 C, $04 C, $04 C, $00 C, $08 C, $00 C, $00 C, $7D C,
CREATE A6  $60 C, $38 C, $13 C, $05 C, $DA C, $00 C, $00 C, $20 C, $DC C, $00 C, $15 C, $43 C, $00 C, $00 C, $00 C, $00 C, $04 C, $04 C, $00 C, $02 C, $00 C, $00 C, $77 C,
CREATE A7  $60 C, $38 C, $13 C, $05 C, $DA C, $00 C, $0A C, $20 C, $DC C, $00 C, $15 C, $43 C, $00 C, $00 C, $00 C, $00 C, $04 C, $04 C, $00 C, $01 C, $00 C, $00 C, $7E C,
CREATE A8  $60 C, $38 C, $13 C, $05 C, $DA C, $00 C, $50 C, $20 C, $DC C, $00 C, $15 C, $04 C, $00 C, $00 C, $00 C, $00 C, $04 C, $04 C, $00 C, $16 C, $00 C, $00 C, $74 C,
CREATE A9  $60 C, $38 C, $13 C, $05 C, $DA C, $00 C, $50 C, $20 C, $DC C, $00 C, $15 C, $04 C, $00 C, $40 C, $00 C, $40 C, $04 C, $04 C, $00 C, $2B C, $00 C, $00 C, $49 C,
CREATE A10  $60 C, $38 C, $13 C, $05 C, $DA C, $00 C, $50 C, $00 C, $00 C, $00 C, $15 C, $04 C, $00 C, $40 C, $00 C, $40 C, $04 C, $04 C, $00 C, $0D C, $00 C, $00 C, $93 C,
CREATE A11  $60 C, $38 C, $13 C, $05 C, $DA C, $00 C, $50 C, $20 C, $E6 C, $00 C, $15 C, $04 C, $00 C, $40 C, $00 C, $40 C, $04 C, $04 C, $00 C, $0D C, $00 C, $00 C, $55 C,
CREATE A12  $60 C, $38 C, $13 C, $05 C, $DA C, $00 C, $50 C, $20 C, $E6 C, $00 C, $15 C, $04 C, $00 C, $40 C, $25 C, $6F C, $04 C, $04 C, $00 C, $05 C, $00 C, $00 C, $57 C,
CREATE A13  $60 C, $38 C, $13 C, $05 C, $DA C, $00 C, $50 C, $20 C, $E6 C, $00 C, $15 C, $84 C, $00 C, $40 C, $25 C, $6F C, $04 C, $04 C, $00 C, $24 C, $00 C, $00 C, $F6 C,
CREATE A14  $60 C, $38 C, $13 C, $05 C, $DA C, $00 C, $50 C, $20 C, $E6 C, $00 C, $15 C, $84 C, $00 C, $80 C, $25 C, $6F C, $04 C, $04 C, $00 C, $2C C, $00 C, $00 C, $3E C,
CREATE A15  $60 C, $38 C, $13 C, $05 C, $DA C, $00 C, $50 C, $20 C, $E6 C, $00 C, $15 C, $84 C, $00 C, $00 C, $25 C, $2F C, $04 C, $04 C, $00 C, $2C C, $00 C, $00 C, $FE C,
CREATE A16  $60 C, $38 C, $13 C, $05 C, $DA C, $00 C, $50 C, $20 C, $E6 C, $00 C, $15 C, $04 C, $00 C, $00 C, $25 C, $2F C, $04 C, $04 C, $00 C, $24 C, $00 C, $00 C, $76 C,
CREATE B1  $60 C, $38 C, $13 C, $06 C, $FF C, $00 C, $46 C, $00 C, $00 C, $00 C, $15 C, $1A C, $00 C, $00 C, $25 C, $2F C, $06 C, $04 C, $00 C, $25 C, $00 C, $00 C, $9D C,
CREATE B2  $60 C, $38 C, $13 C, $07 C, $FF C, $00 C, $46 C, $00 C, $00 C, $00 C, $15 C, $1A C, $00 C, $00 C, $25 C, $2F C, $06 C, $04 C, $00 C, $25 C, $00 C, $00 C, $9C C,
CREATE B3  $60 C, $38 C, $13 C, $09 C, $DA C, $00 C, $46 C, $00 C, $00 C, $00 C, $15 C, $1A C, $00 C, $00 C, $25 C, $2F C, $00 C, $04 C, $00 C, $06 C, $00 C, $00 C, $92 C,
CREATE B4  $60 C, $38 C, $13 C, $01 C, $FF C, $00 C, $32 C, $00 C, $00 C, $00 C, $15 C, $1A C, $00 C, $00 C, $25 C, $2F C, $05 C, $04 C, $00 C, $06 C, $00 C, $00 C, $CE C,
CREATE B5  $60 C, $38 C, $13 C, $05 C, $FF C, $00 C, $50 C, $00 C, $00 C, $00 C, $15 C, $1A C, $00 C, $00 C, $25 C, $2F C, $04 C, $04 C, $00 C, $06 C, $00 C, $00 C, $A9 C,
CREATE B6  $60 C, $38 C, $13 C, $07 C, $FF C, $00 C, $46 C, $00 C, $00 C, $00 C, $15 C, $1A C, $00 C, $00 C, $25 C, $2F C, $06 C, $04 C, $00 C, $06 C, $00 C, $00 C, $BF C,
CREATE B7  $60 C, $38 C, $13 C, $07 C, $FF C, $00 C, $46 C, $01 C, $00 C, $00 C, $15 C, $1A C, $00 C, $00 C, $25 C, $2F C, $06 C, $04 C, $00 C, $01 C, $00 C, $00 C, $B9 C,
CREATE B8  $60 C, $38 C, $13 C, $07 C, $FF C, $00 C, $46 C, $02 C, $00 C, $00 C, $15 C, $1A C, $00 C, $00 C, $25 C, $2F C, $06 C, $04 C, $00 C, $01 C, $00 C, $00 C, $BA C,
CREATE B9  $60 C, $38 C, $13 C, $07 C, $FF C, $00 C, $46 C, $01 C, $00 C, $00 C, $15 C, $1A C, $00 C, $00 C, $25 C, $2F C, $06 C, $04 C, $00 C, $02 C, $00 C, $00 C, $BA C,
CREATE B10  $60 C, $38 C, $13 C, $07 C, $FF C, $00 C, $46 C, $00 C, $00 C, $00 C, $15 C, $1A C, $00 C, $00 C, $25 C, $2F C, $06 C, $04 C, $00 C, $02 C, $00 C, $00 C, $BB C,
CREATE B11  $60 C, $38 C, $13 C, $07 C, $FF C, $00 C, $46 C, $00 C, $00 C, $00 C, $15 C, $1B C, $00 C, $00 C, $25 C, $2F C, $07 C, $04 C, $00 C, $07 C, $00 C, $00 C, $BE C,
CREATE B12  $60 C, $38 C, $13 C, $07 C, $FF C, $00 C, $46 C, $00 C, $00 C, $00 C, $15 C, $1B C, $00 C, $00 C, $25 C, $2F C, $00 C, $04 C, $00 C, $07 C, $00 C, $00 C, $B9 C,
CREATE B13  $60 C, $38 C, $13 C, $07 C, $FF C, $00 C, $46 C, $00 C, $00 C, $00 C, $15 C, $1B C, $00 C, $00 C, $25 C, $2F C, $00 C, $00 C, $00 C, $08 C, $00 C, $00 C, $B2 C,
CREATE B14  $60 C, $38 C, $13 C, $07 C, $FF C, $00 C, $46 C, $00 C, $00 C, $00 C, $15 C, $1B C, $00 C, $00 C, $25 C, $2F C, $00 C, $01 C, $00 C, $08 C, $00 C, $00 C, $B3 C,
CREATE B15  $60 C, $38 C, $13 C, $05 C, $DA C, $00 C, $00 C, $00 C, $00 C, $00 C, $15 C, $5B C, $00 C, $00 C, $25 C, $2F C, $04 C, $01 C, $00 C, $04 C, $00 C, $00 C, $9A C,
CREATE B16  $60 C, $38 C, $13 C, $07 C, $FF C, $00 C, $46 C, $00 C, $00 C, $00 C, $15 C, $1B C, $00 C, $00 C, $25 C, $2F C, $00 C, $01 C, $00 C, $16 C, $00 C, $00 C, $AD C,
CREATE B17  $60 C, $38 C, $13 C, $07 C, $FF C, $00 C, $46 C, $00 C, $00 C, $00 C, $15 C, $1B C, $00 C, $40 C, $25 C, $6F C, $00 C, $01 C, $00 C, $2B C, $00 C, $00 C, $90 C,
CREATE B18  $60 C, $38 C, $13 C, $07 C, $FF C, $00 C, $46 C, $00 C, $00 C, $00 C, $15 C, $1B C, $00 C, $40 C, $00 C, $40 C, $00 C, $01 C, $00 C, $05 C, $00 C, $00 C, $B4 C,
CREATE B19  $60 C, $78 C, $09 C, $03 C, $7F C, $00 C, $00 C, $00 C, $00 C, $00 C, $00 C, $0F C, $73 C,
CREATE B20  $60 C, $38 C, $13 C, $07 C, $FF C, $00 C, $46 C, $20 C, $E6 C, $00 C, $15 C, $1B C, $00 C, $40 C, $00 C, $40 C, $00 C, $01 C, $00 C, $0D C, $00 C, $00 C, $7A C,
CREATE B21  $60 C, $38 C, $13 C, $07 C, $FF C, $00 C, $46 C, $20 C, $E6 C, $00 C, $15 C, $9B C, $00 C, $40 C, $00 C, $40 C, $00 C, $01 C, $00 C, $24 C, $00 C, $00 C, $D3 C,
CREATE R1  $60 C, $38 C, $13 C, $07 C, $FF C, $00 C, $46 C, $20 C, $E6 C, $00 C, $15 C, $18 C, $00 C, $00 C, $25 C, $2F C, $06 C, $04 C, $00 C, $06 C, $00 C, $00 C, $7B C,
CREATE RAW
  8991 , 4507 , 530 , 564 , 530 , 566 , 531 , 570 , 530 , 574 , 530 , 577 ,
  530 , 1730 , 530 , 1735 , 530 , 574 , 530 , 564 , 529 , 567 , 530 , 570 ,
  530 , 1723 , 530 , 1728 , 529 , 1732 , 529 , 585 , 529 , 575 , 530 , 1713 ,
  530 , 1716 , 530 , 571 , 530 , 573 , 530 , 1728 , 529 , 581 , 530 , 584 ,
  530 , 575 , 529 , 1713 , 530 , 1717 , 530 , 1720 , 530 , 574 , 530 , 577 ,
  530 , 581 , 530 , 584 , 530 , 574 , 530 , 1714 , 529 , 1717 , 530 , 1720 ,
  529 , 1724 , 530 , 1727 , 530 , 1730 , 530 , 1734 , 530 , 1725 , 529 , 564 ,
  529 , 568 , 530 , 570 , 530 , 574 , 530 , 577 , 530 , 581 , 530 , 584 ,
  530 , 575 , 530 , 563 , 530 , 1716 , 530 , 1721 , 529 , 575 , 529 , 578 ,
  529 , 581 , 530 , 1735 , 529 , 575 , 530 , 563 , 530 , 567 , 530 , 571 ,
  529 , 575 , 529 , 577 , 530 , 1731 , 530 , 584 , 530 , 575 , 529 , 564 ,
  529 , 1717 , 530 , 1720 , 530 , 574 , 530 , 577 , 530 , 1730 , 530 , 1734 ,
  530 , 1725 , 530 , 563 , 530 , 567 , 530 , 570 , 530 , 574 , 530 , 578 ,
  529 , 582 , 529 , 585 , 530 , 574 , 530 , 1714 , 529 , 567 , 530 , 1721 ,
  529 , 574 , 529 , 1728 , 530 , 581 , 530 , 585 , 529 , 575 , 530 , 564 ,
  529 , 568 , 529 , 571 , 529 , 1724 , 529 , 1728 , 530 , 581 , 530 , 585 ,
  529 , 575 , 530 , 563 , 530 , 568 , 529 , 571 , 529 , 575 , 529 , 578 ,
  530 , 581 , 530 , 584 , 530 , 575 , 529 , 564 , 530 , 567 , 530 , 571 ,
  529 , 575 , 529 , 578 , 529 , 582 , 530 , 584 , 530 , 574 , 530 , 1714 ,
  530 , 567 , 529 , 1721 , 529 , 575 , 529 , 578 , 529 , 1732 , 529 , 585 ,
  529 , 575 , 529 , 1714 , 529 , 1718 , 529 , 1721 , 529 , 1725 , 528 , 578 ,
  529 , 1733 , 528 , 586 , 529 , 575 , 529 , 564 , 529 , 1718 , 529 , 1722 ,
  528 , 575 , 529 , 578 , 529 , 582 , 529 , 586 , 529 , 575 , 529 , 565 ,
  528 , 568 , 529 , 1722 , 529 , 575 , 528 , 579 , 529 , 582 , 529 , 585 ,
  529 , 576 , 529 , 565 , 528 , 569 , 528 , 572 , 529 , 575 , 529 , 579 ,
  528 , 583 , 528 , 586 , 528 , 577 , 528 , 565 , 528 , 1719 , 528 , 1723 ,
  527 , 576 , 528 , 580 , 527 , 584 , 527 , 587 , 528 , 577 , 527 , 566 ,
  528 , 569 , 528 , 573 , 527 , 577 , 527 , 580 , 527 , 584 , 527 , 588 ,
  527 , 577 , 527 , 566 , 528 , 570 , 527 , 573 , 527 , 601 , 503 , 581 ,
  526 , 584 , 527 , 612 , 503 , 577 , 527 , 1740 , 503 , 1744 , 503 , 597 ,
  503 , 1750 , 503 , 1731 , 526 , 1758 , 502 , 1738 , 526 , 587 , 816 ,
CREATE BAD  23 ALLOT

: SAME-BYTES? ( a1 a2 u -- f )
  0 ?DO  OVER I + C@  OVER I + C@ <> IF 2DROP UNLOOP FALSE EXIT THEN  LOOP  2DROP TRUE ;
: FRAME= ( a -- f )   AC-FRAME SWAP 23 SAME-BYTES? ;
: ALL-OK? ( a1 .. an n -- f )   TRUE SWAP 0 ?DO  SWAP AC-OK? AND  LOOP ;
: BYTE@ ( i -- c )   FRAME + C@ ;

\ ---- the frames of the remote control: $60, $38, 19, the data, their XOR
T{ A1 A2 A3 A4 A5 A6 A7 A8 A9 A10 A11 A12 A13 A14 A15 A16  16 ALL-OK? -> TRUE }T
T{ B1 B2 B3 B4 B5 B6 B7 B8 B9 B10 B11 B12 B13 B14 B15 B16 B17 B18 B20 B21  20 ALL-OK? -> TRUE }T
T{ B19 13 AC-CHECK?  B19 AC-OK?  B19 12 AC-CHECK? -> TRUE FALSE FALSE }T   \ the short frame
T{ A1 BAD 23 MOVE  BAD 22 + DUP C@ 1 XOR SWAP C!  BAD AC-OK? -> FALSE }T

\ ---- the times of a real frame back into bytes
T{ RAW 371 AC-DECODE  SWAP R1 23 SAME-BYTES? -> 184 TRUE }T

\ ---- a frame of the remote control becomes the state, and goes out as it came
T{ AC-RESET  B2 AC-TAKE  B2 FRAME= -> TRUE }T
T{ POWER-BIT FIELD@  MODE-BITS FIELD@  FLAP-CODE FIELD@  FAN-CODE FIELD@  KEY-CODE FIELD@ -> 1 3 6 4 $25 }T
T{ AC-RESET  A1 AC-TAKE  A1 FRAME=  SET-TEMP @ -> TRUE 24 }T

\ ---- the AC- words give the bytes of the remote control (B1 to B13)
T{ AC-RESET  B2 AC-TAKE  FALSE AC-POWER!  3 BYTE@  19 BYTE@ -> $06 $25 }T          \ as B1
T{ AC-RESET  B2 AC-TAKE  FAN AC-MODE!  3 BYTE@  6 BYTE@  19 BYTE@ -> $09 $46 $06 }T   \ as B3
T{ HEAT AC-MODE!  21 AC-TEMP!  3 BYTE@  6 BYTE@ -> $01 $32 }T                       \ as B4
T{ COOL AC-MODE!  24 AC-TEMP!  3 BYTE@  6 BYTE@  19 BYTE@ -> $05 $50 $01 }T         \ as B5
T{ AC-RESET  A1 AC-TAKE  17 AC-TEMP!  6 BYTE@  19 BYTE@  SET-TEMP @ -> $0A $02 17 }T
T{ AC-RESET  B2 AC-TAKE  7 AC-FLAP!  16 BYTE@  19 BYTE@ -> 7 $07 }T                 \ as B11
T{ 0 AC-FLAP!  16 BYTE@  0 AC-FAN!  17 BYTE@  19 BYTE@ -> 0 0 $08 }T               \ as B12, B13
T{ AC-RESET  B8 AC-TAKE  DRY-GRADE FIELD@  COOL AC-MODE!  DRY-GRADE FIELD@ -> 2 0 }T

\ ---- times: out and back
T{ AC-RESET  TRUE AC-POWER!  AC-TIMINGS  NIP -> 371 }T
T{ AC-TIMINGS  1- CELLS + @  TIMES @  TIMES CELL+ @ -> 800 9000 4500 }T   \ the last light, the header
T{ AC-TIMINGS AC-DECODE  SWAP AC-FRAME 23 SAME-BYTES? -> 184 TRUE }T

\ ---- errors
T{ 31 ' AC-TEMP! CATCH NIP -> -2 }T
T{ 8 ' AC-FAN! CATCH NIP -> -2 }T
T{ 8 ' AC-FLAP! CATCH NIP -> -2 }T
T{ 5 ' AC-MODE! CATCH NIP -> -2 }T
T{ 24 0 ' AC-CLOCK! CATCH NIP NIP -> -2 }T

\ ---- the clock (32-bit cells)
: MINUTES ( n -- ms )   60000 * ;
: CLOCK-TESTS ( -- )
  T{ AC-RESET  B2 AC-TAKE  NOW -> 21 60 * 26 + }T
  T{ CLOCK-MS @ 1 MINUTES - CLOCK-MS !  AC-FRAME DROP  10 BYTE@  11 BYTE@ -> 21 27 }T
  T{ 23 59 AC-CLOCK!  CLOCK-MS @ 1 MINUTES - CLOCK-MS !  NOW -> 0 }T ;
: CLOCK-TESTS? ( -- )   CELL 4 = IF CLOCK-TESTS THEN ;
CLOCK-TESTS?

S" rty3" .RESULTS
