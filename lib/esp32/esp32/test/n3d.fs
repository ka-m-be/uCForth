\ n3d.fs - WROOM RAM-only arithmetic tests; use with ucf term, not INCLUDE
\ No PHY, radio, network, flash writes or real credentials.
\ Check lsof on the port before starting. This file resets the board.
#reset
ONLY FORTH DEFINITIONS ALSO INTERNALS
VARIABLE D0 VARIABLE V0
: SNAP-SIZE ( -- )  HERE D0 ! VP @ V0 ! ;
: .SIZE ( a n -- )  TYPE HERE D0 @ - . VP @ V0 @ - . CR ;
SNAP-SIZE
#require BYTES ../../../net/bytes.fs
S" bytes " .SIZE
SNAP-SIZE
#require SHA1 ../../../sha1.fs
S" sha1 " .SIZE
SNAP-SIZE
#require KDF1 ../../../kdf1.fs
S" kdf1 " .SIZE
SNAP-SIZE
#require AES128 ../../../aes128.fs
S" aes128 " .SIZE
SNAP-SIZE
#require CCM ../../../ccm.fs
S" ccm " .SIZE
SNAP-SIZE
#require WPAKEY ../../../net/wpakey.fs
S" wpakey " .SIZE
ONLY FORTH DEFINITIONS ALSO INTERNALS
." free dictionary/data: " UNUSED . DATA-END VP @ - . CR
#include ../../../../forth/test/tester.fs
ONLY FORTH DEFINITIONS
: KDF-SHORT ;
#include ../../../test/sha1.fs
#include ../../../test/kdf1.fs
#include ../../../test/aes128.fs
#include ../../../test/ccm.fs
#include ../../../net/test/wpakey.fs
ONLY FORTH DEFINITIONS ALSO CLOCK ALSO TIMERS ALSO KDF1 ALSO BYTES
VARIABLE TICKS VARIABLE MAX-STEP VARIABLE START-MS
: TICK ( -- )  1 TICKS +! ;
: CHUNK ( -- )
  MILLIS >R 8 PBKDF2-STEP DROP MILLIS R> - MAX-STEP @ MAX MAX-STEP ! ;
: PART ( -- )  64 0 DO CHUNK LOOP ;
0 TICKS ! 0 MAX-STEP ! ' TICK 10 EVERY MILLIS START-MS !
RFC4096-START
PART
PART
PART
PART
PART
PART
PART
PART
T{ 1 PBKDF2-STEP KOUT P4096 20 BYTES= -> TRUE TRUE }T
RFC25-START
PART
PART
PART
PART
PART
PART
PART
PART
PART
PART
PART
PART
PART
PART
PART
PART
T{ 1 PBKDF2-STEP KOUT P25 25 BYTES= -> TRUE TRUE }T
RFCNUL-START
PART
PART
PART
PART
PART
PART
PART
PART
T{ 1 PBKDF2-STEP KOUT PNUL 16 BYTES= -> TRUE TRUE }T
PMK-START
PART
PART
PART
PART
PART
PART
PART
PART
PART
PART
PART
PART
PART
PART
PART
PART
T{ 1 PBKDF2-STEP PMK EPMK 32 BYTES= -> TRUE TRUE }T
' TICK 0 EVERY
MILLIS START-MS @ - START-MS !
T{ MAX-STEP @ 100 < TICKS @ 10 * START-MS @ - ABS 50 < -> TRUE TRUE }T
." derivation ms/max step ms/timer ticks: " START-MS @ . MAX-STEP @ . TICKS @ . CR
S" n3d-long" .RESULTS
." n3d total: " TOTAL-TESTS @ . TOTAL-ERRORS @ . CR
PBKDF2-CANCEL HMAC1-CLEAR
ONLY FORTH ALSO AES128 AES128-CLEAR
#reset
#bye
