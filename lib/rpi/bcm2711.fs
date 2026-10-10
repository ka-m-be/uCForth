\ bcm2711.fs - package BCM2711: the chip of the Raspberry Pi 4 and 400
\
\ The registers of the peripherals, the mailbox to the GPU, and a
\ framebuffer from the GPU. The addresses are the addresses of the chip
\ (targets/rpi4: VM addresses are chip addresses), in the "low
\ peripheral" mode of the firmware: the peripherals start at $FE000000.
\
\   buf ch MBOX-CALL ( -- f )   send the message at buf (16-byte aligned)
\                               on channel ch, wait for the answer
\   w h FB-ALLOC ( -- a pitch w h )
\                               a framebuffer of w x h pixels, 32 bits
\                               each; a = 0 if the GPU gives none
\
\ The mailbox is in the Broadcom documentation "Mailbox property
\ interface" (github.com/raspberrypi/firmware/wiki).

PACKAGE BCM2711   \ lib/package.fs

PUBLIC
$FE000000 CONSTANT PERI
PERI $00B880 + CONSTANT MBOX   \ the mailbox 0: read at 0, status at $18, write at $20
PERI $200000 + CONSTANT GPIO
PERI $201000 + CONSTANT UART0

PRIVATE
$80000000 CONSTANT FULL   $40000000 CONSTANT EMPTY
: STATUS ( -- x )   MBOX $18 + @ ;

PUBLIC
: MBOX-CALL ( buf ch -- f )   \ f true: the GPU answered "done"
  OVER OR                                        ( buf msg )
  BEGIN STATUS FULL AND 0= UNTIL  DUP MBOX $20 + !
  BEGIN
    BEGIN STATUS EMPTY AND 0= UNTIL
    DUP MBOX @ =
  UNTIL  DROP  CELL+ @ $80000000 = ;

PRIVATE
160 BUFFER: MRAW
MRAW 15 + -16 AND CONSTANT MSG   \ 16-byte aligned, 36 cells
: M! ( x i -- )   CELLS MSG + ! ;
: M@ ( i -- x )   CELLS MSG + @ ;

PUBLIC
: FB-ALLOC ( w h -- a pitch w h )
  MSG 140 0 FILL
  140 0 M!                                   \ size; 1 M! = 0: a request
  $48003 2 M!  8 3 M!  OVER 5 M!  DUP 6 M!    \ physical size
  $48004 7 M!  8 8 M!  SWAP 10 M!  11 M!      \ virtual size
  $48009 12 M!  8 13 M!                       \ virtual offset 0 0
  $48005 17 M!  4 18 M!  32 20 M!             \ 32 bits per pixel
  $48006 21 M!  4 22 M!  1 24 M!              \ pixel order RGB
  $40001 25 M!  8 26 M!  4096 28 M!           \ allocate, aligned on 4 KB
  $40008 30 M!  4 31 M!                       \ get the pitch
  MSG 8 MBOX-CALL  28 M@ AND                  \ the address is 0 on an error
  $3FFFFFFF AND  33 M@  5 M@  6 M@ ;          \ bus address -> ARM address

END-PACKAGE
