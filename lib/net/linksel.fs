\ linksel.fs - package RADIO-LINK: one TCP/IP over the station or over the setup AP
\
\ A board has one radio and one TCP/IP stack (LINK-NET). RADIO-LINK gives
\ LINK-NET the words of the contract LINK and passes each call on to
\ WIFI-LINK (the station, WLAN-BACK) or to AP-LINK (the setup network,
\ SETUP-AP). Only one of them runs at a time; the board never routes between
\ them, and the mode changes only while the radio is off. Both LINKs use the
\ same packet RAM.
\
\   a n LINK-POOL! ( -- )   the packet RAM for both LINKs: n bytes at a, at least
\                           WLINK-POOL-SIZE and AP-POOL-SIZE
\   LINK-STATION ( -- )     the station from now on (so it is at the start)
\   LINK-ACCESS ( -- )      the setup AP from now on
\   LINK-AP? ( -- f )       the setup AP is selected
\   LINK-MAC LINK-MTU LINK-UP? LINK-SEND LINK-RECEIVE LINK-JOIN LINK-TICK LINK-CONTROL
\                           the contract LINK, of the LINK selected
\
\ Load it in ucf term after WIFI-LINK and AP-LINK (apps/irbridge/app.fs does
\ it), then give it the RAM:  RADIO-POOL LINK-POOL!
\ The test on the host: lib/net/test/linksel.fs.

PACKAGE RADIO-LINK ALSO RADIO ALSO WIFI-LINK
CREATE STATION                        \ the words of WIFI-LINK
' LINK-MAC , ' LINK-UP? , ' LINK-SEND , ' LINK-RECEIVE ,
' LINK-JOIN , ' LINK-TICK , ' LINK-CONTROL ,
ALSO AP-LINK
CREATE ACCESS                         \ the words of AP-LINK
' LINK-MAC , ' LINK-UP? , ' LINK-SEND , ' LINK-RECEIVE ,
' LINK-JOIN , ' LINK-TICK , ' LINK-CONTROL ,
VARIABLE AP-MODE
: CALL ( index -- ) AP-MODE @ IF ACCESS ELSE STATION THEN SWAP CELLS + @ EXECUTE ;
: ?OFF ( -- ) RADIO-CHANNEL@ ABORT" LINK: stop the radio before changing mode" ;
PUBLIC
: LINK-POOL! ( a n -- )
  WLINK-POOL-SIZE AP-POOL-SIZE MAX < ABORT" LINK: the packet RAM is too small"
  DUP WLINK-POOL! AP-POOL! ;
: LINK-AP? ( -- f ) AP-MODE @ ;
: LINK-STATION ( -- ) ?OFF AP-MODE OFF! ;
: LINK-ACCESS ( -- ) ?OFF AP-MODE ON! ;
: LINK-MAC ( -- a ) 0 CALL ;
: LINK-MTU ( -- n ) 1500 ;
: LINK-UP? ( -- f ) 1 CALL ;
: LINK-SEND ( a n -- ior ) 2 CALL ;
: LINK-RECEIVE ( a -- n ) 3 CALL ;
: LINK-JOIN ( ip -- ior ) 4 CALL ;
: LINK-TICK ( -- ) 5 CALL ;
: LINK-CONTROL ( -- ) 6 CALL ;
END-PACKAGE
: LINK ( -- ) RADIO-LINK ;
