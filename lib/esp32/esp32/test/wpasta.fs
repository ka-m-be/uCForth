\ wpasta.fs - synthetic station exchange on WROOM, no PHY, radio or flash
#reset
#require BYTES ../../../net/bytes.fs
#require SHA1 ../../../sha1.fs
#require KDF1 ../../../kdf1.fs
#require AES128 ../../../aes128.fs
#require WPAKEY ../../../net/wpakey.fs
#include ../../../../forth/test/tester.fs
#include ../../../net/test/stamock.fs
#include ../../../net/wifi.fs
#include ../../../net/wpasta.fs
#include ../../../net/test/wpavec.fs
ONLY FORTH DEFINITIONS ALSO TIMERS
VARIABLE TICKS
: TICK ( -- ) 1 TICKS +! ;
: TIMED-EXCHANGE ( -- n ) MILLIS >R FULL MILLIS R> - ;
0 TICKS ! ' TICK 10 EVERY
TIMED-EXCHANGE ." synthetic exchange ms: " . CR
' TICK 0 EVERY
ALSO WLAN
T{ WIFI-STATUS TICKS @ 0 > -> 6 TRUE }T
S" wpasta-board" .RESULTS
#reset
#bye
