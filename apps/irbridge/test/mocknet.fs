\ mocknet.fs - the mocks of LINK and NET for the tests of the IR bridge (apps/irbridge/test/)
\
\ The bridge takes from them only the MAC (its ID) and its address. The
\ test of the web page takes the real ones instead (our TCP/IP over the
\ loop). Load it before mocks.fs.
CREATE MAC  $A0 C, $B1 C, $C2 C, $D3 C, $E4 C, $F5 C,
PACKAGE LINK  PUBLIC  : LINK-MAC ( -- a )   MAC ;  END-PACKAGE
PACKAGE NET   PUBLIC  : MY-IP ( -- ip )   $C0A80114 ;  END-PACKAGE   \ 192.168.1.20
