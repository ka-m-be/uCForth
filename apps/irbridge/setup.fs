\ setup.fs - package BRIDGE-SETUP: the IR bridge gets its WLAN and broker without a terminal
\
\ The setup (orga/Archiv/013, section 7): the bridge and the station stop, the
\ board listens for the networks around (WIFI-SCAN), then it opens the
\ setup network IRBRIDGE- and the last 6 hex digits of its ID (BRIDGE-ID),
\ open, for the first device only (SETUP-AP: the board is 192.168.77.1).
\ The page of the setup (BRIDGE-WEB, web.fs) shows the networks with WPA2
\ and takes the network, the passphrase and the broker; then it calls
\ BRIDGE-SETUP-JOIN. 2 s later (the answer is out) the setup network
\ closes, the board looks for the networks again and takes the chosen one
\ by its name, as the console does it (WLAN-PASSWORD after WIFI-SCAN); the
\ station derives the key from the passphrase (about 50 s on the WROOM)
\ and joins the network. When that works, the profile
\ (WLAN-SAVE) and the broker (BRIDGE-SAVE) go into the flash, and the chip
\ starts again (BRIDGE-RESTART): with BRIDGE-BOOT-ON the bridge starts with
\ them. When it fails, the setup network opens again, the page shows the
\ code, and the console a line with it (for -2 its text). After 5 min
\ without a try the chip starts again with the old
\ settings. BRIDGE-SETUP-START does it all at the console; the button BOOT
\ starts it without a terminal (BRIDGE-BOOT, boot.fs).
\
\   BRIDGE-SETUP-START ( -- )   the bridge and the station stop, a scan, then
\                     the setup network for 5 min
\   i a n BRIDGE-SETUP-JOIN ( -- )   network i of the scan, the passphrase a n
\                     (8 to 63 characters): the try begins 2 s later
\   BRIDGE-SETUP? ( -- f )   the setup network is open
\   BRIDGE-SETUP-NETS ( -- u )   the networks of the scan: WIFI-NET 0 to u-1
\   BRIDGE-SETUP-CODE ( -- ior )   the last try: 0, or the code why it failed
\   BRIDGE-SETUP-PHASE ( -- n )   0 none, 1 the setup network is open, 2 the
\                     answer goes out, 3 the try (the key, the join)
\   BRIDGE-RESTART ( -- )   the end of the setup: 100 ms for the last line,
\                     then BYE, the chip starts again; the test sets another
\                     word (DEFER)
\   .BRIDGE-SETUP ( -- )   the phase (0 none, 1 open, 2 the answer goes out,
\                     3 the try), the networks of the scan, the code
\
\ The passphrase stays in the RAM only up to the start of the derivation,
\ then it is 0, as with WLAN-PASSWORD; the flash gets only the key (WLSAVE).
\ The restart at the end gives the RAM its frozen state again. The setup
\ network is open: the passphrase goes over the air readable for all
\ (lib/esp32/esp32/wlap.fs). Load it after SETUP-AP, WLAN-BACK, WLSAVE,
\ IRBRIDGE and BRIDGE-SETTINGS (apps/irbridge/app.fs does it). Test on the
\ host: apps/irbridge/test/setup.fs.

PACKAGE BRIDGE-SETUP  ALSO SETUP-AP  ALSO RADIO  ALSO WIFI  ALSO WLAN  ALSO WLSAVE  ALSO WLAN-BACK
ALSO BRIDGE-SETTINGS  ALSO IRBRIDGE  ALSO TEXT  ALSO IDLE   \ lib/package.fs

0 CONSTANT S-NONE   1 CONSTANT S-OPEN   2 CONSTANT S-ANSWER   3 CONSTANT S-TRY   \ the phases
6 CONSTANT AP-CHANNEL                \ the channel of the setup network
VARIABLE PHASE   VARIABLE DUE        \ MILLIS: the end of the phase
VARIABLE #FOUND   VARIABLE TRY-CODE
32 BUFFER: CHOSEN   VARIABLE CHOSEN-N   \ the name of the chosen network
64 BUFFER: PHRASE   VARIABLE PHRASE-N   \ the passphrase, up to the derivation
16 BUFFER: NET-NAME                  \ counted: IRBRIDGE- and 6 hex digits

: RESTART-NOW ( -- )   100 MS  BYE ;   \ the last line goes out first
PUBLIC
DEFER BRIDGE-RESTART   ' RESTART-NOW IS BRIDGE-RESTART
PRIVATE
: DUE-IN ( ms -- )   MILLIS + DUE ! ;
: DUE? ( -- f )   MILLIS DUE @ - 0< 0= ;
: RADIO-ON ( -- )   RADIO-START RADIO-FACTORY ;
: OPEN-NET ( -- )   \ the networks around, then the setup network for 5 min
  RADIO-ON  S" IRBRIDGE-" NET-NAME PLACE  RADIO-MAC@ BRIDGE-ID 6 /STRING NET-NAME +PLACE
  WIFI-SCAN #FOUND !  RADIO-STOP
  NET-NAME COUNT AP-CHANNEL SETUP-START  S-OPEN PHASE !  300000 DUE-IN ;
: DERIVE ( -- )   \ the chosen network in the new scan; the derivation of its key starts
  CHOSEN CHOSEN-N @ WIFI-FIND 0= ABORT" BRIDGE-SETUP: the network is not in the scan"
  PHRASE PHRASE-N @ WIFI-DERIVE ;
: TRY-NET ( -- )   \ the setup network off, a new scan; the station derives the key and joins
  SETUP-STOP  RADIO-ON  WIFI-SCAN #FOUND !
  ['] DERIVE CATCH  PHRASE 64 0 FILL  THROW  S-TRY PHASE ! ;
: END-SETUP ( -- )   S-NONE PHASE !  SETUP-STOP  BRIDGE-RESTART ;
: FAILED ( ior -- )   \ the code for the page and the console; the setup network again
  DUP TRY-CODE !  ." bridge setup: the try failed, code " DUP .
  -2 = IF  ERRMSG @ ERRMSG CELL+ @ TYPE  THEN  CR
  WIFI-LEAVE  RADIO-STOP  OPEN-NET ;
: STEP ( -- )
  PHASE @ CASE
    S-OPEN OF  DUE? IF END-SETUP THEN  ENDOF            \ 5 min without a try
    S-ANSWER OF  DUE? IF TRY-NET THEN  ENDOF            \ the answer is out
    S-TRY OF
      WIFI-POLL
      WIFI-STATUS 6 = IF  WLAN-SAVE  BRIDGE-SAVE  END-SETUP  THEN
      WIFI-STATUS 7 = IF  WIFI-ERROR FAILED  THEN
    ENDOF
  ENDCASE ;
: WORK ( -- )   \ the word in IDLE: an error is the code of the try; when even that fails, the end
  ['] STEP CATCH  ?DUP 0= IF EXIT THEN
  DUP -28 = IF THROW THEN                               \ Ctrl-C as at the console
  ['] FAILED CATCH IF  DROP END-SETUP  THEN ;

PUBLIC
: BRIDGE-SETUP-START ( -- )
  PHASE @ ABORT" BRIDGE-SETUP: started already"
  BRIDGE-STOP  WBACK-STOP  BRIDGE-LOAD DROP              \ the saved broker for the page
  0 TRY-CODE !  OPEN-NET  ['] WORK IDLE+ ;
: BRIDGE-SETUP-JOIN ( i a n -- )
  PHASE @ S-OPEN <> ABORT" BRIDGE-SETUP: the setup network is not open"
  DUP 8 64 WITHIN 0= ABORT" BRIDGE-SETUP: a passphrase has 8 to 63 characters"
  DUP PHRASE-N !  PHRASE SWAP MOVE
  WIFI-NET 2DROP  DUP CHOSEN-N !  CHOSEN SWAP MOVE      \ the name: the try finds it in a new scan
  S-ANSWER PHASE !  2000 DUE-IN ;
: BRIDGE-SETUP? ( -- f )   PHASE @ S-OPEN = ;
: BRIDGE-SETUP-NETS ( -- u )   #FOUND @ ;
: BRIDGE-SETUP-CODE ( -- ior )   TRY-CODE @ ;
: BRIDGE-SETUP-PHASE ( -- n )   PHASE @ ;
: .BRIDGE-SETUP ( -- )
  ." bridge setup phase/networks/code: " PHASE @ . #FOUND @ . TRY-CODE @ . CR ;

END-PACKAGE
