#!/usr/bin/env bash
# ucf.sh - the bash bootstrap VM of uCForth
#
# It is the virtual machine of uCForth in bash: the memory,
# the primitives and a small HAL (doc/vm.md). Its Genesis, a small outer
# interpreter, compiles forth/core.fs once (doc/genesis.md). From then on
# QUIT in the image reads each line, as on a board: first forth/base.fs,
# then the files of the command line, then the keyboard (or a pipe).
#
# Use:  bash ucf.sh [OPTION ...] [FILE ...]
#
#   FILE              a source file for QUIT; then the keyboard
#   -32               32-bit cells (default: 16 bit)
#   -pure             only the 32 mandatory primitives and the HAL: the
#                     words with "?:" stay Forth words
#   -core FILE        Genesis compiles FILE (default: forth/core.fs)
#   -base FILE        QUIT compiles FILE first (default: forth/base.fs)
#   -image FILE       start from an image, not from core and base
#   -dump-core FILE   write the image after core to FILE (the seed)
#   -dump FILE        write the image after base to FILE
#   -check            build with -pure, compare the images with
#                     seed/core16.hex and seed/base16.hex (32 bit: ...32),
#                     then stop; exit status 0 if they are the same
#
# SAVE-IMAGE (and so FREEZE) writes the image to ucf.hex, or to the file
# in $UCF_IMAGE. The script uses only bash builtins. Tested with bash 3.2
# (macOS) and bash 5.2.
#
# Limits:
# - ?RX waits for a complete line: bash reads lines, not single keys.
# - No checks of the stacks and of addresses, as in a kernel on a board.
# - MS waits actively. Bash 3.2 has no clock with milliseconds: there MS
#   does not wait, and MILLIS counts the whole seconds plus the MS times.
# - REBOOT starts BOOT or QUIT again, but does not load the image again.
#   ERASE-IMAGE does nothing.

LC_ALL=C
set -f

usage() {
  printf '%s\n' 'use: bash ucf.sh [-32] [-pure] [-core FILE] [-base FILE] [-image FILE]' \
                '                [-dump-core FILE] [-dump FILE] [-check] [FILE ...]' >&2
  exit 2
}

# ==== Options ===========================================================
# The default files lie in the project: two folders above this script.

here=${BASH_SOURCE[0]%/*}
[ "$here" = "${BASH_SOURCE[0]}" ] && here=.
case $here in
  */bootstrap/bash) ROOT=${here%/bootstrap/bash} ;;
  bootstrap/bash)   ROOT=. ;;
  .)                ROOT=../.. ;;
  *)                ROOT=$here/../.. ;;
esac
if [ "$ROOT" = . ]; then PFX=; else PFX=$ROOT/; fi

BITS=16 PURE=0 CHECK=0 IMAGE= DUMP= DUMP_CORE=
CORE_FILE=${PFX}forth/core.fs BASE_FILE=${PFX}forth/base.fs
while [ $# -gt 0 ]; do
  case $1 in
    -32)        BITS=32; shift ;;
    -pure)      PURE=1; shift ;;
    -check)     CHECK=1 PURE=1; shift ;;
    -core)      [ $# -ge 2 ] || usage; CORE_FILE=$2; shift 2 ;;
    -base)      [ $# -ge 2 ] || usage; BASE_FILE=$2; shift 2 ;;
    -image)     [ $# -ge 2 ] || usage; IMAGE=$2; shift 2 ;;
    -dump-core) [ $# -ge 2 ] || usage; DUMP_CORE=$2; shift 2 ;;
    -dump)      [ $# -ge 2 ] || usage; DUMP=$2; shift 2 ;;
    -*)         usage ;;
    *)          break ;;
  esac
done
if [ -n "$IMAGE" ]; then           # the first line of an image names the cell size
  [ -r "$IMAGE" ] || { printf 'ucf.sh: cannot read %s\n' "$IMAGE" >&2; exit 1; }
  IFS= read -r first < "$IMAGE"
  case $first in *bits=32*) BITS=32 ;; *bits=16*) BITS=16 ;; esac
fi
ARGS=("$@")                         # the files; "set --" below needs $@

# ==== Memory map (doc/vm.md, section 3) =================================
# M holds one cell per element: M[a >> S] is the cell at address a.
# Bytes are little endian: C@ and C! take the byte out of its cell.
# GUARD: the first word comes 8 cells above S0; a stack underflow lands
# in these free cells.

M=()
(( C = BITS / 8, S = C == 2 ? 1 : 2, A = C - 1, K = C / 2,
   MASK = (1 << BITS) - 1, SIGN = 1 << (BITS - 1),
   SYS = 0x100, DATA_START = 0x200 * K, DATA_END = K == 1 ? 0x1000 : 0x10000,
   TIB = DATA_END, TIB_SIZE = 0x100, PAD = TIB + 0x100,
   RS_BOTTOM = PAD + 0x100, R0 = RS_BOTTOM + 256 * C,
   DS_BOTTOM = R0, S0 = DS_BOTTOM + 256 * C, DICT_START = S0,
   GUARD = 8 * C,
   MEM_END = K == 1 ? 0x10000 : 0x40000, DICT_END = MEM_END - 16,
   HALT_XT = DATA_START - 16, HALT_ADDR = HALT_XT + C,
   RUN_ADDR = MEM_END - 2 * C,
   WL_FORTH = DATA_START, WL_INTERNALS = WL_FORTH + 3 * C,
   VP_START = WL_INTERNALS + 3 * C ))

# System variables: name, cells, vocabulary (F = FORTH, I = INTERNALS)
SV_NAME=() SV_VAL=() SV_VOC=()
(( a = SYS ))
set -- DP 1 I  VP 1 I  LATEST 1 I  STATE 1 I  BASE 1 F  '>IN' 1 I  '#TIB' 1 I \
       CURRENT 1 I  '#ORDER' 1 I  CONTEXT 16 I  VOC-LINK 1 I  CASE-SENSE 1 I \
       BASE-END 1 I  FROZEN-END 1 I  AUTORUN 1 I  ERRMSG 2 I  HANDLER 1 I
while [ $# -gt 0 ]; do
  SV_NAME+=("$1"); SV_VAL+=($a); SV_VOC+=("$3"); (( a += $2 * C )); shift 3
done
DP_A=${SV_VAL[0]} VP_A=${SV_VAL[1]} LATEST_A=${SV_VAL[2]} STATE_A=${SV_VAL[3]}
BASE_A=${SV_VAL[4]} TOIN_A=${SV_VAL[5]} NTIB_A=${SV_VAL[6]} CURRENT_A=${SV_VAL[7]}
NORDER_A=${SV_VAL[8]} CONTEXT_A=${SV_VAL[9]} VOCLINK_A=${SV_VAL[10]}
CASESENSE_A=${SV_VAL[11]} BASEEND_A=${SV_VAL[12]} FROZENEND_A=${SV_VAL[13]}
HANDLER_A=${SV_VAL[16]}
SV_NAME+=(TIB PAD S0 R0 DATA-END WL-FORTH WL-INTERNALS CELL DICT-END)
SV_VAL+=($TIB $PAD $S0 $R0 $DATA_END $WL_FORTH $WL_INTERNALS $C $DICT_END)
SV_VOC+=(I I I I I I I I I)

# ==== Primitives ========================================================
# Number, name, vocabulary, flags (64 = compile-only, 128 = immediate).
# 0-3 are the code field types, 4-31 the other mandatory primitives,
# 32-65 the optional ones, 80-88 the HAL, 100-108 exist only for Genesis.

NAME=() VOC=() FLAG=()
set -- 4 EXIT F 64  5 LIT I 64  6 BRANCH I 64  7 '?BRANCH' I 64 \
  8 EXECUTE F 0  9 @ F 0  10 ! F 0  11 C@ F 0  12 C! F 0  13 DUP F 0 \
  14 DROP F 0  15 SWAP F 0  16 OVER F 0  17 '>R' F 64  18 'R>' F 64 \
  19 R@ F 64  20 SP@ I 0  21 SP! I 0  22 RP@ I 0  23 RP! I 0  24 AND F 0 \
  25 OR F 0  26 XOR F 0  27 '0<' F 0  28 UM+ I 0  29 '?RX' I 0  30 TX! I 0 \
  31 !IO I 0 \
  32 + F 0  33 - F 0  34 1+ F 0  35 1- F 0  36 NEGATE F 0  37 INVERT F 0 \
  38 '2*' F 0  39 2/ F 0  40 LSHIFT F 0  41 RSHIFT F 0  42 = F 0  43 0= F 0 \
  44 '<' F 0  45 'U<' F 0  46 '*' F 0  47 'UM*' F 0  48 UM/MOD F 0 \
  49 ROT F 0  50 NIP F 0  51 '?DUP' F 0  52 2DUP F 0  53 2DROP F 0 \
  54 +! F 0  55 MOVE F 0  56 FILL F 0  57 COUNT F 0  58 FIND-NAME I 0 \
  59 '(DO)' I 64  60 '(?DO)' I 64  61 '(LOOP)' I 64  62 '(+LOOP)' I 64 \
  63 I F 64  64 J F 64  65 UNLOOP F 64 \
  80 MILLIS F 0  81 MS F 0  82 US F 0  83 BIS! F 0  84 BIC! F 0 \
  85 SAVE-IMAGE I 0  86 ERASE-IMAGE I 0  87 REBOOT I 0  88 BYE F 0 \
  100 PARSE-NAME I 0  101 PARSE I 0  102 HEADER I 0  103 REVEAL I 0 \
  104 IMMEDIATE F 0  105 , F 0  106 C, F 0  107 '[' F 128  108 ']' F 0
PRIM_OPS=
while [ $# -gt 0 ]; do
  NAME[$1]=$2 VOC[$1]=$3 FLAG[$1]=$4
  if (( $1 < 32 || $1 >= 80 || ! PURE )); then PRIM_OPS="$PRIM_OPS $1"; fi
  shift 4
done

# Each simple primitive is one arithmetic expression. SP and RP point to
# the top cell; the stacks grow down. IP is the next cell of the thread,
# W the xt that runs. Flags are MASK (true) or 0.
# The expressions have no branches (&& || ?:): bash 3.2 does an assignment
# to an array element also in the branch that it must skip, when that
# branch reads the array. So a condition gives 0 or 1 and multiplies.
P=()
P[0]='rp -= C, M[rp>>S] = ip, ip = w + C'                                  # DOCOL
P[1]='sp -= C, M[sp>>S] = w + 2*C'                                         # DOVAR
P[2]='sp -= C, M[sp>>S] = M[(w+C)>>S]'                                     # DOCON
P[3]='sp -= C, M[sp>>S] = w + 2*C, rp -= C, M[rp>>S] = ip, ip = M[(w+C)>>S]'   # DODOES
P[4]='ip = M[rp>>S], rp += C'                                              # EXIT
P[5]='sp -= C, M[sp>>S] = M[ip>>S], ip += C'                               # LIT
P[6]='ip = M[ip>>S]'                                                       # BRANCH
P[7]='pt = M[sp>>S] == 0, sp += C, ip = ip + C + pt * (M[ip>>S] - ip - C)'   # ?BRANCH
P[9]='M[sp>>S] = M[M[sp>>S]>>S]'                                           # @
P[10]='pa = M[sp>>S], M[pa>>S] = M[(sp+C)>>S], sp += 2*C'                    # !
P[11]='pa = M[sp>>S], M[sp>>S] = M[pa>>S] >> ((pa&A)<<3) & 255'               # C@
P[12]='pa = M[sp>>S], ph = (pa&A)<<3, pi = pa>>S,
       M[pi] = M[pi] & ~(255<<ph) | (M[(sp+C)>>S] & 255) << ph, sp += 2*C'     # C!
P[13]='sp -= C, M[sp>>S] = M[(sp+C)>>S]'                                   # DUP
P[14]='sp += C'                                                            # DROP
P[15]='pt = M[sp>>S], M[sp>>S] = M[(sp+C)>>S], M[(sp+C)>>S] = pt'            # SWAP
P[16]='sp -= C, M[sp>>S] = M[(sp+2*C)>>S]'                                 # OVER
P[17]='rp -= C, M[rp>>S] = M[sp>>S], sp += C'                              # >R
P[18]='sp -= C, M[sp>>S] = M[rp>>S], rp += C'                              # R>
P[19]='sp -= C, M[sp>>S] = M[rp>>S]'                                       # R@
P[20]='pt = sp, sp -= C, M[sp>>S] = pt'                                      # SP@
P[21]='sp = M[sp>>S]'                                                      # SP!
P[22]='sp -= C, M[sp>>S] = rp'                                             # RP@
P[23]='rp = M[sp>>S], sp += C'                                             # RP!
P[24]='pt = M[sp>>S], sp += C, M[sp>>S] &= pt'                               # AND
P[25]='pt = M[sp>>S], sp += C, M[sp>>S] |= pt'                               # OR
P[26]='pt = M[sp>>S], sp += C, M[sp>>S] ^= pt'                               # XOR
P[27]='M[sp>>S] = ((M[sp>>S] & SIGN) != 0) * MASK'                         # 0<
P[28]='pt = M[(sp+C)>>S] + M[sp>>S], M[(sp+C)>>S] = pt & MASK, M[sp>>S] = pt >> BITS'   # UM+
P[32]='pt = M[sp>>S], sp += C, M[sp>>S] = M[sp>>S] + pt & MASK'              # +
P[33]='pt = M[sp>>S], sp += C, M[sp>>S] = M[sp>>S] - pt & MASK'              # -
P[34]='M[sp>>S] = M[sp>>S] + 1 & MASK'                                     # 1+
P[35]='M[sp>>S] = M[sp>>S] - 1 & MASK'                                     # 1-
P[36]='M[sp>>S] = -M[sp>>S] & MASK'                                        # NEGATE
P[37]='M[sp>>S] ^= MASK'                                                   # INVERT
P[38]='M[sp>>S] = M[sp>>S] << 1 & MASK'                                    # 2*
P[39]='pt = M[sp>>S], M[sp>>S] = pt >> 1 | pt & SIGN'                         # 2/
P[40]='pt = M[sp>>S], sp += C, M[sp>>S] = (pt < BITS) * (M[sp>>S] << (pt & 31) & MASK)'   # LSHIFT
P[41]='pt = M[sp>>S], sp += C, M[sp>>S] = (pt < BITS) * (M[sp>>S] >> (pt & 31))'   # RSHIFT
P[42]='pt = M[sp>>S], sp += C, M[sp>>S] = (M[sp>>S] == pt) * MASK'          # =
P[43]='M[sp>>S] = (M[sp>>S] == 0) * MASK'                                  # 0=
P[44]='pt = M[sp>>S] ^ SIGN, sp += C, M[sp>>S] = ((M[sp>>S] ^ SIGN) < pt) * MASK'   # <
P[45]='pt = M[sp>>S], sp += C, M[sp>>S] = (M[sp>>S] < pt) * MASK'           # U<
P[46]='pt = M[sp>>S], sp += C, M[sp>>S] = M[sp>>S] * pt & MASK'              # *
P[47]='pt = M[(sp+C)>>S] * M[sp>>S], M[(sp+C)>>S] = pt & MASK, M[sp>>S] = pt >> BITS & MASK'   # UM*
P[49]='pt = M[(sp+2*C)>>S], M[(sp+2*C)>>S] = M[(sp+C)>>S],
       M[(sp+C)>>S] = M[sp>>S], M[sp>>S] = pt'                              # ROT
P[50]='pt = M[sp>>S], sp += C, M[sp>>S] = pt'                                # NIP
P[51]='pt = (M[sp>>S] != 0) * C, sp -= pt, M[sp>>S] = M[(sp+pt)>>S]'       # ?DUP
P[52]='sp -= 2*C, M[sp>>S] = M[(sp+2*C)>>S], M[(sp+C)>>S] = M[(sp+3*C)>>S]'   # 2DUP
P[53]='sp += 2*C'                                                          # 2DROP
P[54]='pa = M[sp>>S], M[pa>>S] = M[pa>>S] + M[(sp+C)>>S] & MASK, sp += 2*C'   # +!
P[57]='pa = M[sp>>S], M[sp>>S] = pa + 1 & MASK, sp -= C,
       M[sp>>S] = M[pa>>S] >> ((pa&A)<<3) & 255'                             # COUNT
P[59]='rp -= 3*C, M[(rp+2*C)>>S] = M[ip>>S], M[(rp+C)>>S] = M[(sp+C)>>S],
       M[rp>>S] = M[sp>>S], sp += 2*C, ip += C'                            # (DO)
P[60]='pf = M[sp>>S] == M[(sp+C)>>S], M[(rp-C)>>S] = M[ip>>S],
       M[(rp-2*C)>>S] = M[(sp+C)>>S], M[(rp-3*C)>>S] = M[sp>>S],
       rp -= (1 - pf) * 3*C, ip = ip + C + pf * (M[ip>>S] - ip - C), sp += 2*C'   # (?DO)
P[61]='pt = M[rp>>S] + 1 & MASK, pf = pt == M[(rp+C)>>S], M[rp>>S] = pt,
       rp += pf * 3*C, ip = ip + C + (1 - pf) * (M[ip>>S] - ip - C)'        # (LOOP)
P[62]='pn = M[sp>>S], sp += C, pn = (pn ^ SIGN) - SIGN,
       px = ((M[rp>>S] - M[(rp+C)>>S] & MASK) ^ SIGN) - SIGN,
       py = ((px + pn & MASK) ^ SIGN) - SIGN,
       pf = ((px ^ py) & (px ^ pn)) < 0, M[rp>>S] = M[rp>>S] + pn & MASK,
       rp += pf * 3*C, ip = ip + C + (1 - pf) * (M[ip>>S] - ip - C)'        # (+LOOP)
P[63]='sp -= C, M[sp>>S] = M[rp>>S]'                                       # I
P[64]='sp -= C, M[sp>>S] = M[(rp+3*C)>>S]'                                 # J
P[65]='rp += 3*C'                                                          # UNLOOP
P[83]='pa = M[sp>>S], pt = M[(sp+C)>>S] & 255, sp += 2*C,
       M[pa>>S] |= pt << ((pa&A)<<3)'                                         # BIS!
P[84]='pa = M[sp>>S], pt = M[(sp+C)>>S] & 255, sp += 2*C,
       M[pa>>S] &= ~(pt << ((pa&A)<<3))'                                      # BIC!
P[103]='pt = M[LATEST_A>>S], pa = M[CURRENT_A>>S],
        M[pa>>S] += (pt != 0) * (pt - M[pa>>S])'                            # REVEAL
P[104]='M[(M[LATEST_A>>S]+C)>>S] |= 128'                                   # IMMEDIATE
P[105]='pt = M[DP_A>>S], M[pt>>S] = M[sp>>S], sp += C, M[DP_A>>S] = pt + C'   # ,
P[106]='pt = M[DP_A>>S], ph = (pt&A)<<3, pi = pt>>S,
        M[pi] = M[pi] & ~(255<<ph) | (M[sp>>S] & 255) << ph, sp += C, M[DP_A>>S] = pt + 1'   # C,
P[107]='M[STATE_A>>S] = 0'                                                 # [
P[108]='M[STATE_A>>S] = MASK'                                              # ]

# The other primitives need bash code: SPEC marks them.
SPEC=()
for (( op = 0; op < 128; op++ )); do [ -n "${P[op]}" ] || SPEC[op]=1; done

# HAVE marks the primitives that this VM has; an image may use
# only these. PRIMS lists the primitives with a header (for the image).
HAVE=()
for (( op = 0; op < 128; op++ )); do [ -n "${P[op]}" ] && HAVE[op]=1; done
for op in 8 29 30 31 48 55 56 58 80 81 82 85 86 87 88 89 90 91 92 93 98 99; do HAVE[op]=1; done
PRIMS= prev=-2 first=0
for op in 0 1 2 3 $PRIM_OPS; do     # the list as ranges, as "0-31,80-88"
  (( op >= 100 )) && continue
  if (( op != prev + 1 )); then
    (( prev >= 0 )) && { (( first == prev )) && PRIMS="$PRIMS$first," || PRIMS="$PRIMS$first-$prev,"; }
    first=$op
  fi
  prev=$op
done
(( first == prev )) && PRIMS="$PRIMS$first" || PRIMS="$PRIMS$first-$prev"

# ==== Helpers ===========================================================

CH=()                               # CH[c]: the character with the code c
for (( c = 1; c < 256; c++ )); do printf -v o '%03o' $c; printf -v t "\\$o"; CH[c]=$t; done

ord() { printf -v R '%d' "'$1"; (( R &= 255 )); }

# fline FD: the next line of a file in LINE (doc/genesis.md, section 2):
# the text after the last LF is one more line, also when it is empty.
# FDONE[FD] is 1 after that line; the status is then 1.
FDONE=()
fline() {
  (( FDONE[$1] )) && return 1
  IFS= read -r LINE <&$1 && return 0
  FDONE[$1]=1
  return 0
}
getb() { (( R = M[$1 >> S] >> (($1 & A) << 3) & 255 )); }
setb() { (( i_ = $1 >> S, h_ = ($1 & A) << 3, M[i_] = M[i_] & ~(255 << h_) | ($2 & 255) << h_ )); }
push() { (( sp -= C, M[sp>>S] = $1 & MASK )); }
ntxt() { (( R = ($1 + C + 1 + (M[($1 + C) >> S] & 31) + A) & ~A )); }   # nt -> xt

VMS=0                               # bash 3.2: the time that MS waited (virtual)
now_ms() {                          # R = milliseconds
  if [ -n "$EPOCHREALTIME" ]; then  # bash 5
    local s=${EPOCHREALTIME%.*} f=${EPOCHREALTIME#*.}
    (( R = s * 1000 + 10#${f:0:3} ))
  else                              # bash 3.2 has only whole seconds
    (( R = SECONDS * 1000 + VMS ))
  fi
}
now_ms; T0=$R

OB=                                 # the output line that is not complete
ERRSEEN=0                           # a line of output reported an error

msg() {                             # a message of this script on its own line
  [ -n "$OB" ] && { printf '%s\n' "$OB"; OB=; }
  printf '  %s\n' "$1"
}

bye() {
  [ -n "$OB" ] && printf '%s\n' "$OB"
  exit "${1:-0}"
}

THROW_TEXT() {
  case $1 in
    -3) R='data stack overflow' ;;   -4) R='data stack underflow' ;;
    -8) R='dictionary overflow' ;;   -10) R='division by zero' ;;
    -11) R='result out of range' ;;  -13) R='undefined word' ;;
    -14) R='compile-only word' ;;    -16) R='missing name' ;;
    -19) R='name too long' ;;        -21) R='unsupported operation' ;;
    -24) R='number out of range' ;;  *) R="error $1" ;;
  esac
}

# ==== Search in the dictionary ==========================================
# KEY[0..KN-1] holds the name; without CASE-SENSE in upper case.

wlfind() {                          # $1: wordlist. R = nt or 0
  local nt j a b cs
  (( nt = M[$1 >> S], cs = M[CASESENSE_A >> S] ))
  while (( nt )); do
    if (( (M[(nt + C) >> S] & 31) == KN )); then
      (( a = nt + C + 1 ))
      for (( j = 0; j < KN; j++ )); do
        (( b = M[(a + j) >> S] >> (((a + j) & A) << 3) & 255,
           !cs && b >= 97 && b <= 122 && (b -= 32), b == KEY[j] )) || break
      done
      (( j == KN )) && { R=$nt; return; }
    fi
    (( nt = M[nt >> S] ))
  done
  R=0
}

gfind() {                           # the search order, the top first. R = nt or 0
  local i
  if (( KN == 0 || KN > 31 )); then R=0; return; fi
  for (( i = M[NORDER_A >> S] - 1; i >= 0; i-- )); do
    wlfind $(( M[(CONTEXT_A + i * C) >> S] ))
    (( R )) && return
  done
  R=0
}

keystr() {                          # KEY from the string $1
  local j cs
  (( KN = ${#1}, cs = M[CASESENSE_A >> S] ))
  for (( j = 0; j < KN; j++ )); do
    ord "${1:j:1}"; (( !cs && R >= 97 && R <= 122 && (R -= 32), KEY[j] = R ))
  done
}

keymem() {                          # KEY from memory: $1 address, $2 length
  local j b cs
  (( KN = $2, cs = M[CASESENSE_A >> S] ))
  for (( j = 0; j < KN; j++ )); do
    (( b = M[($1 + j) >> S] >> ((($1 + j) & A) << 3) & 255,
       !cs && b >= 97 && b <= 122 && (b -= 32), KEY[j] = b ))
  done
}

sysxt() {                           # the oldest word with exactly this name
  local wl nt found j a n=${#1}
  for wl in $WL_FORTH $WL_INTERNALS; do
    (( found = 0, nt = M[wl >> S] ))
    while (( nt )); do
      if (( (M[(nt + C) >> S] & 31) == n )); then
        (( a = nt + C + 1 ))
        for (( j = 0; j < n; j++ )); do
          ord "${1:j:1}"; c_=$R; getb $(( a + j )); (( R == c_ )) || break
        done
        (( j == n )) && found=$nt
      fi
      (( nt = M[nt >> S] ))
    done
    (( found )) && { ntxt $found; return; }
  done
  R=0
}

# ==== The virtual machine ===============================================

XT_THROW=0 XT_QUIT=0 XT_BOOT=0

throw() {                           # THROW n in the image: W and OP get THROW
  if (( ! XT_THROW )); then THROW_TEXT $1; gfail "$R (no THROW yet)"; fi
  (( sp -= C, M[sp >> S] = $1 & MASK, w = XT_THROW, op = M[w >> S] ))
}

run_xt() {                          # execute xt $1; return when it is complete
  (( M[RUN_ADDR >> S] = $1, M[(RUN_ADDR + C) >> S] = HALT_XT, ip = RUN_ADDR ))
  vm
}

vm() {                              # NEXT and the primitives, until HALT
  while :; do
    if (( w = M[ip>>S], ip += C, op = M[w>>S], op > 127 || SPEC[op] )); then
      while :; do                   # the primitives with bash code
        case $op in
          8)   (( w = M[sp>>S], sp += C, op = M[w>>S] )) ;;      # EXECUTE
          29)  rx; break ;;                                      # ?RX
          30)  tx; break ;;                                      # TX!
          31)  break ;;                                          # !IO
          48)  ummod && break ;;                                 # UM/MOD
          55)  move_; break ;;                                   # MOVE
          56)  fill_; break ;;                                   # FILL
          58)  findname; break ;;                                # FIND-NAME
          80)  now_ms; push $(( R - T0 )); break ;;              # MILLIS
          81)  ms_; break ;;                                     # MS
          82)  (( sp += C )); break ;;                           # US
          85)  save_image; break ;;                              # SAVE-IMAGE
          86)  break ;;                                          # ERASE-IMAGE
          87)  reboot ;;                                         # REBOOT
          88)  bye 0 ;;                                          # BYE
          89)  fopen 0; break ;;                                 # OPEN-FILE
          90)  fopen 1; break ;;                                 # CREATE-FILE
          91)  fclose; break ;;                                  # CLOSE-FILE
          92)  fread; break ;;                                   # READ-FILE
          93)  fwrite; break ;;                                  # WRITE-FILE
          98)  freadline; break ;;                               # READ-LINE
          99)  (( R = M[sp>>S] & 255, sp += C )); bye $R ;;      # (BYE)
          100) pname; push $PA; push $PN; break ;;               # PARSE-NAME
          101) (( t = M[sp>>S] & 255, sp += C )); pparse $t; push $PA; push $PN; break ;;
          102) gheader; break ;;                                 # HEADER
          127) return ;;                                         # HALT: back to bash
          *)   throw -21 ;;
        esac
        (( op <= 127 && SPEC[op] )) && continue
        (( op > 127 )) && { throw -21; continue; }
        (( ${P[op]} )); break
      done
    else
      (( ${P[op]} ))
    fi
  done
}

ummod() {                           # ( lo hi u -- rem quot ). Status 1: THROW
  local u hi lo q r n j
  (( u = M[sp>>S], hi = M[(sp+C)>>S], lo = M[(sp+2*C)>>S], sp += C ))
  if (( u == 0 )); then throw -10; return 1; fi
  if (( hi >= u )); then throw -11; return 1; fi
  if (( BITS == 16 || hi < 0x80000000 )); then
    (( n = hi << BITS | lo, q = n / u, r = n % u ))
  else                              # the 64-bit number does not fit bash
    (( q = 0, r = hi ))
    for (( j = BITS - 1; j >= 0; j-- )); do
      (( r = r << 1 | (lo >> j & 1), q <<= 1, r >= u && (r -= u, q |= 1) ))
    done
  fi
  (( M[(sp+C)>>S] = r, M[sp>>S] = q ))
  return 0
}

move_() {                           # ( a1 a2 u -- ), also for areas that overlap
  local a1 a2 u j x y b i h
  (( u = M[sp>>S], a2 = M[(sp+C)>>S], a1 = M[(sp+2*C)>>S], sp += 3*C ))
  if (( a1 < a2 )); then
    for (( j = u - 1; j >= 0; j-- )); do
      (( x = a1 + j, y = a2 + j, b = M[x>>S] >> ((x&A)<<3) & 255,
         i = y>>S, h = (y&A)<<3, M[i] = M[i] & ~(255<<h) | b << h ))
    done
  else
    for (( j = 0; j < u; j++ )); do
      (( x = a1 + j, y = a2 + j, b = M[x>>S] >> ((x&A)<<3) & 255,
         i = y>>S, h = (y&A)<<3, M[i] = M[i] & ~(255<<h) | b << h ))
    done
  fi
}

fill_() {                           # ( a u c -- )
  local a u c j y i h
  (( c = M[sp>>S] & 255, u = M[(sp+C)>>S], a = M[(sp+2*C)>>S], sp += 3*C ))
  for (( j = 0; j < u; j++ )); do
    (( y = a + j, i = y>>S, h = (y&A)<<3, M[i] = M[i] & ~(255<<h) | c << h ))
  done
}

findname() {                        # ( a u -- nt | 0 )
  local a u
  (( u = M[sp>>S], a = M[(sp+C)>>S], sp += C ))
  if (( u == 0 || u > 31 )); then R=0; else keymem $a $u; gfind; fi
  (( M[sp>>S] = R ))
}

ms_() {                             # ( u -- ): wait u milliseconds
  local end
  if [ -z "$EPOCHREALTIME" ]; then (( VMS += M[sp>>S], sp += C )); return; fi
  now_ms; (( end = R + M[sp>>S], sp += C ))
  while now_ms; (( R < end )); do :; done
}

reboot() {                          # ( f -- ): stacks empty, then BOOT or QUIT
  local f
  (( f = M[sp>>S], sp = S0, rp = R0, M[STATE_A>>S] = 0, M[HANDLER_A>>S] = 0,
     ip = RUN_ADDR + C, w = f && XT_BOOT ? XT_BOOT : XT_QUIT, op = M[w>>S] ))
}

# ==== Terminal: input lines for ?RX, output for TX! =====================
# The input comes from the files of the command line, then from the
# keyboard or a pipe. A file line hides the prompt of QUIT.

FILES=()                            # without -image: FILES[0] = core, FILES[1] = base
FI=0                                # the next file to open
CURFILE= CURLN=0                    # the file that loads, its line number
IN=() IN_LEN=0 IN_POS=0             # the characters of the current line
BUILD_END=-1                        # after this file the system is complete

setline() {                         # the codes of the line $1, then LF
  local j n=${#1}
  for (( j = 0; j < n; j++ )); do
    printf -v c '%d' "'${1:j:1}"; (( IN[j] = c & 255 ))
  done
  (( IN[n] = 10, IN_LEN = n + 1, IN_POS = 0 ))
}

getline() {                         # the next line for ?RX
  local line
  while :; do
    if [ -n "$CURFILE" ] && (( ERRSEEN )); then
      msg "$CURFILE:$CURLN: loading stopped"
      exec 4<&-; CURFILE=
      (( FI <= BUILD_END + 1 && BUILD_END >= 0 )) && { FI=$(( BUILD_END + 1 )); build_end; }
      FI=${#FILES[@]}
    fi
    ERRSEEN=0
    if [ -n "$CURFILE" ]; then
      if fline 4; then
        (( CURLN++ )); OB=; setline "${LINE%$'\r'}"; return
      fi
      exec 4<&-; CURFILE=
      (( FI == BUILD_END + 1 )) && build_end
      continue
    fi
    if (( FI < ${#FILES[@]} )); then
      CURFILE=${FILES[FI]}; CURLN=0; FDONE[4]=0; (( FI++ ))
      if ! exec 4<"$CURFILE"; then
        msg "cannot read $CURFILE"; CURFILE=; FI=${#FILES[@]}
      fi
      continue
    fi
    if [ -t 0 ]; then                # the keyboard: the terminal shows the line
      printf '%s' "$OB"; OB=
      IFS= read -r line || { printf '\n'; bye 0; }
    else                            # a pipe: show the line after the prompt
      IFS= read -r line || [ -n "$line" ] || bye 0
      printf '%s%s\n' "$OB" "$line"; OB=
    fi
    setline "${line%$'\r'}"; return
  done
}

XT_REFILL=0 REFILL_END=0            # the body of REFILL: QUIT waits for a line there

refilling() {                       # status 0 if a return address points into REFILL
  local r
  for (( r = rp; r < rp + 8 * C && r < R0; r += C )); do
    (( M[r>>S] > XT_REFILL && M[r>>S] < REFILL_END )) && return 0
  done
  return 1
}

rx() {                              # ?RX ( -- c true | 0 )
  if (( IN_POS >= IN_LEN )); then
    if ! refilling && { [ -n "$CURFILE" ] || (( FI < ${#FILES[@]} )) || [ ! -t 0 ]; }; then
      push 0; return                # a word reads keys while files load: no key (doc/vm.md 9)
    fi
    getline                         # QUIT waits in REFILL, or a word waits at the keyboard
  fi
  (( sp -= 2*C, M[(sp+C)>>S] = IN[IN_POS], M[sp>>S] = MASK, IN_POS++ ))
}

bodyend() {                         # $1: xt. R = the next header in memory after it
  local wl nt best=$MEM_END
  (( wl = M[VOCLINK_A>>S] ))
  while (( wl )); do
    (( nt = M[wl>>S] ))
    while (( nt )); do (( nt > $1 && nt < best && (best = nt), nt = M[nt>>S] )); done
    (( wl = M[(wl + C)>>S] ))
  done
  R=$best
}

find_refill() { sysxt REFILL; XT_REFILL=$R; bodyend $XT_REFILL; REFILL_END=$R; }

tx() {                              # TX! ( c -- )
  local c
  (( c = M[sp>>S] & 255, sp += C ))
  if (( c == 10 )); then
    printf '%s\n' "$OB"
    case $OB in *error:*) ERRSEEN=1 ;; esac
    OB=
  elif (( c )); then
    OB+=${CH[c]}
  fi
}

# ==== Files: HAL 89-93, 98 (doc/vm.md, section 8) ======================
# A file id (fid) is a file descriptor of bash, 10 to 29. FLINE holds the
# rest of the current line of a file; FST is 0 (no line), 1 (a line that
# ends with LF) or 2 (the last line, without LF). Text files only: bash
# cannot hold the byte 0 in a string.

FOPEN=() FLINE=() FST=()

memstr() {                          # $1 address, $2 length: R = these bytes as text
  local j b t=
  for (( j = 0; j < $2; j++ )); do
    (( b = M[($1 + j) >> S] >> ((($1 + j) & A) << 3) & 255 ))
    t+=${CH[b]}
  done
  R=$t
}

fret2() { (( M[(sp+C)>>S] = $1 & MASK, M[sp>>S] = $2 & MASK )); }      # the top two cells
fret3() { (( M[(sp+2*C)>>S] = $1 & MASK, M[(sp+C)>>S] = $2 & MASK, M[sp>>S] = $3 & MASK )); }
fid_ok() { (( $1 >= 10 && $1 < 30 && FOPEN[$1] )); }

fopen() {                           # OPEN-FILE / CREATE-FILE ( a u fam -- fid ior ); $1 = 1: create
  local fam u a fd name
  (( fam = M[sp>>S] & 3, u = M[(sp+C)>>S], a = M[(sp+2*C)>>S], sp += C ))
  memstr $a $u; name=$R
  for (( fd = 10; fd < 30; fd++ )); do (( FOPEN[fd] )) || break; done
  if (( fd == 30 )); then fret2 0 -37; return; fi
  if (( $1 )); then
    { rm -f "$name" && : > "$name"; } 2>/dev/null || { fret2 0 -37; return; }   # a new file (macOS: signature)
  elif [ ! -e "$name" ]; then
    fret2 0 -38; return
  fi
  # Test first, then exec without any other redirection: bash keeps its own
  # copies of file descriptors from 10 on while a redirection runs.
  if [ -d "$name" ] || [ ! -r "$name" ] || { (( fam )) && [ ! -w "$name" ]; }; then
    fret2 0 -37; return
  fi
  if (( fam == 0 )); then eval "exec $fd<\"\$name\""; else eval "exec $fd<>\"\$name\""; fi
  FOPEN[fd]=1 FLINE[fd]= FST[fd]=0
  fret2 $fd 0
}

fclose() {                          # CLOSE-FILE ( fid -- ior )
  local fd
  (( fd = M[sp>>S] ))
  if fid_ok $fd; then
    eval "exec $fd<&-"; FOPEN[fd]=0
    (( M[sp>>S] = 0 ))
  else
    (( M[sp>>S] = -37 & MASK ))
  fi
}

fload() {                           # $1 fd: the next line into FLINE; status 1 at the end
  local line
  if IFS= read -r line <&$1; then FLINE[$1]=$line; FST[$1]=1; return 0; fi
  if [ -n "$line" ]; then FLINE[$1]=$line; FST[$1]=2; return 0; fi
  return 1
}

freadline() {                       # READ-LINE ( a u fid -- u2 f ior )
  local a u fd line n j c x i h
  (( fd = M[sp>>S], u = M[(sp+C)>>S], a = M[(sp+2*C)>>S] ))
  fid_ok $fd || { fret3 0 0 -37; return; }
  if (( ! FST[fd] )); then
    fload $fd || { fret3 0 0 0; return; }           # the end of the file
    FLINE[fd]=${FLINE[fd]%$'\r'}
  fi
  line=${FLINE[fd]}; n=${#line}
  (( n > u )) && n=$u
  for (( j = 0; j < n; j++ )); do
    printf -v c '%d' "'${line:j:1}"
    (( x = a + j, i = x >> S, h = (x & A) << 3, M[i] = M[i] & ~(255 << h) | (c & 255) << h ))
  done
  if (( n == u )); then             # the buffer is full: the rest of the line comes next
    FLINE[fd]=${line:n}
  else
    FLINE[fd]= FST[fd]=0
  fi
  fret3 $n $MASK 0
}

fread() {                           # READ-FILE ( a u fid -- u2 ior )
  local a u fd n=0 line k j c x i h
  (( fd = M[sp>>S], u = M[(sp+C)>>S], a = M[(sp+2*C)>>S], sp += C ))
  fid_ok $fd || { fret2 0 -37; return; }
  while (( n < u )); do
    if (( ! FST[fd] )); then fload $fd || break; fi
    line=${FLINE[fd]}
    if [ -z "$line" ]; then         # the text of the line is out: now its LF
      if (( FST[fd] == 1 )); then
        (( x = a + n, i = x >> S, h = (x & A) << 3, M[i] = M[i] & ~(255 << h) | 10 << h, n++ ))
      fi
      FST[fd]=0; continue
    fi
    (( k = ${#line}, k > u - n && (k = u - n) ))
    for (( j = 0; j < k; j++ )); do
      printf -v c '%d' "'${line:j:1}"
      (( x = a + n + j, i = x >> S, h = (x & A) << 3, M[i] = M[i] & ~(255 << h) | (c & 255) << h ))
    done
    FLINE[fd]=${line:k}; (( n += k ))
  done
  fret2 $n 0
}

fwrite() {                          # WRITE-FILE ( a u fid -- ior )
  local a u fd j b o fmt=
  (( fd = M[sp>>S], u = M[(sp+C)>>S], a = M[(sp+2*C)>>S], sp += 2*C ))
  if ! fid_ok $fd; then (( M[sp>>S] = -37 & MASK )); return; fi
  for (( j = 0; j < u; j++ )); do   # each byte as an octal escape: also % and \ stay
    (( b = M[(a + j) >> S] >> (((a + j) & A) << 3) & 255 ))
    printf -v o '\\%03o' $b; fmt+=$o
  done
  if printf "$fmt" >&$fd 2>/dev/null; then (( M[sp>>S] = 0 )); else (( M[sp>>S] = -37 & MASK )); fi
}

# ==== Genesis: bash compiles core.fs (doc/genesis.md) ====================
# The outer interpreter: find the word, then execute or compile it; else
# convert a number. PARSE-NAME, PARSE, HEADER, REVEAL, IMMEDIATE, ",",
# "C,", "[" and "]" are host primitives until core.fs defines Forth
# versions. Genesis handles "?:", "\" and "(" itself until core.fs has them.

GNT=' '                             # the headers that Genesis removes at the end
GLN=0 GSKIP=0 GSKIPF=0 XT_CATCH=0 XT_LIT=0 BOOT_COLON=0 BOOT_SEMI=0
L=()                                # the codes of the line in TIB

gfail() {
  msg "error: $1"
  msg "${FILES[0]}:$GLN: loading stopped"
  msg "${FILES[0]} is not complete; the system cannot start"
  bye 1
}
gerr() {                            # a THROW code during Genesis
  THROW_TEXT $1
  if [ -n "$2" ]; then gfail "$R: $2"; else gfail "$R"; fi
}

pname() {                           # PARSE-NAME on TIB: PA PN; >IN moves
  local i n st
  (( i = M[TOIN_A>>S], n = M[NTIB_A>>S] ))
  while (( i < n && L[i] <= 32 )); do (( i++ )); done
  (( st = i ))
  while (( i < n && L[i] > 32 )); do (( i++ )); done
  (( PN = i - st, PA = TIB + st, i < n && i++, M[TOIN_A>>S] = i ))
}

pparse() {                          # PARSE up to the character $1: PA PN
  local i n st
  (( i = M[TOIN_A>>S], n = M[NTIB_A>>S], st = i ))
  while (( i < n && L[i] != $1 )); do (( i++ )); done
  (( PN = i - st, PA = TIB + st, i < n && i++, M[TOIN_A>>S] = i ))
}

comma() { (( t_ = M[DP_A>>S], M[t_>>S] = $1 & MASK, M[DP_A>>S] = t_ + C )); }

mkhead() {                          # a visible header: $1 name, $2 wordlist, $3 flags
  local name=$1 n=${#1} nt j
  (( nt = M[DP_A>>S], M[nt>>S] = M[$2>>S] ))
  setb $(( nt + C )) $(( n | $3 ))
  for (( j = 0; j < n; j++ )); do ord "${name:j:1}"; setb $(( nt + C + 1 + j )) $R; done
  (( R = (nt + C + 1 + n + A) & ~A, M[$2>>S] = nt, M[LATEST_A>>S] = nt, M[DP_A>>S] = R ))
}

gheader() {                         # HEADER ( a u -- ): a new header in CURRENT, not visible
  local a n nt xt j name=
  (( n = M[sp>>S], a = M[(sp+C)>>S], sp += 2*C ))
  (( n == 0 )) && gerr -16
  (( n > 31 )) && gerr -19
  for (( j = 0; j < n; j++ )); do getb $(( a + j )); name+=${CH[R]}; done
  keymem $a $n; wlfind $(( M[CURRENT_A>>S] ))
  if (( R )) && [[ $GNT != *" $R "* ]]; then msg "redefined $name"; fi
  while (( M[DP_A>>S] & A )); do setb $(( M[DP_A>>S] )) 0; (( M[DP_A>>S]++ )); done
  (( nt = M[DP_A>>S], xt = (nt + C + 1 + n + A) & ~A ))
  (( xt + 4 * C > DICT_END )) && gerr -8
  (( M[nt>>S] = M[M[CURRENT_A>>S]>>S] ))
  setb $(( nt + C )) $n
  for (( j = 0; j < n; j++ )); do getb $(( a + j )); setb $(( nt + C + 1 + j )) $R; done
  for (( j = nt + C + 1 + n; j < xt; j++ )); do setb $j 0; done
  (( M[DP_A>>S] = xt, M[LATEST_A>>S] = nt ))
  if [ "$name" = ':' ] && (( ! BOOT_COLON )); then BOOT_COLON=$nt; GNT="$GNT$nt "; fi
  if [ "$name" = ';' ] && (( ! BOOT_SEMI )); then BOOT_SEMI=$nt; GNT="$GNT$nt "; fi
}

gnumber() {                         # $1: a word. R = the number; status 1: no number
  local s=$1 base neg=0 v=0 d c j over=0
  if (( ${#s} == 3 )) && [ "${s:0:1}" = "'" ] && [ "${s:2:1}" = "'" ]; then
    ord "${s:1:1}"; return 0
  fi
  (( base = M[BASE_A>>S] ))
  case $s in -[\$#%]*) neg=1; s=${s:1} ;; esac
  case $s in
    \$*) base=16; s=${s:1} ;;
    \#*) base=10; s=${s:1} ;;
    %*)  base=2;  s=${s:1} ;;
  esac
  case $s in -*) (( neg = !neg )); s=${s:1} ;; esac
  [ -n "$s" ] && (( base >= 2 && base <= 36 )) || return 1
  for (( j = 0; j < ${#s}; j++ )); do
    ord "${s:j:1}"; c=$R
    if (( c >= 48 && c <= 57 )); then (( d = c - 48 ))
    elif (( c >= 65 && c <= 90 )); then (( d = c - 55 ))
    elif (( c >= 97 && c <= 122 )); then (( d = c - 87 ))
    else return 1; fi
    (( d < base )) || return 1
    (( over )) || (( v = v * base + d, v > MASK && (over = 1) ))
  done
  (( over )) && gerr -24 "$1"
  (( neg )) && (( v = -v ))
  (( v < -SIGN || v > MASK )) && gerr -24 "$1"
  (( R = v & MASK ))
  return 0
}

gexec() {                           # execute xt $1; through CATCH once CORE has it
  local code
  if (( XT_CATCH )); then
    push $1; run_xt $XT_CATCH
    (( code = M[sp>>S], sp += C ))
    (( code )) && gerr $(( (code ^ SIGN) - SIGN ))
  else
    run_xt $1
  fi
}

gline() {                           # interpret one line of CORE
  local line=$1 n=${#1} j c x i h st word nt xt fl save
  (( n > TIB_SIZE )) && gfail "line too long ($n bytes, at most $TIB_SIZE)"
  for (( j = 0; j < n; j++ )); do
    printf -v c '%d' "'${line:j:1}"
    (( c &= 255, L[j] = c, x = TIB + j, i = x >> S, h = (x & A) << 3,
       M[i] = M[i] & ~(255 << h) | c << h ))
  done
  (( M[NTIB_A>>S] = n, M[TOIN_A>>S] = 0 ))
  while :; do
    pname
    (( PN )) || return 0
    (( st = PA - TIB ))
    word=${line:st:PN}
    if (( GSKIP )); then            # "?:" skips a definition
      case $word in
        '\') (( M[TOIN_A>>S] = M[NTIB_A>>S] )) ;;
        '(') pparse 41 ;;
        ';') GSKIP=0 GSKIPF=1 ;;
      esac
      continue
    fi
    if (( GSKIPF )); then           # and the flags after it
      shopt -s nocasematch
      case $word in
        immediate|unchecked|compile-only) shopt -u nocasematch; continue ;;
      esac
      shopt -u nocasematch
      GSKIPF=0
    fi
    if [ "$word" = '?:' ]; then
      (( save = M[TOIN_A>>S] ))
      pname
      (( PN )) || gerr -16
      keymem $PA $PN; gfind
      if (( R )); then GSKIP=1; continue; fi
      (( M[TOIN_A>>S] = save ))
      keystr ':'; gfind; ntxt $R; gexec $R
      continue
    fi
    keymem $PA $PN; gfind
    if (( R )); then
      nt=$R; ntxt $nt; xt=$R; getb $(( nt + C )); fl=$R
      if (( M[STATE_A>>S] == 0 || fl & 128 )); then
        (( M[STATE_A>>S] == 0 && fl & 64 )) && gerr -14 "$word"
        gexec $xt
      else
        comma $xt
      fi
      continue
    fi
    case $word in                   # comments until CORE defines them
      '\') (( M[TOIN_A>>S] = M[NTIB_A>>S] )); continue ;;
      '(') pparse 41; continue ;;
    esac
    gnumber "$word" || gerr -13 "$word"
    if (( M[STATE_A>>S] )); then comma $XT_LIT; comma $R; else push $R; fi
  done
}

genesis() {                         # compile CORE; then remove the host primitives
  local line op wl nt j
  (( M[HALT_XT>>S] = 127, M[HALT_ADDR>>S] = HALT_XT,
     M[DP_A>>S] = DICT_START + GUARD, M[VP_A>>S] = VP_START, M[BASE_A>>S] = 10,
     M[(WL_FORTH + C)>>S] = 0, M[(WL_INTERNALS + C)>>S] = WL_FORTH,
     M[VOCLINK_A>>S] = WL_INTERNALS, sp = S0, rp = R0 ))
  for op in $PRIM_OPS; do           # a header for each primitive
    if [ "${VOC[op]}" = F ]; then wl=$WL_FORTH; else wl=$WL_INTERNALS; fi
    mkhead "${NAME[op]}" $wl ${FLAG[op]}
    (( M[R>>S] = op, M[DP_A>>S] = R + C ))
    (( op == 5 )) && XT_LIT=$R
    (( op >= 100 )) && GNT="$GNT$(( M[LATEST_A>>S] )) "
  done
  for (( j = 0; j < ${#SV_NAME[@]}; j++ )); do   # constants for the system variables
    if [ "${SV_VOC[j]}" = F ]; then wl=$WL_FORTH; else wl=$WL_INTERNALS; fi
    mkhead "${SV_NAME[j]}" $wl 0
    (( M[R>>S] = 2, M[(R+C)>>S] = SV_VAL[j], M[DP_A>>S] = R + 2*C ))
  done
  (( M[NORDER_A>>S] = 2, M[CONTEXT_A>>S] = WL_FORTH,
     M[(CONTEXT_A + C)>>S] = WL_INTERNALS, M[CURRENT_A>>S] = WL_FORTH ))
  exec 3<"${FILES[0]}" || { msg "cannot read ${FILES[0]}"; bye 1; }
  FDONE[3]=0
  while fline 3; do
    (( GLN++ ))
    gline "${LINE%$'\r'}"
    if (( ! XT_CATCH )); then keystr CATCH; gfind; (( R )) && { ntxt $R; XT_CATCH=$R; }; fi
  done
  exec 3<&-
  wl=$(( M[VOCLINK_A>>S] ))        # remove the host primitives and the first ":" ";"
  while (( wl )); do
    local prev=0 next
    (( nt = M[wl>>S] ))
    while (( nt )); do
      (( next = M[nt>>S] ))
      if [[ $GNT == *" $nt "* ]]; then
        if (( prev )); then (( M[prev>>S] = next )); else (( M[wl>>S] = next )); fi
      else
        prev=$nt
      fi
      nt=$next
    done
    (( wl = M[(wl + C)>>S] ))
  done
}

# ==== Images: Intel HEX ==================================================
# An image is a text file (doc/vm.md, section 10). The first line names
# the cell size and the start words; then come Intel HEX records for the
# system variables, the RAM data up to VP and the dictionary up to DP.

hexseg() {                          # $1 start, $2 end (exclusive)
  local a=$1 e=$2 n j b sum data hi
  while (( a < e )); do
    (( hi = a >> 16 ))
    if (( hi != ULBA )); then       # extended linear address: the upper 16 bits
      ULBA=$hi
      printf ':02000004%04X%02X\n' $hi $(( -(6 + (hi >> 8) + (hi & 255)) & 255 ))
    fi
    (( n = e - a, n > 16 && (n = 16), (a & 0xFFFF) + n > 0x10000 && (n = 0x10000 - (a & 0xFFFF)) ))
    (( sum = n + (a >> 8 & 255) + (a & 255) ))
    data=
    for (( j = 0; j < n; j++ )); do
      (( b = M[(a+j)>>S] >> (((a+j)&A)<<3) & 255, sum += b ))
      printf -v data '%s%02X' "$data" $b
    done
    printf ':%02X%04X00%s%02X\n' $n $(( a & 0xFFFF )) "$data" $(( -sum & 255 ))
    (( a += n ))
  done
}

write_image() {                     # $1 file, $2 xt of the start word. Status 1 if it fails
  ULBA=0                            # upper address bits; a record only above $FFFF
  {
    printf '; ucf image bits=%d boot=%d quit=%d throw=%d prims=%s\n' \
           $BITS $2 $XT_QUIT $XT_THROW $PRIMS
    hexseg $SYS $DATA_START
    hexseg $DATA_START $(( M[VP_A>>S] ))
    hexseg $DICT_START $(( M[DP_A>>S] ))
    printf ':00000001FF\n'
  } 2>/dev/null > "$1"
}

hexread() {                         # $1 file, $2 "load" or "check": each data byte
  local line n a t j b sum hi=0 k=0
  NDIFF=0 FIRSTDIFF=
  while IFS= read -r line || [ -n "$line" ]; do
    line=${line%$'\r'}
    (( k++ ))
    case $line in
      ';'*) case $line in *" boot="*) META=$line ;; esac; continue ;;
      :*)   ;;
      *)    continue ;;
    esac
    (( n = 16#${line:1:2}, a = 16#${line:3:4}, t = 16#${line:7:2} ))
    (( sum = n + (a >> 8) + (a & 255) + t + 16#${line:9+2*n:2} ))   # n is known now
    for (( j = 0; j < n; j++ )); do (( sum += 16#${line:9+2*j:2} )); done
    (( sum & 255 )) && { printf 'ucf.sh: %s:%d: wrong checksum\n' "$1" $k >&2; exit 1; }
    case $t in
      0) for (( j = 0; j < n; j++ )); do
           (( b = 16#${line:9+2*j:2} ))
           if [ "$2" = load ]; then
             setb $(( hi + a + j )) $b
           else
             getb $(( hi + a + j ))
             if (( R != b )); then
               (( NDIFF++ ))
               [ -n "$FIRSTDIFF" ] || printf -v FIRSTDIFF '$%X (here %d, there %d)' $(( hi + a + j )) $R $b
             fi
           fi
         done ;;
      1) break ;;
      4) (( hi = 16#${line:9:4} << 16 )) ;;
    esac
  done < "$1"
}

metaval() {                         # $1 key: R = its number in the first line of an image
  local v=${META#*" $1="}
  [ "$v" = "$META" ] && { R=0; return; }
  R=${v%%" "*}
}

load_image() {                      # $1 file: memory, start words, primitives
  local list r lo hi op
  META=
  hexread "$1" load
  [ -n "$META" ] || { printf 'ucf.sh: %s is not an image of uCForth\n' "$1" >&2; exit 1; }
  metaval boot; XT_BOOT=$R; metaval quit; XT_QUIT=$R; metaval throw; XT_THROW=$R
  list=${META#*" prims="}; list=${list%%" "*}
  local IFS=,
  for r in $list; do                # each range lo-hi: this VM must have it
    lo=${r%-*} hi=${r#*-}
    for (( op = lo; op <= hi; op++ )); do
      (( HAVE[op] )) || { printf 'ucf.sh: %s needs primitive %d\n' "$1" $op >&2; exit 1; }
    done
  done
}

check_image() {                     # $1 name, $2 file: compare the memory with a reference
  if [ ! -r "$2" ]; then
    msg "check $1: cannot read $2"; CHECKFAIL=1; return
  fi
  hexread "$2" check
  if (( NDIFF )); then
    msg "check $1: $NDIFF bytes differ from $2, the first at $FIRSTDIFF"; CHECKFAIL=1
  else
    msg "check $1: the same as $2"
  fi
}

save_image() {                      # SAVE-IMAGE ( -- ior )
  local f=${UCF_IMAGE:-ucf.hex}
  if write_image "$f" $(( XT_BOOT ? XT_BOOT : XT_QUIT )); then
    msg "saved: $f"; push 0
  else
    push -37
  fi
}

# ==== Start ==============================================================

words_in_forth() {
  local nt n=0
  (( nt = M[WL_FORTH>>S] ))
  while (( nt )); do (( n++, nt = M[nt>>S] )); done
  R=$n
}

core_end() {                        # Genesis is complete: seed and check
  if [ -n "$DUMP_CORE" ]; then write_image "$DUMP_CORE" $XT_QUIT || msg "cannot write $DUMP_CORE"; fi
  (( CHECK )) && check_image core$BITS ${PFX}seed/core$BITS.hex
}

build_end() {                       # base is complete (doc/genesis.md, section 6)
  local here t
  (( here = M[DP_A>>S], M[BASEEND_A>>S] = here, M[FROZENEND_A>>S] = here,
     M[NORDER_A>>S] = 1, M[CONTEXT_A>>S] = WL_FORTH, M[CURRENT_A>>S] = WL_FORTH,
     M[STATE_A>>S] = 0 ))
  BUILD_END=-2
  sysxt BOOT; XT_BOOT=$R
  if [ -n "$DUMP" ]; then write_image "$DUMP" $XT_BOOT || msg "cannot write $DUMP"; fi
  if (( CHECK )); then
    OB=                             # the prompt of QUIT is not wanted here
    check_image base$BITS ${PFX}seed/base$BITS.hex
    bye $CHECKFAIL
  fi
  now_ms; (( t = (R - T0 + 500) / 1000 ))
  words_in_forth
  printf 'uCForth - bash bootstrap VM - %d bit%s - %d words in FORTH - built in %d s\n' \
         $BITS "$( (( PURE )) && printf ' - pure')" $R $t
}

CHECKFAIL=0
if [ -n "$IMAGE" ]; then
  load_image "$IMAGE"
  FILES=("${ARGS[@]}")
  find_refill
  words_in_forth
  printf 'uCForth - bash bootstrap VM - %d bit - %d words in FORTH - image %s\n' $BITS $R "$IMAGE"
  (( sp = S0, rp = R0, M[STATE_A>>S] = 0, M[HANDLER_A>>S] = 0 ))
  run_xt $(( XT_BOOT ? XT_BOOT : XT_QUIT ))
else
  FILES=("$CORE_FILE" "$BASE_FILE" "${ARGS[@]}")
  genesis
  sysxt THROW; XT_THROW=$R; sysxt QUIT; XT_QUIT=$R
  (( XT_QUIT )) || gfail "${FILES[0]} does not define QUIT"
  find_refill
  core_end
  FI=1 BUILD_END=1
  run_xt $XT_QUIT
fi
bye 0
