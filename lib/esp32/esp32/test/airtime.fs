\ airtime.fs - package AIR-TIME: the time of each NET-STEP in us, with the overhead of the measure (a test)
\ AIR-TIME-START puts the measured step in place of NET-STEP; .AIR-TIME shows count, maximum, steps over 1 ms.
PACKAGE AIR-TIME ALSO NETIF ALSO TIMERS ALSO IDLE ALSO CHIP
VARIABLE STEP-MAX
VARIABLE STEP-N
VARIABLE STEP-OVER
: MEASURE-STEP ( -- )
  MICROS >R NET-STEP MICROS R> -
  DUP STEP-MAX @ MAX STEP-MAX !
  1000 > IF 1 STEP-OVER +! THEN 1 STEP-N +! ;
PUBLIC
: AIR-TIME-START ( -- )
  0 STEP-MAX ! 0 STEP-N ! 0 STEP-OVER !
  ['] NET-STEP 0 EVERY ['] NET-CONTROL IDLE+ ['] MEASURE-STEP 1 EVERY ;
: AIR-TIME-STOP ( -- ) ['] MEASURE-STEP 0 EVERY ;
: .AIR-TIME ( -- )
  ." NET-STEP count/max us/over 1000 us: " STEP-N @ . STEP-MAX @ . STEP-OVER @ . CR ;
END-PACKAGE
