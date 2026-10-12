\ ircap.fs - package IR-CAPTURE: the IR receiver of the WROOM into 4 raw slots, by a CODE handler
\
\ The RMT of the ESP32 cannot take a frame in parts (it has no RX wrap):
\ channel 4 takes 4 blocks of the RAM of the RMT, 256 pulse words, and stops
\ at the end of a frame (12 ms without light) or when its RAM is full. Then
\ the RMT asks for an interrupt (source 47). The handler CAPTURE is machine
\ code (a CODE word at interrupt level 2): it stops the reception, copies the
\ pulse words and the time of the end into a free raw slot, and starts the
\ reception again; that takes about 21 us at 240 MHz. It uses only registers
\ and the internal RAM, no Forth word and no ROM function, so it may run at
\ any time, also while the flash is busy. A frame longer than the RAM is not
\ cut: the handler drops it, and also the rest of it up to its end (the
\ tail), and counts it (long). With all 4 slots in use it drops the new
\ frame and counts it (full). IR-QUEUE (lib/esp32/irqueue.fs) decodes the
\ slots.
\
\   IR-CAP-ON ( -- )           the handler on; channel 4 receives
\   IR-CAP-OFF ( -- )          off; the slots are cleared
\   IR-CAP-FRAME? ( -- a n us true | false )   the oldest slot: n pulse words at
\                              a, the end in us (MICROS)
\   IR-CAP-RELEASE ( -- )      that slot is free again
\   IR-CAP-PENDING? ( -- f )   a slot waits
\   IR-CAP-LAST ( -- us )      the end of the last frame, also of a dropped one
\   IR-CAP-POS ( -- n )        where the RMT writes now: it moves with each pulse
\   .IR-CAP ( -- )             full, long, the most slots in use, the interrupts,
\                              the longest run of the handler in CPU cycles
\
\ IR (lib/esp32/ir.fs) sets the pins and channel 4 first (IR-PINS). The
\ assembler (asm/xtensa.fs) is needed only to make the handler: this file
\ loads it after the package, puts the code into SRAM0 and forgets the
\ assembler again (MARKER IRC-TOOLS), as phy.fs does. FREEZE keeps the code.
\
\ Load it in ucf term after RMT and IR (apps/irbridge/app.fs does it):
\   #require IR-CAPTURE lib/esp32/esp32/ircap.fs
\ The test on the board: lib/esp32/esp32/test/irqueue.fs.

PACKAGE IR-CAPTURE ALSO CHIP ALSO GPIO ALSO INTR ALSO RMT ALSO RMT.I
4 CONSTANT CAP-SLOTS                  \ the handler takes write mod 4: keep it 4
256 CONSTANT CAP-WORDS                \ the RAM of channel 4: 4 blocks of 64 pulse words
CAP-WORDS CELLS 8 + CONSTANT CAP-SIZE \ a slot: the count, the end in us, the pulse words
0 CONSTANT C-WRITE                    \ the fields of CAP-STATE: the slots written
4 CONSTANT C-READ                     \ the slots released
8 CONSTANT C-FULL                     \ frames dropped: all slots in use
12 CONSTANT C-LONG                    \ frames dropped: longer than the RAM
16 CONSTANT C-HIGH                    \ the most slots in use
20 CONSTANT C-END-US                  \ the end of the last frame
24 CONSTANT C-IRQS                    \ the interrupts
28 CONSTANT C-MAX                     \ the longest run of the handler in CPU cycles
32 CONSTANT C-TAIL                    \ not 0: drop the rest of a long frame
36 SRAM-BUFFER: CAP-STATE
CAP-SLOTS CAP-SIZE * SRAM-BUFFER: CAP-RAW
CAP-STATE 36 0 FILL CAP-RAW CAP-SLOTS CAP-SIZE * 0 FILL
4 CONF1 CONSTANT RX4-CONF1            \ channel 4: the reception and its RAM
4 RMT-RAM CONSTANT RX4-RAM
RMT-BASE $70 + CONSTANT RX4-STATUS    \ RMT_CH4STATUS_REG: bits 0 to 9, where the RMT writes
$6000 CONSTANT RX4-EVENTS             \ INT-RAW bit 13 (3 ch + 1): the end; bit 14 (3 ch + 2): RAM full
$22 CONSTANT RX-RUN                   \ CONF1 bit 1: receive; bit 5: the RAM belongs to the RMT
12 CONSTANT RX-RESET                  \ CONF1 bits 2 and 3: the write and read pointers back
VARIABLE ENABLED
VARIABLE CAP-BYTES                    \ the size of the machine code
: SLOT ( i -- a ) CAP-SLOTS 1- AND CAP-SIZE * CAP-RAW + ;
: CAP@ ( field -- n ) CAP-STATE + @ ;
PARSE-NAME CAPTURE HEADER 66 , 0 , REVEAL   \ a CODE word (code field 66); the address of its code comes below

PUBLIC
: IR-CAP-POS ( -- n ) RX4-STATUS @ $3FF AND ;
: IR-CAP-ON ( -- )
  ENABLED @ ABORT" IR-CAPTURE: already running"
  ['] CAPTURE CELL+ @ 0= ABORT" IR-CAPTURE: handler was not assembled"
  4 BLOCKS@ 4 <> ABORT" IR-CAPTURE: RX4 needs four blocks"
  RMT-SOURCE INT-OFF 4 RMT-RX-OFF
  CAP-STATE 36 0 FILL CAP-RAW CAP-SLOTS CAP-SIZE * 0 FILL
  MICROS CAP-STATE C-END-US + !
  ['] CAPTURE RMT-SOURCE INT! 4 TRUE RMT-RX-INT
  RMT-SOURCE INT-ON +INT 4 RMT-RX-ON TRUE ENABLED ! ;
: IR-CAP-OFF ( -- )
  ENABLED @ IF
    4 FALSE RMT-RX-INT RMT-SOURCE INT-OFF 4 RMT-RX-OFF
    0 RMT-SOURCE INT! FALSE ENABLED !
  THEN
  CAP-RAW CAP-SLOTS CAP-SIZE * 0 FILL  0 CAP-STATE C-WRITE + !  0 CAP-STATE C-READ + ! ;
: IR-CAP-PENDING? ( -- f ) C-WRITE CAP@ C-READ CAP@ <> ;
: IR-CAP-FRAME? ( -- a n us true | false )
  IR-CAP-PENDING? 0= IF FALSE EXIT THEN
  C-READ CAP@ SLOT  DUP 8 +  OVER @  ROT CELL+ @  TRUE ;
: IR-CAP-RELEASE ( -- )
  IR-CAP-PENDING? 0= ABORT" IR-CAPTURE: no frame to release"
  1 CAP-STATE C-READ + +! ;
: IR-CAP-LAST ( -- us ) C-END-US CAP@ ;
: .IR-CAP ( -- )
  ." raw full/long/high-water/IRQs/max cycles: "
  C-FULL CAP@ . C-LONG CAP@ . C-HIGH CAP@ . C-IRQS CAP@ . C-MAX CAP@ . CR ;
END-PACKAGE

\ The handler. The assembler and this package go again after the link (IRC-TOOLS).
MARKER IRC-TOOLS
ONLY FORTH
#require ADD, ../../../asm/xtensa.fs
ONLY FORTH
#require ASM asm.fs
ONLY FORTH
PACKAGE IRC-BUILD ALSO ASM ALSO ROM ALSO BOOTIMG ALSO CHIP ALSO IR-CAPTURE.I ALSO RMT.I
INT-RAW RMT-BASE - CONSTANT >RAW      \ the registers of the RMT, from RMT-BASE in a2
INT-CLR RMT-BASE - CONSTANT >CLR
RX4-CONF1 RMT-BASE - CONSTANT >CONF1
CODE CAP-ASM   \ a2 RMT-BASE, a3 events, a4 CAP-STATE, a6 the slot, a7 the RAM, a8 the words, a9 cycles
  A9 CCOUNT RSR,                                         \ the cycles at the start
  A2 RMT-BASE CONST,  A3 A2 >RAW L32I,  A5 RX4-EVENTS CONST,  A3 A3 A5 AND,
  A3 L9 BEQZ,                                            \ no event of channel 4: not ours
  A4 CAP-STATE >CHIP CONST,
  A5 A4 C-IRQS L32I,  A5 A5 1 ADDI.N,  A5 A4 C-IRQS S32I,
  A5 TG0 CONST,  A6 0 MOVI.N,  MEMW,  A6 A5 TG-UPDATE S32I,   \ the time of the end, as MICROS
  MEMW,  A6 A5 TG-LO L32I,  A6 A4 C-END-US S32I,
  A5 A2 >CONF1 L32I,  A6 RX-RUN MOVI.N,  A5 A5 A6 OR,  A5 A5 A6 XOR,
  MEMW,  A5 A2 >CONF1 S32I,                              \ the reception off; the RAM to the CPU
  A3 14 L3 BBSI,                                         \ the RAM is full: a long frame
  A5 A4 C-TAIL L32I,  A5 L7 BNEZ,                        \ the end of a long frame: drop it too
  A5 A4 C-WRITE L32I,  A6 A4 C-READ L32I,  A6 A5 A6 SUB,
  A7 CAP-SLOTS MOVI.N,  A6 A7 L2 BGEU,                   \ all slots in use
  A6 A5 0 2 EXTUI,  A7 CAP-SIZE CONST,  A6 A6 A7 MULL,   \ the slot: written mod 4
  A7 CAP-RAW >CHIP CONST,  A6 A6 A7 ADD.N,
  A7 RX4-RAM CONST,  A8 0 MOVI.N,
LABEL L1                                                 \ copy the pulse words up to the end mark
  A3 A7 0 L32I,  A5 A8 2 SLLI,  A5 A5 A6 ADD.N,  A3 A5 8 S32I,
  A7 A7 4 ADDI.N,  A8 A8 1 ADDI.N,
  A5 A3 0 15 EXTUI,  A5 L4 BEQZ,                         \ a half with the time 0: the end
  A5 A3 16 15 EXTUI,  A5 L4 BEQZ,
  A5 CAP-WORDS MOVI,  A8 A5 L1 BNE,
LABEL L3                                                 \ long: count it; drop the rest up to its end
  A5 A4 C-LONG L32I,  A5 A5 1 ADDI.N,  A5 A4 C-LONG S32I,
  A5 1 MOVI.N,  A5 A4 C-TAIL S32I,  L5 J,
LABEL L7                                                 \ the tail has ended: take the next frames
  A5 0 MOVI.N,  A5 A4 C-TAIL S32I,  L5 J,
LABEL L2                                                 \ full: count it
  A5 A4 C-FULL L32I,  A5 A5 1 ADDI.N,  A5 A4 C-FULL S32I,  L5 J,
LABEL L4                                                 \ the end mark after a8 words
  A8 1 L8 BNEI,  A3 15 L5 BBCI,                          \ one word without light: no frame
LABEL L8                                                 \ the slot is done: its count and end
  A8 A6 0 S32I,  A5 A4 C-END-US L32I,  A5 A6 4 S32I,
  MEMW,  A5 A4 C-WRITE L32I,  A5 A5 1 ADDI.N,  A5 A4 C-WRITE S32I,
  A6 A4 C-READ L32I,  A6 A5 A6 SUB,  A7 A4 C-HIGH L32I,
  A6 A7 L5 BLTU,  A6 A4 C-HIGH S32I,                     \ the most slots in use
LABEL L5                                                 \ the reception on again, from the start of the RAM
  A3 A2 >CONF1 L32I,  A5 RX-RUN MOVI.N,  A3 A3 A5 OR,  A3 A3 A5 XOR,
  MEMW,  A3 A2 >CONF1 S32I,
  A5 RX-RESET MOVI.N,  A3 A3 A5 OR,  MEMW,  A3 A2 >CONF1 S32I,
  A3 A3 A5 XOR,  MEMW,  A3 A2 >CONF1 S32I,
  A5 RX-RUN MOVI.N,  A3 A3 A5 OR,  MEMW,  A3 A2 >CONF1 S32I,
  A3 RX4-EVENTS CONST,  MEMW,  A3 A2 >CLR S32I,          \ the events of channel 4 go
  A3 CCOUNT RSR,  A3 A3 A9 SUB,  A5 A4 C-MAX L32I,
  A3 A5 L9 BLTU,  A3 A4 C-MAX S32I,                      \ the longest run
LABEL L9
  RET,
END-CODE
' CAP-ASM >CODE ' CAPTURE CELL+ !
CODE-HERE @ ' CAP-ASM >CODE - CAP-BYTES !
END-PACKAGE
IRC-TOOLS ALSO IR-CAPTURE
