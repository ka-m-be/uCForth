\ romsyms.fs - the functions in the ROM of the ESP32 that libphy, libpp and librtc call
\
\ EXTERN gives each name its address, and the literals that wait for it
\ get it (CLINK, clink.fs). The addresses are from the linker scripts
\ esp32.rom*.ld of ESP-IDF, as PlatformIO has them (package
\ framework-arduinoespressif32 3.20017); __divsf3 only from the older
\ script of the Arduino core 1.0.4. The ROM of the chip does not change,
\ so they hold for each version of ESP-IDF. Later this list belongs to
\ the layer of the chip, which has the functions of the ROM (ucf:
\ lib/esp32/esp32/chip.fs; README.md, "Für ucf vorgesehen").
\
\ Load it before the packages: then no literal has to wait for these
\ names. Needs CLINK (clink.fs).

ALSO CLINK
$40008534 EXTERN ets_delay_us         \ esp32.rom.ld; libphy libpp librtc
$400090CC EXTERN uart_div_modify      \ esp32.rom.ld; librtc
$40004100 EXTERN phy_get_romfuncs     \ esp32.rom.ld; libphy
$4000AB7C EXTERN roundup2             \ esp32.rom.ld; libpp
$4000C2C8 EXTERN memcpy               \ esp32.rom.newlib-funcs.ld; libphy libpp
$4000C44C EXTERN memset               \ esp32.rom.newlib-funcs.ld; libpp
$4000C260 EXTERN memcmp               \ esp32.rom.newlib-funcs.ld; libpp
$40056C08 EXTERN sprintf              \ esp32.rom.newlib-nano.ld; libphy; needs the newlib data of the ROM (README.md)
\ $4000BEB8 EXTERN free               \ esp32.rom.syscalls.ld; libpp: the heap of the ROM, not usable (README.md)
$40002590 EXTERN __adddf3             \ esp32.rom.libgcc.ld; libpp
$400026E4 EXTERN __subdf3             \ esp32.rom.libgcc.ld; libpp
$4006358C EXTERN __muldf3             \ esp32.rom.libgcc.ld; libpp
$40002954 EXTERN __divdf3             \ esp32.rom.libgcc.ld; libphy
$4000234C EXTERN __divsf3             \ esp32.rom.libgcc.ld of the core 1.0.4 only; libphy libpp
$4000CA84 EXTERN __divdi3             \ esp32.rom.libgcc.ld; libphy
$4000CFF8 EXTERN __udivdi3            \ esp32.rom.libgcc.ld; libpp
$4000D280 EXTERN __umoddi3            \ esp32.rom.libgcc.ld; libpp
$4000C944 EXTERN __floatsidf          \ esp32.rom.libgcc.ld; libpp
$4000C938 EXTERN __floatunsidf        \ esp32.rom.libgcc.ld; libphy libpp
$4000C8B0 EXTERN __floatundisf        \ esp32.rom.libgcc.ld; libpp
$40002C34 EXTERN __extendsfdf2        \ esp32.rom.libgcc.ld; libpp
$40002B90 EXTERN __truncdfsf2         \ esp32.rom.libgcc.ld; libphy libpp
PREVIOUS
