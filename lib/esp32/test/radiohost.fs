\ radiohost.fs - the test of ESP32-RADIO on the host: the list of the reception
\
\ lib/esp32/esp32/radio.fs runs on the board, with the registers of the
\ MAC, SRAM1, ESP32-PHY and INTR. Here three small packages stand for
\ them, so that the words of the list run on the host:
\
\ - MOCK-MAC: @ ! C@ C! MOVE FILL BIS! BIC! take the addresses of the board. The 8 KB
\   of the MAC and 16 KB of SRAM1 from $3FFE4000 are buffers here; the
\   other registers of the chip are one cell. The registers do what they
\   did on the board (orga/002_Plan.md, section 8.3, N3b-2 and N3b-3): the
\   reset of the MAC sets RX-NEXT to RX-END; a reload is done at once, and
\   it starts the DMA at RX-BASE only when RX-NEXT is RX-END; the bits
\   written to INT-CLEAR go from INT-STATUS.
\ - ESP32-PHY and INTR: the words that radio.fs uses; they only note the
\   channel and the handler of source 0.
\
\ ARRIVE ( a n -- ) is the DMA when a frame comes, as on the board: the n
\ bytes go into the descriptors from RX-NEXT on, each one as full as its
\ size lets it be. The last one gets the length of its part and the end
\ of the frame; the ones before it stay as they were. RX-NEXT is then the
\ next descriptor, or RX-END after the last. When the descriptors do not
\ take the whole frame, it is lost (LOST). FRAME makes the bytes of the
\ MAC: its header of 28 bytes (the level, the length with the FCS), then
\ the frame.
\ Each system with files and 32-bit cells runs it; in the folder ucf:
\   build/ucf-c -32 host/files.fs lib/esp32/test/radiohost.fs </dev/null
\ At the end: "radiohost: 34 tests, 0 errors".

ONLY FORTH DEFINITIONS  ALSO FILES
[DEFINED] T{ S" forth/test/tester.fs" ?INCLUDED
ONLY FORTH DEFINITIONS  ALSO FILES

PACKAGE MOCK-MAC
$3FF73000 CONSTANT MAC   8192 CONSTANT MAC-SIZE
$3FFE4000 CONSTANT SRAM1   16384 CONSTANT SRAM1-SIZE
MAC-SIZE BUFFER: MAC-MEM     MAC-MEM MAC-SIZE 0 FILL
SRAM1-SIZE BUFFER: SRAM1-MEM   SRAM1-MEM SRAM1-SIZE 0 FILL
VARIABLE ELSEWHERE           \ the other registers of the chip
: IN? ( a start size -- f )   OVER + WITHIN ;
PUBLIC
VARIABLE RNG-SAMPLE   $89ABCDEF RNG-SAMPLE !
VARIABLE PHY-RATE-REG
VARIABLE PHY-RATE2-REG
VARIABLE PHY-WDT-REG
VARIABLE MAC-READY-STUCK
: >HOST ( a -- a' )   \ the place in this system for an address of the board
  DUP $60035144 = IF  DROP RNG-SAMPLE  EXIT THEN
  DUP $6001C860 = OVER $3FF5C860 = OR IF  DROP PHY-RATE-REG  EXIT THEN
  DUP $6001C87C = OVER $3FF5C87C = OR IF  DROP PHY-RATE2-REG  EXIT THEN
  DUP $3FF5D040 = IF  DROP PHY-WDT-REG  EXIT THEN
  DUP MAC MAC-SIZE IN? IF  MAC - MAC-MEM +  EXIT THEN
  DUP SRAM1 SRAM1-SIZE IN? IF  SRAM1 - SRAM1-MEM +  EXIT THEN
  DUP $3FF00000 $100000 IN? IF  DROP ELSEWHERE  THEN ;
VARIABLE LOST                \ the frames that found no descriptor
VARIABLE #WRITES             \ writes to registers or SRAM1, not the bytes of a test
VARIABLE #TX-STARTS          \ writes with TX start bits to any of the five slots
VARIABLE #ACK-STARTS
VARIABLE MAC-NOW
VARIABLE MAC-HOOK            \ executes when MILLIS advances; the TX test supplies events
VARIABLE PMD-READS
VARIABLE TX-CLEAR-READS      \ PMD reads before the most recent nonzero slot status was cleared
: MILLIS ( -- u )
  1 MAC-NOW +!  MAC-HOOK @ ?DUP IF EXECUTE THEN  MAC-NOW @ ;
PRIVATE
: NOTE-WRITE ( a -- )
  DUP MAC MAC-SIZE IN?  SWAP SRAM1 SRAM1-SIZE IN? OR IF  1 #WRITES +!  THEN ;
: NOTE-TX ( x a -- )
  DUP MAC $D00 + 36 IN? IF
    MAC $D00 + -  7 AND 0=  SWAP $C0000000 AND 0<> AND IF  1 #TX-STARTS +!  THEN
  ELSE 2DROP THEN ;
: NOTE-ACK ( x a -- )
  OVER $C0000000 AND IF
    DUP MAC $D10 + = IF
      >HOST @ OR $1000000 AND IF  1 #ACK-STARTS +!  THEN
    ELSE 2DROP THEN
  ELSE 2DROP THEN ;
MAC $84 + CONSTANT CTRL-AT   MAC $C4C + CONSTANT CLEAR-AT
MAC $84 + >HOST CONSTANT CTRL-REG
MAC $88 + >HOST CONSTANT BASE-REG
MAC $8C + >HOST CONSTANT NEXT-REG
MAC $C48 + >HOST CONSTANT STATUS-REG
MAC $CBC + CONSTANT ERROR-CLEAR-AT
MAC $CC4 + CONSTANT COMPLETE-CLEAR-AT
MAC $CC0 + >HOST CONSTANT ERROR-REG
MAC $CC8 + >HOST CONSTANT COMPLETE-REG
$3FF00000 CONSTANT END-MARK
$80000000 CONSTANT OWNER
: MAC-RESET ( -- )   MAC-MEM MAC-SIZE 0 FILL  END-MARK NEXT-REG ! ;
: DMA-RELOAD ( -- )   NEXT-REG @ END-MARK = IF  BASE-REG @ NEXT-REG !  THEN ;
VARIABLE FROM   VARIABLE LEFT  \ the bytes of the frame that the DMA still writes
: ROOM? ( n -- f )   \ the descriptors from RX-NEXT on take n bytes
  NEXT-REG @  BEGIN                                  ( n x )
    DUP END-MARK = IF  2DROP FALSE EXIT  THEN
    >HOST  DUP @ OWNER AND 0= IF  2DROP FALSE EXIT  THEN
    TUCK @ $FFF AND -  DUP 1 < IF  2DROP TRUE EXIT  THEN
    SWAP 2 CELLS + @  ?DUP 0= IF END-MARK THEN
  AGAIN ;
: PART ( d -- )   \ the DMA writes the next bytes into descriptor d (in this system)
  DUP @ $FFF AND  LEFT @ MIN  >R                     \ R: the bytes that go into it
  FROM @  OVER CELL+ @ >HOST  R@ MOVE  R@ FROM +!  R@ NEGATE LEFT +!
  LEFT @ 0= IF  R@ 12 LSHIFT  OVER @ $FFF AND OR  $C0000000 OR  OVER !  THEN  R> DROP
  2 CELLS + @  ?DUP 0= IF END-MARK THEN  NEXT-REG ! ;   \ the end of the frame; owner bit stays
PUBLIC
: ARRIVE ( a n -- )   \ a frame comes: the DMA writes it into the descriptors from RX-NEXT on
  CTRL-REG @ OWNER AND 0=  OVER ROOM? 0=  OR IF  2DROP  1 LOST +!  EXIT THEN
  LEFT !  FROM !
  BEGIN  NEXT-REG @ >HOST PART  LEFT @ 0=  UNTIL
  MAC $2CC + >HOST DUP @ 1+ SWAP !
  STATUS-REG @ $1000020 OR  STATUS-REG ! ;           \ the interrupt of the reception
: BIS! ( mask a -- )
  DUP NOTE-WRITE  2DUP NOTE-TX  2DUP NOTE-ACK
  DUP $3FF000D0 = IF  OVER 4 AND IF MAC-RESET THEN  THEN                 \ WIFI-RST-EN: the MAC
  DUP CTRL-AT = IF  OVER 1 AND IF DMA-RELOAD THEN  SWAP -2 AND SWAP  THEN   \ RX-CTRL: reload at once
  DUP $3FF73D24 = IF
    OVER 2 AND MAC-READY-STUCK @ 0= AND IF  SWAP 1 OR SWAP  THEN
  THEN
  >HOST TUCK @ OR SWAP ! ;
: BIC! ( mask a -- )   DUP NOTE-WRITE  >HOST TUCK @ SWAP INVERT AND SWAP ! ;
: ! ( x a -- )
  DUP NOTE-WRITE  2DUP NOTE-TX
  DUP CLEAR-AT = IF  DROP INVERT STATUS-REG @ AND STATUS-REG !  EXIT THEN   \ INT-CLEAR
  DUP ERROR-CLEAR-AT = IF
    DROP DUP ERROR-REG @ AND $40004 AND IF  PMD-READS @ TX-CLEAR-READS !  THEN
    INVERT ERROR-REG @ AND ERROR-REG !  EXIT
  THEN
  DUP COMPLETE-CLEAR-AT = IF
    DROP DUP COMPLETE-REG @ AND 4 AND IF  PMD-READS @ TX-CLEAR-READS !  THEN
    INVERT COMPLETE-REG @ AND COMPLETE-REG !  EXIT
  THEN
  >HOST ! ;
: @ ( a -- x )
  DUP MAC $11F8 + = IF  1 PMD-READS +!  THEN  >HOST @ ;
: C@ ( a -- c )   >HOST C@ ;
: C! ( c a -- )   DUP NOTE-WRITE  >HOST C! ;
: MOVE ( a b n -- )   >R DUP NOTE-WRITE  >HOST SWAP >HOST SWAP R> MOVE ;
: FILL ( a n c -- )   >R OVER NOTE-WRITE  SWAP >HOST SWAP R> FILL ;
END-PACKAGE

PACKAGE ESP32-PHY
PUBLIC
VARIABLE PHY-CH              \ the channel of the radio
VARIABLE #TRACKS
VARIABLE #LOW-OFF
: PHY-INIT ( -- n )   0 ;
: HAL-INIT ( -- ) ;
: PHY-CHANNEL! ( ch -- )   PHY-CH ! ;
: PHY-OFF ( -- )   0 PHY-CH ! ;
: PHY-TRACK ( -- )   1 #TRACKS +! ;
: PHY-LOW-OFF ( -- )   1 #LOW-OFF +! ;
END-PACKAGE

PACKAGE INTR
PUBLIC
VARIABLE INT0-XT             \ the handler of source 0 (the MAC)
VARIABLE INT0-ON             \ source 0 goes to its handler
: INT! ( xt i -- )   0= IF INT0-XT ! ELSE DROP THEN ;
: INT-ON ( i -- )   0= IF TRUE INT0-ON ! THEN ;
: INT-OFF ( i -- )   0= IF FALSE INT0-ON ! THEN ;
: +INT ( -- ) ;
END-PACKAGE

INCLUDE lib/net/bytes.fs
INCLUDE lib/esp32/esp32/macinit.fs
INCLUDE lib/esp32/esp32/radio.fs
ALSO ESP32-RADIO.I

2048 BUFFER: SCRATCH
VARIABLE RAW   40 RAW !      \ byte 0 of the header: the level 40 - 96 = -56 dBm
: FRAME ( k n -- a n' )   \ the header of the MAC, then n bytes k, k+1, ...; n' with the padding
  SCRATCH RX-HEAD 0 FILL  RAW @ SCRATCH C!
  DUP 4 +  DUP 255 AND SCRATCH 24 + C!  8 RSHIFT SCRATCH 25 + C!   \ the length with the FCS
  DUP 0 ?DO  OVER I + SCRATCH RX-HEAD + I + C!  LOOP  NIP
  3 + -4 AND RX-HEAD +  SCRATCH SWAP ;
: FRAME? ( a n k -- f )   \ the n bytes at a (an address of the board) are those of FRAME
  >R >R >HOST R> R>  SWAP 0 ?DO
    OVER I + C@  OVER I + 255 AND <> IF  2DROP FALSE UNLOOP EXIT  THEN
  LOOP  2DROP TRUE ;
\ The first byte of a frame counts the frames that came: fewer than 256 here.
: RECEIVED ( -- a n | 0 0 )   RADIO-RECEIVE ?DUP 0= IF 0 0 THEN ;
VARIABLE SENT   VARIABLE GOT   VARIABLE BAD
: COME ( u -- )   \ u frames of 100 bytes come
  0 ?DO  SENT @ 100 FRAME ARRIVE  1 SENT +!  LOOP ;
: ONE ( -- f )   \ RADIO-RECEIVE once: TRUE if a frame came; BAD counts a wrong one
  RECEIVED DUP 0= IF  2DROP FALSE EXIT  THEN              ( a n )
  DUP 100 <> IF  1 BAD +!  THEN
  OVER >HOST C@  DUP GOT @ < IF  1 BAD +!  THEN           \ in order; a lost frame leaves a gap
  DUP 1+ GOT !  FRAME? 0= IF  1 BAD +!  THEN  TRUE ;
: TAKE-ALL ( -- u )   0 BEGIN ONE WHILE 1+ REPEAT ;   \ RADIO-RECEIVE until it gives 0: u frames
: ROUNDS ( u -- n )   \ u times: one frame comes, RADIO-RECEIVE takes it
  0 SWAP 0 ?DO  1 COME  TAKE-ALL +  LOOP ;

\ While the radio is off: no channel, no reception.
T{ RADIO-CHANNEL@ -> 0 }T
T{ ' RADIO-RECEIVE CATCH -> -2 }T

\ The start: one list of 8 descriptors; the DMA begins with the first.
T{ RADIO-START  ON? RADIO-CHANNEL@ PHY-CH @ -> TRUE 1 1 }T
T{ RX-CTRL @ RX-ON AND -> RX-ON }T
T{ RX-BASE @ RX-NEXT @ -> 0 DESC 0 DESC }T
T{ 0 DESC @  0 DESC CELL+ @  0 DESC >NEXT @ -> $80000640 0 BUF 1 DESC }T
T{ 7 DESC >NEXT @  HEAD TAIL HELD -> 0 0 7 -1 }T
T{ INT0-XT @ INT0-ON @ -> ' MAC-INT TRUE }T
T{ RECEIVED -> 0 0 }T

\ A frame comes. RADIO-RECEIVE gives its bytes after the header of the
\ MAC; the next one gives the descriptor back, at the end of the list.
T{ 0 60 FRAME ARRIVE  RECEIVED -> 0 BUF RX-HEAD + 60 }T
T{ 0 BUF RX-HEAD + 60 0 FRAME?  HEAD HELD  RADIO-LEVEL -> TRUE 1 0 -56 }T
T{ RECEIVED  HELD TAIL  7 DESC >NEXT @  0 DESC @ -> 0 0 -1 0 0 DESC $80000640 }T

\ Frames that come before RADIO-RECEIVE wait, and come out in order.
T{ 7 COME  TAKE-ALL  BAD @  LOST @ -> 7 0 0 }T
T{ 3 COME  TAKE-ALL  5 COME  TAKE-ALL  BAD @ -> 3 5 0 }T

\ More frames than descriptors: 8 wait, 2 are lost, and the DMA stands at
\ the end of the list. Each RADIO-RECEIVE gives a descriptor back, and
\ the DMA fills it again, also before the other frames are taken. (First
\ one frame more: so the list does not begin with descriptor 0.)
T{ 1 COME  TAKE-ALL  HEAD -> 1 1 }T
T{ 10 COME  LOST @  RX-NEXT @ RX-END = -> 2 TRUE }T
T{ ONE ONE  RX-NEXT @ RX-END =  1 COME  LOST @ -> TRUE TRUE FALSE 2 }T
T{ TAKE-ALL  BAD @  LOST @ -> 7 0 2 }T

\ Many frames, one by one: the list goes round many times.
T{ 100 ROUNDS  BAD @  LOST @ -> 100 0 2 }T

\ The length of a frame comes from the header (the buffer has up to 3
\ bytes more), the level too: byte 0 with its sign, minus 96.
T{ 0 61 FRAME NIP  0 61 FRAME ARRIVE  RECEIVED NIP  RADIO-LEVEL -> 92 61 -56 }T
T{ $F0 RAW !  0 8 FRAME ARRIVE  RECEIVED NIP  RADIO-LEVEL  40 RAW ! -> 8 -112 }T

\ A frame longer than a buffer fills two descriptors: RADIO-RECEIVE gives
\ them back and counts it. Then all 8 descriptors take frames again.
T{ 0 1700 FRAME ARRIVE  RECEIVED  #LONG @ -> 0 0 1 }T
T{ 8 COME  TAKE-ALL  BAD @  LOST @ -> 8 0 2 }T

\ The filter of a scan. HAL-INIT leaves bits in both registers (as on the
\ board); RADIO-BEACONS sets its 4 bits and clears the control frames.
\ Then only the beacons come out; another frame goes back, unread.
T{ $7045 FILTER !  $05000000 CTRL-FILTER !  RADIO-BEACONS -> }T
T{ FILTER @  CTRL-FILTER @  BEACONS? -> $7057 0 TRUE }T
T{ $80 40 FRAME ARRIVE  $50 40 FRAME ARRIVE  $80 40 FRAME ARRIVE -> }T
T{ RECEIVED NIP  RECEIVED  RECEIVED NIP  RECEIVED  #OTHER @ -> 40 0 0 40 0 0 1 }T

\ The handler: it notes the bits that came, clears them, and counts.
T{ 0 #INTS !  0 INT-BITS !  INT-STATUS @ 0<> -> TRUE }T
T{ MAC-INT  INT-STATUS @  INT-BITS @  #INTS @ -> 0 $1000020 1 }T

\ Off: no reception, no handler; a frame is lost; RADIO-RECEIVE is an error.
T{ RADIO-STOP  RX-CTRL @ RX-ON AND  INT0-ON @  HELD  PHY-CH @  BEACONS? -> 0 FALSE -1 0 FALSE }T
T{ 1 COME  LOST @ -> 3 }T
T{ ' RADIO-RECEIVE CATCH -> -2 }T

\ On again: a new list; every frame comes out.
T{ RADIO-START  RX-NEXT @  HEAD TAIL  BEACONS? -> 0 DESC 0 7 FALSE }T
T{ 3 COME  TAKE-ALL  BAD @ -> 3 0 }T
RADIO-STOP

S" radiohost" .RESULTS
