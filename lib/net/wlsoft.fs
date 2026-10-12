\ wlsoft.fs - package WLINK-SOFT: CCMP in Forth for WIFI-LINK, for a radio without hardware CCMP
\
\ WIFI-LINK (wlink.fs) needs the CCMP of each data frame. ESP32-RADIO does it
\ in its hardware (RADIO-CCMP?); a radio without it needs this file. It sets
\ the hooks of WIFI-LINK (SOFT-START SOFT-RX SOFT-TX SOFT-STEP SOFT-STOP):
\ AES-CCM (package CCM) decrypts a received frame and encrypts a frame to
\ send, in steps of LINK-TICK, so a step stays short. The AES state of the
\ job and that of the foreground (WLAN uses AES too) are kept apart. It has
\ no words of its own for the user.
\
\ Load it right after wlink.fs, on a radio without hardware CCMP:
\   #require WLINK-SOFT lib/net/wlsoft.fs        (ucf term)
\   ALSO FILES  INCLUDE lib/net/wlsoft.fs        (host)
\ The test on the host: lib/net/test/wlink.fs.

PACKAGE WLINK-SOFT ALSO WIFI-LINK ALSO WIFI-LINK.I ALSO RADIO ALSO WLAN ALSO WLAN.I ALSO CCMP ALSO CCM ALSO AES128
13 BUFFER: NONCE
24 BUFFER: AAD
AES128-CONTEXT BUFFER: JOB-AES        \ the AES state of the job
AES128-CONTEXT BUFFER: SAVED-AES      \ that of the foreground, meanwhile
: WITH-AES ( i*x xt -- j*x )   \ xt with the AES state of the job; the foreground keeps its own
  SAVED-AES AES128-SAVE  CATCH  JOB-AES AES128-SAVE  SAVED-AES AES128-RESTORE  THROW ;
: RX-START ( key -- )   \ J-DECRYPT: AES-CCM of the received frame
  AES128-KEY
  RAW-R @ RAW-BUF JOB-H @ AAD CCMP-AAD
  RAW-R @ RAW-BUF PN JOB-TID @ 16 = IF 0 ELSE JOB-TID @ THEN NONCE CCMP-NONCE
  >R NONCE AAD R>
  RAW-R @ RAW-BUF JOB-H @ 8 + +
  JOB-N @ JOB-H @ - 16 - DUP JOB-PLEN !
  RAW-R @ RAW-BUF JOB-N @ + 8 - TRUE CCM-START ;
: TX-START ( -- )   \ J-ENCRYPT: AES-CCM of the frame for the radio, with the key of the pair
  RADIO-BUF 24 AAD CCMP-AAD >R RADIO-BUF PN 0 NONCE CCMP-NONCE
  STA-PTK 32 + AES128-KEY
  NONCE AAD R> RADIO-BUF 32 + JOB-PLEN @ 8 +
  RADIO-BUF 40 + JOB-PLEN @ + FALSE CCM-START ;
: STEP ( -- )   \ a step of the CCM; at its end the frame goes on
  SAVED-AES AES128-SAVE JOB-AES AES128-RESTORE
  ['] CCM-STEP CATCH DUP IF  >R SAVED-AES AES128-RESTORE R> THROW  THEN DROP
  >R JOB-AES AES128-SAVE SAVED-AES AES128-RESTORE R>
  IF  JOB @ J-DECRYPT = IF RX-DONE ELSE J-READY JOB ! THEN  THEN ;
: STOP ( -- )   CCM-CANCEL  JOB-AES AES128-CONTEXT 0 FILL  SAVED-AES AES128-CONTEXT 0 FILL ;
: RX-SOFT ( key -- )   ['] RX-START WITH-AES ;
: TX-SOFT ( -- )   ['] TX-START WITH-AES ;
' RADIO-SOFT-CCMP IS SOFT-START
' RX-SOFT IS SOFT-RX
' TX-SOFT IS SOFT-TX
' STEP IS SOFT-STEP
' STOP IS SOFT-STOP
END-PACKAGE
