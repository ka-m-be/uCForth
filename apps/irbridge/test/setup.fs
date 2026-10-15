\ setup.fs - the test of BRIDGE-SETUP on the host: the setup without a terminal (apps/irbridge/)
\
\ SETUP-AP, RADIO, WIFI, WLAN, WLSAVE, WLAN-BACK, BRIDGE-SETTINGS and
\ IRBRIDGE are mocks here: they note what the setup asks of them, and the
\ scan finds 3 networks; WIFI-FIND notes the name and finds it at
\ FOUND-AT (-1: not). BRIDGE-RESTART only counts. The steps run with
\ IDLE-RUN, as KEY runs them on the board. 32-bit cells (5 min in ms); in
\ the folder ucf:
\   build/ucf-c -32 host/files.fs apps/irbridge/test/setup.fs </dev/null
\ tools/test.fs runs it too. At the end: "setup: n tests, 0 errors".

ONLY FORTH DEFINITIONS  ALSO FILES
[DEFINED] T{ S" forth/test/tester.fs" ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] TEXT  S" lib/text.fs"  ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] IDLE  S" lib/idle.fs"  ?INCLUDED
ONLY FORTH DEFINITIONS  ALSO TEXT
VARIABLE RADIO?   VARIABLE #SCANS   VARIABLE #APS   VARIABLE #AP-STOPS   VARIABLE AP-FAIL
64 BUFFER: AP-NAME   VARIABLE AP-CH
VARIABLE M-STATUS   VARIABLE M-ERROR   VARIABLE #POLLS   VARIABLE #LEAVES   VARIABLE DERIVE-FAIL
VARIABLE D-NET   64 BUFFER: D-PASS
VARIABLE #WSAVES   VARIABLE #WSTOPS   VARIABLE #BSAVES   VARIABLE #LOADS   VARIABLE #BSTOPS
64 BUFFER: F-NAME   VARIABLE FOUND-AT   2 FOUND-AT !
CREATE M-MAC  $A0 C, $B1 C, $C2 C, $D3 C, $E4 C, $F5 C,
PACKAGE RADIO  PUBLIC
: RADIO-START ( -- )   RADIO? ON! ;   : RADIO-STOP ( -- )   RADIO? OFF! ;
: RADIO-FACTORY ( -- ) ;   : RADIO-MAC@ ( -- a )   M-MAC ;
END-PACKAGE
PACKAGE WIFI  ALSO TEXT  PUBLIC
: WIFI-SCAN ( -- u )   1 #SCANS +!  3 ;
: WIFI-NET ( i -- a n ch dbm )   CASE  0 OF S" Alpha" ENDOF  1 OF S" Home" ENDOF  S" Guest" ROT  ENDCASE  6 -50 ;
: WIFI-FIND ( a n -- i true | false )   F-NAME PLACE  FOUND-AT @ DUP 0< IF DROP FALSE ELSE TRUE THEN ;
END-PACKAGE
PACKAGE SETUP-AP  ALSO TEXT  PUBLIC
: SETUP-START ( a n ch -- )
  AP-FAIL @ ABORT" mock: no AP"  RADIO? @ ABORT" mock: the radio is on"  AP-CH !  AP-NAME PLACE  1 #APS +! ;
: SETUP-STOP ( -- )   1 #AP-STOPS +! ;
END-PACKAGE
PACKAGE WLAN  ALSO TEXT  PUBLIC
: WIFI-DERIVE ( i pass n -- )   DERIVE-FAIL @ ABORT" mock: no derivation"  D-PASS PLACE  D-NET !  1 M-STATUS ! ;
: WIFI-POLL ( -- )   1 #POLLS +! ;
: WIFI-STATUS ( -- n )   M-STATUS @ ;
: WIFI-ERROR ( -- ior )   M-ERROR @ ;
: WIFI-LEAVE ( -- )   1 #LEAVES +!  0 M-STATUS ! ;
END-PACKAGE
PACKAGE WLSAVE  ALSO WLAN  PUBLIC
: WLAN-SAVE ( -- )   WIFI-STATUS 6 <> ABORT" mock: not joined"  1 #WSAVES +! ;
END-PACKAGE
PACKAGE WLAN-BACK  PUBLIC  : WBACK-STOP ( -- )   1 #WSTOPS +! ;  END-PACKAGE
PACKAGE BRIDGE-SETTINGS  PUBLIC
: BRIDGE-SAVE ( -- )   1 #BSAVES +! ;   : BRIDGE-LOAD ( -- f )   1 #LOADS +!  TRUE ;
END-PACKAGE
PACKAGE IRBRIDGE  PUBLIC
: BRIDGE-STOP ( -- )   1 #BSTOPS +! ;
: BRIDGE-ID ( mac -- a u )   M-MAC <> ABORT" mock: another MAC"  S" A0B1C2D3E4F5" ;
END-PACKAGE
ONLY FORTH DEFINITIONS  ALSO FILES  INCLUDE apps/irbridge/setup.fs
ONLY FORTH DEFINITIONS  ALSO INTERNALS  ALSO TEXT  ALSO IDLE  ALSO BRIDGE-SETUP  ALSO BRIDGE-SETUP.I
DECIMAL
VARIABLE #RESTARTS   : RESTART ( -- )   1 #RESTARTS +! ;   ' RESTART IS BRIDGE-RESTART
: DUE-NOW ( -- )   MILLIS 1- DUE ! ;      \ the phase ends with the next step
: ZERO? ( a u -- f )   BOUNDS ?DO  I C@ IF  UNLOOP FALSE EXIT  THEN  LOOP  TRUE ;
: PASS ( -- a u )   S" 12345678" ;        \ a passphrase of the test, without meaning
: CAUGHT3 ( x1 x2 x3 n -- n )   >R 2DROP DROP R> ;   \ after CATCH of a word with 3 inputs: the code

\ ---- the start: the bridge and the station stop, a scan, the setup network
T{ BRIDGE-SETUP-START  #BSTOPS @ #WSTOPS @ #LOADS @ #SCANS @ #APS @ -> 1 1 1 1 1 }T
T{ AP-NAME COUNT S" IRBRIDGE-D3E4F5" STR=  AP-CH @  RADIO? @ -> TRUE 6 FALSE }T
T{ BRIDGE-SETUP?  BRIDGE-SETUP-NETS  BRIDGE-SETUP-CODE  BRIDGE-SETUP-PHASE -> TRUE 3 0 1 }T
T{ ' BRIDGE-SETUP-START CATCH  #SCANS @ -> -2 1 }T                 \ only once
T{ IDLE-RUN  BRIDGE-SETUP?  #AP-STOPS @ -> TRUE 0 }T                \ 5 min are not over

\ ---- a try that works: the profile and the broker into the flash, the restart
T{ 7 S" short" ' BRIDGE-SETUP-JOIN CATCH CAUGHT3  PHASE @ -> -2 1 }T
T{ 1 PASS BRIDGE-SETUP-JOIN  BRIDGE-SETUP?  PHASE @ -> FALSE 2 }T
T{ IDLE-RUN  PHASE @  #AP-STOPS @ -> 2 0 }T                          \ the answer goes out
T{ DUE-NOW IDLE-RUN  PHASE @  #AP-STOPS @  RADIO? @  D-NET @  D-PASS COUNT PASS STR= -> 3 1 TRUE 2 TRUE }T
T{ #SCANS @  F-NAME COUNT S" Home" STR= -> 2 TRUE }T                  \ a new scan; the network by its name
T{ PHRASE 64 ZERO? -> TRUE }T                                        \ the passphrase is gone
T{ IDLE-RUN  #POLLS @  PHASE @  #WSAVES @ -> 1 3 0 }T                \ the derivation, the join
T{ 6 M-STATUS !  IDLE-RUN  #WSAVES @ #BSAVES @ #RESTARTS @  PHASE @  #AP-STOPS @ -> 1 1 1 0 2 }T

\ ---- a try that fails: the setup network again, with the code
T{ 0 M-STATUS !  BRIDGE-SETUP-START  #SCANS @  #APS @  BRIDGE-SETUP-CODE -> 3 2 0 }T
T{ 0 PASS BRIDGE-SETUP-JOIN  DUE-NOW IDLE-RUN  PHASE @  F-NAME COUNT S" Alpha" STR= -> 3 TRUE }T
T{ 7 M-STATUS !  -422 M-ERROR !  IDLE-RUN  BRIDGE-SETUP?  BRIDGE-SETUP-CODE -> TRUE -422 }T
T{ #LEAVES @  #SCANS @  #APS @  RADIO? @  #WSAVES @ -> 1 5 3 FALSE 1 }T

\ ---- the network is not in the new scan: the code, the setup network again, the passphrase gone
T{ -1 FOUND-AT !  1 PASS BRIDGE-SETUP-JOIN  DUE-NOW IDLE-RUN  2 FOUND-AT !
   BRIDGE-SETUP?  BRIDGE-SETUP-CODE  PHRASE 64 ZERO?  #APS @ -> TRUE -2 TRUE 4 }T

\ ---- an error in the try: its code, the setup network again, the passphrase gone
T{ DERIVE-FAIL ON!  2 PASS BRIDGE-SETUP-JOIN  DUE-NOW IDLE-RUN  DERIVE-FAIL OFF!
   BRIDGE-SETUP?  BRIDGE-SETUP-CODE  PHRASE 64 ZERO?  #APS @ -> TRUE -2 TRUE 5 }T

\ ---- the setup network does not open again: the end
T{ 0 PASS BRIDGE-SETUP-JOIN  DUE-NOW IDLE-RUN  AP-FAIL ON!  7 M-STATUS !  IDLE-RUN  AP-FAIL OFF!
   PHASE @  #RESTARTS @  BRIDGE-SETUP-CODE -> 0 2 -2 }T
T{ 0 PASS ' BRIDGE-SETUP-JOIN CATCH CAUGHT3 -> -2 }T               \ not open

\ ---- 5 min without a try: the end, with the old settings
T{ BRIDGE-SETUP-START  DUE-NOW IDLE-RUN  PHASE @  #RESTARTS @  #WSAVES @ -> 0 3 1 }T

' WORK IDLE-
S" setup" .RESULTS
