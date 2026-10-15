\ cores.fs - package ESP32-CORES: the second core of the ESP32 (APP_CPU), for MULTICORE
\
\ The part of the chip for the package MULTICORE (lib/esp32/multicore.fs).
\ Core 0 (PRO_CPU) runs the console; this package starts and stops core 1
\ (APP_CPU) over the registers APPCPU_CTRL_A to _D of DPORT, as orga/016
\ measured it (K0). The HAL starts each core with its own state
\ (targets/esp32/hal.fs, "Cores"); its words 106 to 108 are here:
\
\   #CORES ( -- n )           2: the cores of the ESP32
\   THIS-CORE ( -- i )        HAL 106: the core that runs it (bit 13 of PRID)
\   CORE-ENTRY ( xt i -- a )  HAL 107: core i runs xt after its start; a is
\                             START1 of the HAL
\   CORE-PARK ( -- )          HAL 108: this core waits for its reset
\   CORE-SETUP ( i -- )       core i is in reset: its cache and MMU for the
\                             frozen part of the dictionary (as START does
\                             it for core 0, and a pulse on MMU_IA_CLR, as
\                             ESP-IDF does: do_multicore_settings in
\                             cpu_start.c), its clock (timer 1 of timer
\                             group 0 counts on from timer 0), and its CPU
\                             interrupt 23 for the requests of core 0
\   CORE-BOOT ( a i -- )      core i starts at a: its clock on, out of reset
\   CORE-RESET ( i -- )       core i into reset, its clock off
\   CORE-ASK ( i -- )         a request to core i: its CPU interrupt 23
\                             (FROM_CPU 1); the HAL makes a break of it
\   CORES-HOLD ( -- )         core 1 waits (RUNSTALL), its cache off: before
\                             a call of the ROM for the flash ('FLASH-HOLD)
\   CORES-FREE ( -- )         its cache flushed and on, it goes on
\
\ After END-PACKAGE, CHIP-CORES ( -- ) gives this package (as CHIP the
\ layer of the chip): MULTICORE writes ALSO CHIP-CORES. Only core 0 uses
\ these words; THIS-CORE and CORE-PARK also core 1. The cache waits and
\ flushes as in the HAL (CACHE-ON); the registers are in the Technical
\ Reference Manual of the ESP32 (DPORT, chapter 5; timer group, 18) and
\ in dport_reg.h of the SDK of Arduino-ESP32. Needs ROM and BOOTIMG.
\ Load it with lib/esp32/cores.fs (ucf term: #when). Test on the host:
\ lib/esp32/test/cores.fs (with a mock of this package); on the board:
\ lib/esp32/esp32/test/cores.fs.

PACKAGE ESP32-CORES  ALSO ROM  ALSO BOOTIMG   \ lib/package.fs

$3FF0002C CONSTANT CTRL-A           \ APPCPU_CTRL_A: bit 0 holds core 1 in reset
$3FF00030 CONSTANT CTRL-B           \ APPCPU_CTRL_B: bit 0 its clock
$3FF00034 CONSTANT CTRL-C           \ APPCPU_CTRL_C: bit 0 RUNSTALL, it waits
$3FF00038 CONSTANT CTRL-D           \ APPCPU_CTRL_D: its ROM calls this address after the start
$3FF00058 CONSTANT CACHE            \ DPORT_APP_CACHE_CTRL: bit 3 on, bit 4 flush, bit 5 flush done
$3FF0005C CONSTANT CACHE1           \ DPORT_APP_CACHE_CTRL1: bit 4 masks DROM0 ($3F40_0000 ...), bit 13 MMU_IA_CLR
$3FF00418 CONSTANT DBUG             \ DPORT_APP_DCACHE_DBUG0: bits 18-7 are 1 when the cache rests
$3FF000E0 CONSTANT ASK              \ DPORT_CPU_INTR_FROM_CPU_1: source 25
$3FF0027C CONSTANT ASK-MAP          \ DPORT_APP_CPU_INTR_FROM_CPU_1_MAP: its CPU interrupt on core 1
23 CONSTANT ASK-INT                 \ level 3: LEVEL3 of the HAL
$3FF5F000 CONSTANT TG0              \ timer group 0: timer 0 (core 0) from 0, timer 1 (core 1) from $24
$400095A4 CONSTANT 'MMU-INIT        \ mmu_init ( cpu -- ): all entries of the MMU empty
$400095E0 CONSTANT 'MMU-SET         \ cache_flash_mmu_set ( cpu pid vaddr paddr psize num -- r )
: ?ONE ( i -- )   1 <> ABORT" ESP32-CORES: the ESP32 has only core 1 besides core 0" ;
: CACHE-OFF ( -- )   \ when the cache rests
  BEGIN  DBUG @ 7 RSHIFT $FFF AND  1 =  UNTIL  8 CACHE BIC! ;
: CACHE-ON ( -- )   \ flush, then on
  16 CACHE BIC!  16 CACHE BIS!
  BEGIN  CACHE @ 32 AND  UNTIL  16 CACHE BIC!  8 CACHE BIS! ;
: DROM-MAP ( -- )   \ DROM pages 1 to 3 (VM addresses $10000 to $3FFFF) on the flash, as START
  1 1 'MMU-INIT SYSCALL DROP
  1 0  FE-AT2 CELL+ @ $10000 +  DICT-FLASH $10000 +  64 3  6 'MMU-SET SYSCALL DROP ;
: CLOCK-SYNC ( -- )   \ timer 1 counts as timer 0, from its count on
  TG0 @  TG0 $24 + !                                    \ the configuration: on, up, the divider
  0 TG0 $0C + !  TG0 4 + @  TG0 8 + @                   \ timer 0 now
  TG0 $40 + !  TG0 $3C + !  0 TG0 $44 + ! ;             \ into timer 1

PUBLIC
2 CONSTANT #CORES
PARSE-NAME THIS-CORE HEADER  106 ,  REVEAL
PARSE-NAME CORE-ENTRY HEADER  107 ,  REVEAL
PARSE-NAME CORE-PARK HEADER  108 ,  REVEAL
: CORE-SETUP ( i -- )   \ as ESP-IDF; without the pulse of MMU_IA_CLR its first read of the cache hangs
  ?ONE  CACHE-OFF  DROM-MAP  $2000 CACHE1 BIS!  $2000 CACHE1 BIC!  16 CACHE1 BIC!  CACHE-ON
  CLOCK-SYNC  0 ASK !  ASK-INT ASK-MAP ! ;
: CORE-BOOT ( a i -- )   ?ONE  CTRL-D !  1 CTRL-B !  0 CTRL-C !  1 CTRL-A !  0 CTRL-A ! ;
: CORE-RESET ( i -- )   ?ONE  1 CTRL-A !  0 CTRL-B ! ;
: CORE-ASK ( i -- )   ?ONE  1 ASK ! ;
: CORES-HOLD ( -- )   1 CTRL-C !  CACHE-OFF ;
: CORES-FREE ( -- )   CACHE-ON  0 CTRL-C ! ;

END-PACKAGE
: CHIP-CORES ( -- )   ESP32-CORES ;   \ ALSO CHIP-CORES: the cores of the chip of this board
