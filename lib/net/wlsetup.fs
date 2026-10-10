\ wlsetup.fs - local, unechoed passphrase input; no password in the source line
PACKAGE WLAN-SETUP ALSO WLAN ALSO WIFI ALSO IDLE
64 BUFFER: PASSWORD
VARIABLE OLD-ECHO
: INPUT-PASSWORD ( i -- )
  ." passphrase (local, no echo; finish with Enter): "
  PASSWORD 64 ACCEPT PASSWORD SWAP WIFI-DERIVE ;
: PROTECTED-INPUT ( i -- ) -BREAK ['] INPUT-PASSWORD CATCH +BREAK THROW ;
PUBLIC
: WLAN-PASSWORD ( i -- )
  DUP WIFI-WPA2? 0= ABORT" WLAN: WPA2-PSK/CCMP without required PMF needed"
  WIFI-STATUS 0<> ABORT" WLAN: leave before entering another profile"
  PASSWORD 64 0 FILL ECHO @ OLD-ECHO ! 0 ECHO !
  ['] PROTECTED-INPUT CATCH
  PASSWORD 64 0 FILL OLD-ECHO @ ECHO !
  ?DUP IF WIFI-LEAVE THROW THEN
  ." key derivation started; use WIFI-POLL or WIFI-WAIT" CR ;
END-PACKAGE
