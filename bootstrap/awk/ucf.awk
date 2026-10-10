# ucf.awk - the awk bootstrap VM of uCForth
#
# It is the virtual machine of uCForth in awk: the memory, the
# primitives and a small HAL (doc/vm.md). Its Genesis, a small outer
# interpreter, compiles forth/core.fs once (doc/genesis.md). From then on
# QUIT in the image reads each line, as on a board: first forth/base.fs,
# then the files of the command line, then the keyboard (or a pipe).
#
# Use:  LC_ALL=C awk -f ucf.awk -- [OPTION ...] [FILE ...]
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
# "--" stops the options of awk, so that awk gives "-32" and the others
# to this script. SAVE-IMAGE (and so FREEZE) writes the image to ucf.hex,
# or to the file in the environment variable UCF_IMAGE. The script uses
# only POSIX awk: no bit operations, no extensions. Tested with the awk
# of macOS, gawk, mawk and BusyBox awk.
#
# Limits:
# - ?RX waits for a complete line: awk reads lines, not single keys.
# - No checks of the stacks and of addresses, as in a kernel on a board.
# - awk has only a clock with seconds: MS does not wait, and MILLIS
#   counts the whole seconds plus the MS times.
# - REBOOT starts BOOT or QUIT again, but does not load the image again.
#   ERASE-IMAGE does nothing.
# - Text files only. OPEN-FILE with W/O or R/W writes at the end of the
#   file.

BEGIN {
  BUILD_END = -1                    # all other variables start with 0 or ""
  options()
  memory_map()
  primitives()
  tables()
  start()
}

function usage() {
  printf "%s\n%s\n", "use: LC_ALL=C awk -f ucf.awk -- [-32] [-pure] [-core FILE] [-base FILE]", \
         "       [-image FILE] [-dump-core FILE] [-dump FILE] [-check] [FILE ...]" > "/dev/stderr"
  exit 2
}

# ==== Options ===========================================================
# The default files lie in the project. The script looks for them in the
# current folder (ucf), then two folders above (a start in bootstrap/awk).

function options(   i, a, n) {
  if (readable("forth/core.fs")) PFX = ""
  else if (readable("../../forth/core.fs")) PFX = "../../"
  else PFX = ""
  BITS = 16; PURE = 0; CHECK = 0; IMAGE = ""; DUMP = ""; DUMP_CORE = ""
  CORE_FILE = PFX "forth/core.fs"; BASE_FILE = PFX "forth/base.fs"
  NARGS = 0
  for (i = 1; i < ARGC; i++) {
    a = ARGV[i]; ARGV[i] = ""       # so that getline reads the keyboard
    if (a == "--") continue
    if (a == "-32") BITS = 32
    else if (a == "-pure") PURE = 1
    else if (a == "-check") { CHECK = 1; PURE = 1 }
    else if (a == "-core" || a == "-base" || a == "-image" || a == "-dump-core" || a == "-dump") {
      if (i + 1 >= ARGC) usage()
      n = ARGV[i + 1]; ARGV[++i] = ""
      if (a == "-core") CORE_FILE = n
      else if (a == "-base") BASE_FILE = n
      else if (a == "-image") IMAGE = n
      else if (a == "-dump-core") DUMP_CORE = n
      else DUMP = n
    }
    else if (substr(a, 1, 1) == "-") usage()
    else ARGS[NARGS++] = a
  }
  if (IMAGE != "") {                # the first line of an image names the cell size
    if (slurp(IMAGE) < 0) { printf "ucf.awk: cannot read %s\n", IMAGE > "/dev/stderr"; exit 1 }
    if (index(SLURP, "bits=32")) BITS = 32; else BITS = 16
  }
  if (length("\303\244") != 2)
    printf "ucf.awk: awk counts characters, not bytes; start it with LC_ALL=C\n" > "/dev/stderr"
}

# ==== Memory map (doc/vm.md, section 3) =================================
# M[a] holds the cell at the address a; a is a multiple of the cell size.
# Bytes are little endian: C@ and C! take the byte out of its cell.

function memory_map(   k, t, a, i, n, cells) {
  C = BITS / 8; A = C - 1; K = C / 2
  MOD = 2 ^ BITS; MASK = MOD - 1; SIGN = MOD / 2
  SYS = 256; DATA_START = 512 * K; DATA_END = (K == 1) ? 4096 : 65536
  TIB = DATA_END; TIB_SIZE = 256; PAD = TIB + 256
  RS_BOTTOM = PAD + 256; R0 = RS_BOTTOM + 256 * C
  S0 = R0 + 256 * C; DICT_START = S0
  GUARD = 8 * C                     # the first word comes 8 cells above S0:
                                    # a stack underflow lands in these free cells
  MEM_END = (K == 1) ? 65536 : 262144; DICT_END = MEM_END - 16
  HALT_XT = DATA_START - 16; HALT_ADDR = HALT_XT + C
  RUN_ADDR = MEM_END - 2 * C        # two cells to run one word
  THROW_ADDR = MEM_END - 4 * C      # two cells to run THROW
  WL_FORTH = DATA_START; WL_INTERNALS = WL_FORTH + 3 * C
  VP_START = WL_INTERNALS + 3 * C
  for (a = 0; a < MEM_END; a += C) M[a] = 0   # each cell is a number
  for (k = 0; k < 4; k++) P8[k] = 256 ^ k
  for (k = 0; k <= 32; k++) P2[k] = 2 ^ k

  # System variables: name, cells, vocabulary (F = FORTH, I = INTERNALS)
  n = split("DP 1 I VP 1 I LATEST 1 I STATE 1 I BASE 1 F >IN 1 I #TIB 1 I " \
            "CURRENT 1 I #ORDER 1 I CONTEXT 16 I VOC-LINK 1 I CASE-SENSE 1 I " \
            "BASE-END 1 I FROZEN-END 1 I AUTORUN 1 I ERRMSG 2 I HANDLER 1 I", t, " ")
  NSV = 0; a = SYS
  for (i = 1; i <= n; i += 3) {
    SV_NAME[NSV] = t[i]; SV_VAL[NSV] = a; SV_VOC[NSV] = t[i + 2]; NSV++
    a += t[i + 1] * C
  }
  DP_A = SV_VAL[0]; VP_A = SV_VAL[1]; LATEST_A = SV_VAL[2]; STATE_A = SV_VAL[3]
  BASE_A = SV_VAL[4]; TOIN_A = SV_VAL[5]; NTIB_A = SV_VAL[6]; CURRENT_A = SV_VAL[7]
  NORDER_A = SV_VAL[8]; CONTEXT_A = SV_VAL[9]; VOCLINK_A = SV_VAL[10]
  CASESENSE_A = SV_VAL[11]; BASEEND_A = SV_VAL[12]; FROZENEND_A = SV_VAL[13]
  HANDLER_A = SV_VAL[16]
  n = split("TIB PAD S0 R0 DATA-END WL-FORTH WL-INTERNALS CELL DICT-END", t, " ")
  split(TIB " " PAD " " S0 " " R0 " " DATA_END " " WL_FORTH " " WL_INTERNALS " " C " " DICT_END, cells, " ")
  for (i = 1; i <= n; i++) { SV_NAME[NSV] = t[i]; SV_VAL[NSV] = cells[i] + 0; SV_VOC[NSV] = "I"; NSV++ }
}

# ==== Primitives ========================================================
# Number, name, vocabulary, flags (64 = compile-only, 128 = immediate).
# 0-3 are the code field types, 4-31 the other mandatory primitives,
# 32-65 the optional ones, 80-88 the HAL, 100-108 exist only for Genesis.

function primitives(   t, n, i, op, prev, first, ops, k) {
  n = split("4 EXIT F 64  5 LIT I 64  6 BRANCH I 64  7 ?BRANCH I 64 " \
    "8 EXECUTE F 0  9 @ F 0  10 ! F 0  11 C@ F 0  12 C! F 0  13 DUP F 0 " \
    "14 DROP F 0  15 SWAP F 0  16 OVER F 0  17 >R F 64  18 R> F 64 " \
    "19 R@ F 64  20 SP@ I 0  21 SP! I 0  22 RP@ I 0  23 RP! I 0  24 AND F 0 " \
    "25 OR F 0  26 XOR F 0  27 0< F 0  28 UM+ I 0  29 ?RX I 0  30 TX! I 0 " \
    "31 !IO I 0 " \
    "32 + F 0  33 - F 0  34 1+ F 0  35 1- F 0  36 NEGATE F 0  37 INVERT F 0 " \
    "38 2* F 0  39 2/ F 0  40 LSHIFT F 0  41 RSHIFT F 0  42 = F 0  43 0= F 0 " \
    "44 < F 0  45 U< F 0  46 * F 0  47 UM* F 0  48 UM/MOD F 0 " \
    "49 ROT F 0  50 NIP F 0  51 ?DUP F 0  52 2DUP F 0  53 2DROP F 0 " \
    "54 +! F 0  55 MOVE F 0  56 FILL F 0  57 COUNT F 0  58 FIND-NAME I 0 " \
    "59 (DO) I 64  60 (?DO) I 64  61 (LOOP) I 64  62 (+LOOP) I 64 " \
    "63 I F 64  64 J F 64  65 UNLOOP F 64 " \
    "80 MILLIS F 0  81 MS F 0  82 US F 0  83 BIS! F 0  84 BIC! F 0 " \
    "85 SAVE-IMAGE I 0  86 ERASE-IMAGE I 0  87 REBOOT I 0  88 BYE F 0 " \
    "100 PARSE-NAME I 0  101 PARSE I 0  102 HEADER I 0  103 REVEAL I 0 " \
    "104 IMMEDIATE F 0  105 , F 0  106 C, F 0  107 [ F 128  108 ] F 0", t, " ")
  NPRIM = 0
  for (i = 1; i <= n; i += 4) {
    op = t[i] + 0
    NAME[op] = t[i + 1]; VOC[op] = t[i + 2]; FLAG[op] = t[i + 3] + 0
    if (op < 32 || op >= 80 || !PURE) PRIM_OP[NPRIM++] = op
  }
  # HAVE marks the primitives that this VM has; an image may use only
  # these. PRIMS lists the primitives with a header, as "0-31,80-88".
  for (op = 0; op <= 65; op++) HAVE[op] = 1
  for (op = 80; op <= 93; op++) HAVE[op] = 1
  HAVE[98] = 1; HAVE[99] = 1
  ops[0] = 0; ops[1] = 1; ops[2] = 2; ops[3] = 3; k = 4
  for (i = 0; i < NPRIM; i++) if (PRIM_OP[i] < 100) ops[k++] = PRIM_OP[i]
  PRIMS = ""; prev = -2; first = 0
  for (i = 0; i < k; i++) {
    op = ops[i]
    if (op != prev + 1) {
      if (prev >= 0) PRIMS = PRIMS (first == prev ? first : first "-" prev) ","
      first = op
    }
    prev = op
  }
  PRIMS = PRIMS (first == prev ? first : first "-" prev)
}

# ==== Tables ============================================================
# CH[c] is the character with the code c, ORD[ch] its code, HEXV[ch] the
# value of a hex digit. AND8[x * 256 + y] is x AND y for two bytes: POSIX
# awk has no bit operations. OR and XOR come from AND:
#   x OR y = x + y - (x AND y),   x XOR y = x + y - 2 * (x AND y)

function tables(   i, x, y, a, b, r, p, k, xh, xl, A4) {
  for (i = 0; i < 256; i++) { CH[i] = sprintf("%c", i); ORD[CH[i]] = i }
  for (i = 0; i < 16; i++) {
    HEXV[substr("0123456789ABCDEF", i + 1, 1)] = i
    HEXV[substr("0123456789abcdef", i + 1, 1)] = i
  }
  for (x = 0; x < 16; x++)
    for (y = 0; y < 16; y++) {
      r = 0; p = 1; a = x; b = y
      for (k = 0; k < 4; k++) {
        if (a % 2 && b % 2) r += p
        a = int(a / 2); b = int(b / 2); p *= 2
      }
      A4[x * 16 + y] = r
    }
  for (x = 0; x < 256; x++) {
    xh = int(x / 16) * 16; xl = (x % 16) * 16
    for (y = 0; y < 256; y++)
      AND8[x * 256 + y] = A4[xh + int(y / 16)] * 16 + A4[xl + y % 16]
  }
}

function band(x, y,   r, p, k) {  # x AND y for two cells
  if (x == MASK) return y
  if (y == MASK) return x
  r = 0; p = 1
  for (k = 0; k < C; k++) {
    r += AND8[(x % 256) * 256 + y % 256] * p
    x = int(x / 256); y = int(y / 256); p *= 256
  }
  return r
}

# ==== Helpers ===========================================================

function getb(a,   r) { r = a % C; return int(M[a - r] / P8[r]) % 256 }
function setb(a, b,   r, x, c) {
  r = a % C; x = a - r; c = M[x]
  M[x] = c + (b % 256 - int(c / P8[r]) % 256) * P8[r]
}
function push(x) { sp -= C; x %= MOD; if (x < 0) x += MOD; M[sp] = x }
function signed(x) { return x >= SIGN ? x - MOD : x }
function ntxt(nt) { return int((nt + C + 1 + M[nt + C] % 32 + A) / C) * C }   # nt -> xt

function readable(f,   r, s) { s = (getline r < f); close(f); return s >= 0 }

# slurp FILE: SLURP = the whole file, also the last line end. Status -1
# if the file cannot be read. RS is a byte that a text does not hold, so
# getline gives the file in one piece, with each LF.
function slurp(f,   rs, r, s, n) {
  rs = RS; RS = "\001"; SLURP = ""; n = 0
  while ((s = (getline r < f)) > 0) SLURP = (n++ ? SLURP "\001" : "") r
  close(f); RS = rs
  return s
}

# readlines FILE: FL[1..FLN] = the lines (doc/genesis.md, section 2): the
# text after the last LF is one more line, also when it is empty.
function readlines(f,   i) {
  if (slurp(f) < 0) return 0
  split("", FL)
  FLN = split(SLURP, FL, "\n")
  if (FLN == 0) { FLN = 1; FL[1] = "" }
  for (i = 1; i <= FLN; i++) sub(/\r$/, "", FL[i])
  return 1
}

# A file name for the shell: in single quotes, without a single quote.
function canwrite(f) {            # a new, empty file f; status 1 if it works
  if (index(f, "'")) return 0       # a new file, not the old one shorter: macOS keeps the signature of a program for each file
  return system("2>/dev/null rm -f '" f "' && 2>/dev/null : > '" f "'") == 0
}

# VMS: the time that MS waited (virtual). srand() gives the time: in
# seconds, or in a smaller unit (mawk 1.3.4 of 2025: nanoseconds).
function now_ms(   t) {
  srand(); t = srand()
  while (t > 1e11) t /= 1000        # a smaller unit: make seconds of it
  return int(t * 1000) + VMS
}

function msg(s) {                   # a message of this script on its own line
  if (OB != "") { printf "%s\n", OB; OB = "" }
  printf "  %s\n", s
}

function bye(n) {
  if (OB != "") printf "%s\n", OB
  exit n
}

function throw_text(n) {
  if (n == -3) return "data stack overflow"
  if (n == -4) return "data stack underflow"
  if (n == -8) return "dictionary overflow"
  if (n == -10) return "division by zero"
  if (n == -11) return "result out of range"
  if (n == -13) return "undefined word"
  if (n == -14) return "compile-only word"
  if (n == -16) return "missing name"
  if (n == -19) return "name too long"
  if (n == -21) return "unsupported operation"
  if (n == -24) return "number out of range"
  return "error " n
}

# ==== Search in the dictionary ==========================================
# KEY[0..KN-1] holds the name; without CASE-SENSE in upper case.

function wlfind(wl,   nt, j, a, b, cs) {   # the nt in the wordlist wl, or 0
  nt = M[wl]; cs = M[CASESENSE_A]
  while (nt) {
    if (M[nt + C] % 32 == KN) {
      a = nt + C + 1
      for (j = 0; j < KN; j++) {
        b = getb(a + j)
        if (!cs && b >= 97 && b <= 122) b -= 32
        if (b != KEY[j]) break
      }
      if (j == KN) return nt
    }
    nt = M[nt]
  }
  return 0
}

function gfind(   i, nt) {          # the search order, the top first: nt or 0
  if (KN == 0 || KN > 31) return 0
  for (i = M[NORDER_A] - 1; i >= 0; i--) {
    nt = wlfind(M[CONTEXT_A + i * C])
    if (nt) return nt
  }
  return 0
}

function keystr(s,   j, c, cs) {    # KEY from the string s
  KN = length(s); cs = M[CASESENSE_A]
  for (j = 0; j < KN; j++) {
    c = ORD[substr(s, j + 1, 1)]
    if (!cs && c >= 97 && c <= 122) c -= 32
    KEY[j] = c
  }
}

function keymem(a, n,   j, b, cs) { # KEY from memory: address a, length n
  KN = n; cs = M[CASESENSE_A]
  for (j = 0; j < n; j++) {
    b = getb(a + j)
    if (!cs && b >= 97 && b <= 122) b -= 32
    KEY[j] = b
  }
}

function sysxt(s,   wl, w, nt, found, j, a, n) {   # the oldest word with exactly this name
  n = length(s)
  for (w = 0; w < 2; w++) {
    wl = w ? WL_INTERNALS : WL_FORTH
    found = 0; nt = M[wl]
    while (nt) {
      if (M[nt + C] % 32 == n) {
        a = nt + C + 1
        for (j = 0; j < n; j++) if (getb(a + j) != ORD[substr(s, j + 1, 1)]) break
        if (j == n) found = nt
      }
      nt = M[nt]
    }
    if (found) return ntxt(found)
  }
  return 0
}

# ==== The virtual machine ===============================================
# ip, w, sp and rp are the registers, op is the code field of w. SP and
# RP point to the top cell; the stacks grow down. Flags are MASK or 0.

# XT_THROW, XT_QUIT, XT_BOOT: the xts of THROW, QUIT and BOOT

function throw_(n) {                # THROW n in the image: the next word is THROW
  if (!XT_THROW) gfail(throw_text(n) " (no THROW yet)")
  push(n)
  M[THROW_ADDR] = XT_THROW; M[THROW_ADDR + C] = HALT_XT; ip = THROW_ADDR
}

function run_xt(xt) {               # execute xt; return when it is complete
  M[RUN_ADDR] = xt; M[RUN_ADDR + C] = HALT_XT; ip = RUN_ADDR
  vm()
}

# vm: NEXT and the primitives, until HALT (127). The tests of op form a
# tree: first groups of 16, then groups of 4, so that each primitive
# needs only a few tests.
function vm(   t, a, x, y, n) {
  while (1) {
    w = M[ip]; ip += C; op = M[w]
    while (op == 8) { w = M[sp]; sp += C; op = M[w] }   # EXECUTE
    if (op < 16) {
      if (op < 4) {
        if (op == 0) { rp -= C; M[rp] = ip; ip = w + C }                   # DOCOL
        else if (op == 2) { sp -= C; M[sp] = M[w + C] }                    # DOCON
        else if (op == 1) { sp -= C; M[sp] = w + C + C }                   # DOVAR
        else { sp -= C; M[sp] = w + C + C; rp -= C; M[rp] = ip; ip = M[w + C] }   # DODOES
      } else if (op < 8) {
        if (op == 4) { ip = M[rp]; rp += C }                               # EXIT
        else if (op == 5) { sp -= C; M[sp] = M[ip]; ip += C }              # LIT
        else if (op == 7) { if (M[sp]) ip += C; else ip = M[ip]; sp += C } # ?BRANCH
        else ip = M[ip]                                                    # BRANCH
      } else if (op < 12) {
        if (op == 9) M[sp] = M[M[sp]]                                      # @
        else if (op == 10) { M[M[sp]] = M[sp + C]; sp += C + C }           # !
        else if (op == 11) M[sp] = getb(M[sp])                             # C@
      } else {
        if (op == 13) { sp -= C; M[sp] = M[sp + C] }                       # DUP
        else if (op == 14) sp += C                                         # DROP
        else if (op == 15) { t = M[sp]; M[sp] = M[sp + C]; M[sp + C] = t } # SWAP
        else { setb(M[sp], M[sp + C]); sp += C + C }                       # C!
      }
    } else if (op < 32) {
      if (op < 20) {
        if (op == 16) { sp -= C; M[sp] = M[sp + C + C] }                   # OVER
        else if (op == 17) { rp -= C; M[rp] = M[sp]; sp += C }             # >R
        else if (op == 18) { sp -= C; M[sp] = M[rp]; rp += C }             # R>
        else { sp -= C; M[sp] = M[rp] }                                    # R@
      } else if (op < 24) {
        if (op == 20) { t = sp; sp -= C; M[sp] = t }                       # SP@
        else if (op == 21) sp = M[sp]                                      # SP!
        else if (op == 22) { sp -= C; M[sp] = rp }                         # RP@
        else { rp = M[sp]; sp += C }                                       # RP!
      } else if (op < 28) {
        t = M[sp]; sp += C; x = M[sp]
        if (op == 24) M[sp] = band(x, t)                                   # AND
        else if (op == 25) M[sp] = x + t - band(x, t)                      # OR
        else if (op == 26) M[sp] = x + t - 2 * band(x, t)                  # XOR
        else { sp -= C; M[sp] = t >= SIGN ? MASK : 0 }                     # 0<
      } else {
        if (op == 28) {                                                    # UM+
          t = M[sp + C] + M[sp]
          if (t >= MOD) { M[sp + C] = t - MOD; M[sp] = 1 } else { M[sp + C] = t; M[sp] = 0 }
        }
        else if (op == 29) rx()                                            # ?RX
        else if (op == 30) tx()                                            # TX!
      }                                                                    # 31 !IO: nothing
    } else if (op < 48) {
      if (op < 36) {
        if (op == 32) { t = M[sp]; sp += C; t += M[sp]; M[sp] = t >= MOD ? t - MOD : t }   # +
        else if (op == 33) { t = M[sp]; sp += C; t = M[sp] - t; M[sp] = t < 0 ? t + MOD : t }   # -
        else if (op == 34) { t = M[sp] + 1; M[sp] = t == MOD ? 0 : t }     # 1+
        else { t = M[sp] - 1; M[sp] = t < 0 ? MASK : t }                   # 1-
      } else if (op < 40) {
        if (op == 36) { t = M[sp]; M[sp] = t ? MOD - t : 0 }               # NEGATE
        else if (op == 37) M[sp] = MASK - M[sp]                            # INVERT
        else if (op == 38) { t = M[sp] * 2; M[sp] = t >= MOD ? t - MOD : t }   # 2*
        else { t = M[sp]; M[sp] = int(t / 2) + (t >= SIGN ? SIGN : 0) }    # 2/
      } else if (op < 44) {
        if (op == 42) { t = M[sp]; sp += C; M[sp] = M[sp] == t ? MASK : 0 }   # =
        else if (op == 43) M[sp] = M[sp] ? 0 : MASK                        # 0=
        else if (op == 40) { t = M[sp]; sp += C; M[sp] = t >= BITS ? 0 : M[sp] * P2[t] % MOD }   # LSHIFT
        else { t = M[sp]; sp += C; M[sp] = t >= BITS ? 0 : int(M[sp] / P2[t]) }   # RSHIFT
      } else {
        if (op == 44) { t = signed(M[sp]); sp += C; M[sp] = signed(M[sp]) < t ? MASK : 0 }   # <
        else if (op == 45) { t = M[sp]; sp += C; M[sp] = M[sp] < t ? MASK : 0 }   # U<
        else if (op == 46) { t = M[sp]; sp += C; mul(M[sp], t); M[sp] = MLO }   # *
        else { mul(M[sp + C], M[sp]); M[sp + C] = MLO; M[sp] = MHI }       # UM*
      }
    } else if (op < 66) {
      if (op < 52) {
        if (op == 48) ummod()                                              # UM/MOD
        else if (op == 49) { t = M[sp + C + C]; M[sp + C + C] = M[sp + C]; M[sp + C] = M[sp]; M[sp] = t }   # ROT
        else if (op == 50) { t = M[sp]; sp += C; M[sp] = t }               # NIP
        else if (M[sp]) { sp -= C; M[sp] = M[sp + C] }                     # ?DUP
      } else if (op < 56) {
        if (op == 52) { sp -= C + C; M[sp] = M[sp + C + C]; M[sp + C] = M[sp + 3 * C] }   # 2DUP
        else if (op == 53) sp += C + C                                     # 2DROP
        else if (op == 54) { a = M[sp]; t = M[a] + M[sp + C]; M[a] = t >= MOD ? t - MOD : t; sp += C + C }   # +!
        else move_()                                                       # MOVE
      } else if (op < 60) {
        if (op == 57) { a = M[sp]; M[sp] = a + 1 == MOD ? 0 : a + 1; sp -= C; M[sp] = getb(a) }   # COUNT
        else if (op == 58) findname()                                      # FIND-NAME
        else if (op == 56) fill_()                                         # FILL
        else { rp -= 3 * C; M[rp + C + C] = M[ip]; M[rp + C] = M[sp + C]; M[rp] = M[sp]; sp += C + C; ip += C }   # (DO)
      } else if (op < 63) {
        if (op == 61) {                                                    # (LOOP)
          t = M[rp] + 1; if (t == MOD) t = 0
          if (t == M[rp + C]) { rp += 3 * C; ip += C } else { M[rp] = t; ip = M[ip] }
        } else if (op == 60) {                                             # (?DO)
          if (M[sp] == M[sp + C]) ip = M[ip]
          else { rp -= 3 * C; M[rp + C + C] = M[ip]; M[rp + C] = M[sp + C]; M[rp] = M[sp]; ip += C }
          sp += C + C
        } else {                                                           # (+LOOP)
          n = signed(M[sp]); sp += C
          x = M[rp] - M[rp + C]; if (x < 0) x += MOD; x = signed(x)
          y = x + n; if (y >= SIGN) y -= MOD; else if (y < -SIGN) y += MOD
          t = M[rp] + n; if (t >= MOD) t -= MOD; else if (t < 0) t += MOD
          M[rp] = t
          if ((x < 0) != (y < 0) && (x < 0) != (n < 0)) { rp += 3 * C; ip += C } else ip = M[ip]
        }
      } else {
        if (op == 63) { sp -= C; M[sp] = M[rp] }                           # I
        else if (op == 64) { sp -= C; M[sp] = M[rp + 3 * C] }              # J
        else rp += 3 * C                                                   # UNLOOP
      }
    } else if (op == 127) return                                           # HALT: back to awk
    else hal(op)
  }
}

function mul(x, y,   xh, xl, yh, yl, mid, t) {   # MLO MHI = x * y
  if (BITS == 16) { t = x * y; MLO = t % 65536; MHI = int(t / 65536); return }
  xh = int(x / 65536); xl = x % 65536; yh = int(y / 65536); yl = y % 65536
  mid = xh * yl + xl * yh           # below 2^33: awk counts exactly up to 2^53
  t = xl * yl + (mid % 65536) * 65536
  MLO = t % MOD
  MHI = xh * yh + int(mid / 65536) + int(t / MOD)
}

function ummod(   u, hi, lo, q, r, n, d) {   # ( lo hi u -- rem quot )
  u = M[sp]; hi = M[sp + C]; lo = M[sp + C + C]; sp += C
  if (u == 0) { throw_(-10); return }
  if (hi >= u) { throw_(-11); return }
  if (BITS == 16) {
    n = hi * 65536 + lo; q = int(n / u); r = n - q * u
  } else {                          # two steps of 16 bits: each number stays below 2^48
    n = hi * 65536 + int(lo / 65536); d = int(n / u); r = n - d * u
    if (r < 0) { d--; r += u } else if (r >= u) { d++; r -= u }
    q = d
    n = r * 65536 + lo % 65536; d = int(n / u); r = n - d * u
    q = q * 65536 + d
  }
  if (r < 0) { q--; r += u } else if (r >= u) { q++; r -= u }
  M[sp + C] = r; M[sp] = q
}

function move_(   a1, a2, u, j) {   # ( a1 a2 u -- ), also for areas that overlap
  u = M[sp]; a2 = M[sp + C]; a1 = M[sp + C + C]; sp += 3 * C
  if (a1 % C == 0 && a2 % C == 0 && u % C == 0) {   # whole cells
    if (a1 < a2) { for (j = u - C; j >= 0; j -= C) M[a2 + j] = M[a1 + j] }
    else { for (j = 0; j < u; j += C) M[a2 + j] = M[a1 + j] }
  } else if (a1 < a2) {
    for (j = u - 1; j >= 0; j--) setb(a2 + j, getb(a1 + j))
  } else {
    for (j = 0; j < u; j++) setb(a2 + j, getb(a1 + j))
  }
}

function fill_(   a, u, c, j) {     # ( a u c -- )
  c = M[sp] % 256; u = M[sp + C]; a = M[sp + C + C]; sp += 3 * C
  for (j = 0; j < u; j++) setb(a + j, c)
}

function findname(   a, u) {        # ( a u -- nt | 0 )
  u = M[sp]; a = M[sp + C]; sp += C
  if (u == 0 || u > 31) M[sp] = 0
  else { keymem(a, u); M[sp] = gfind() }
}

function reboot(   f) {             # ( f -- ): stacks empty, then BOOT or QUIT
  f = M[sp]
  sp = S0; rp = R0; M[STATE_A] = 0; M[HANDLER_A] = 0
  M[RUN_ADDR] = (f && XT_BOOT) ? XT_BOOT : XT_QUIT; M[RUN_ADDR + C] = HALT_XT
  ip = RUN_ADDR
}

function hal(op,   t, a, b) {       # the HAL, the files and the host primitives
  if (op == 80) push(now_ms() - T0)                                        # MILLIS
  else if (op == 81) { VMS += M[sp]; sp += C }                             # MS
  else if (op == 82) sp += C                                               # US
  else if (op == 83 || op == 84) {                                         # BIS! BIC!
    a = M[sp]; t = M[sp + C] % 256; sp += C + C; b = getb(a)
    if (op == 83) setb(a, b + t - AND8[b * 256 + t])
    else setb(a, b - AND8[b * 256 + t])
  }
  else if (op == 85) save_image()                                          # SAVE-IMAGE
  else if (op == 86) { }                                                   # ERASE-IMAGE
  else if (op == 87) reboot()                                              # REBOOT
  else if (op == 88) bye(0)                                                # BYE
  else if (op == 89) fopen(0)                                              # OPEN-FILE
  else if (op == 90) fopen(1)                                              # CREATE-FILE
  else if (op == 91) fclose()                                              # CLOSE-FILE
  else if (op == 92) fread()                                               # READ-FILE
  else if (op == 93) fwrite()                                              # WRITE-FILE
  else if (op == 98) freadline()                                           # READ-LINE
  else if (op == 99) bye(M[sp] % 256)                                      # (BYE)
  else if (op == 100) { pname(); push(PA); push(PN) }                      # PARSE-NAME
  else if (op == 101) { t = M[sp] % 256; sp += C; pparse(t); push(PA); push(PN) }   # PARSE
  else if (op == 102) gheader()                                            # HEADER
  else if (op == 103) { t = M[LATEST_A]; if (t) M[M[CURRENT_A]] = t }      # REVEAL
  else if (op == 104) { a = M[LATEST_A] + C; b = getb(a); if (b < 128) setb(a, b + 128) }   # IMMEDIATE
  else if (op == 105) { comma(M[sp]); sp += C }                            # ,
  else if (op == 106) { t = M[DP_A]; setb(t, M[sp]); sp += C; M[DP_A] = t + 1 }   # C,
  else if (op == 107) M[STATE_A] = 0                                       # [
  else if (op == 108) M[STATE_A] = MASK                                    # ]
  else throw_(-21)
}

# ==== Terminal: input lines for ?RX, output for TX! =====================
# The input comes from the files of the command line, then from the
# keyboard or a pipe. A file line hides the prompt of QUIT.

# NFILES, FILES: without -image: FILES[0] = core, FILES[1] = base
# FI: the next file to open
# CURFILE, CURLN: the file that loads, its line number
# IN_STR, IN_LEN, IN_POS: the current line, with LF
# BUILD_END: after this file the system is complete

function setline(s) { sub(/\r$/, "", s); IN_STR = s "\n"; IN_LEN = length(IN_STR); IN_POS = 0 }

function getline_(   line) {        # the next line for ?RX
  while (1) {
    if (CURFILE != "" && ERRSEEN) {
      msg(CURFILE ":" CURLN ": loading stopped")
      CURFILE = ""
      if (FI <= BUILD_END + 1 && BUILD_END >= 0) { FI = BUILD_END + 1; build_end() }
      FI = NFILES
    }
    ERRSEEN = 0
    if (CURFILE != "") {
      if (CURLN < CFN) { CURLN++; OB = ""; setline(CF[CURLN]); return }
      CURFILE = ""
      if (FI == BUILD_END + 1) build_end()
      continue
    }
    if (FI < NFILES) {
      CURFILE = FILES[FI]; CURLN = 0; FI++
      if (!load_file(CURFILE)) { msg("cannot read " CURFILE); CURFILE = ""; FI = NFILES }
      continue
    }
    if (TTY) {                      # the keyboard: the terminal shows the line
      printf "%s", OB; OB = ""; fflush()
      if ((getline line) <= 0) { printf "\n"; bye(0) }
    } else {                        # a pipe: show the line after the prompt
      fflush()
      if ((getline line) <= 0) bye(0)
      printf "%s%s\n", OB, line; OB = ""
    }
    setline(line); return
  }
}

function load_file(f,   i) {        # CF[1..CFN]: the lines of the file f
  if (!readlines(f)) return 0
  split("", CF)
  for (i = 1; i <= FLN; i++) CF[i] = FL[i]
  CFN = FLN
  return 1
}

# XT_REFILL, REFILL_END: the body of REFILL: QUIT waits for a line there

function refilling(   r) {          # 1 if a return address points into REFILL
  for (r = rp; r < rp + 8 * C && r < R0; r += C)
    if (M[r] > XT_REFILL && M[r] < REFILL_END) return 1
  return 0
}

function rx() {                     # ?RX ( -- c true | 0 )
  if (IN_POS >= IN_LEN) {
    if (!refilling() && (CURFILE != "" || FI < NFILES || !TTY)) {
      push(0); return               # a word reads keys while files load: no key (doc/vm.md 9)
    }
    getline_()                      # QUIT waits in REFILL, or a word waits at the keyboard
  }
  sp -= C + C; M[sp + C] = ORD[substr(IN_STR, ++IN_POS, 1)]; M[sp] = MASK
}

function bodyend(xt,   wl, nt, best) {   # the next header in memory after xt
  best = MEM_END; wl = M[VOCLINK_A]
  while (wl) {
    nt = M[wl]
    while (nt) { if (nt > xt && nt < best) best = nt; nt = M[nt] }
    wl = M[wl + C]
  }
  return best
}

function find_refill() { XT_REFILL = sysxt("REFILL"); REFILL_END = bodyend(XT_REFILL) }

function tx(   c) {                 # TX! ( c -- )
  c = M[sp] % 256; sp += C
  if (c == 10) {
    printf "%s\n", OB
    if (index(OB, "error:")) ERRSEEN = 1
    OB = ""
  } else if (c) OB = OB CH[c]
}

# ==== Files: HAL 89-93, 98 (doc/vm.md, section 8) ======================
# A file id (fid) is a number from 10 to 29. OPEN-FILE reads the whole
# file: FP[fid, 1..FPN[fid]] are its parts between the LF bytes. Each
# part but the last has an LF after it. FK[fid] is the current part, FO
# the bytes of it that are read. WRITE-FILE appends to the file FNAME.

function memstr(a, n,   j, s) {     # the n bytes at a as text
  s = ""
  for (j = 0; j < n; j++) s = s CH[getb(a + j)]
  return s
}

function fret2(x, y) { M[sp + C] = (x + MOD) % MOD; M[sp] = (y + MOD) % MOD }
function fret3(x, y, z) {
  M[sp + C + C] = (x + MOD) % MOD; M[sp + C] = (y + MOD) % MOD; M[sp] = (z + MOD) % MOD
}
function fid_ok(fd) { return fd >= 10 && fd < 30 && FOPEN[fd] }

function fopen(create,   fam, u, a, fd, name, i, n, part) {   # ( a u fam -- fid ior )
  fam = M[sp] % 4; u = M[sp + C]; a = M[sp + C + C]; sp += C
  name = memstr(a, u)
  for (fd = 10; fd < 30; fd++) if (!FOPEN[fd]) break
  if (fd == 30) { fret2(0, -37); return }
  if (create) {
    if (!canwrite(name)) { fret2(0, -37); return }
    SLURP = ""
  } else if (slurp(name) < 0) { fret2(0, -38); return }
  n = split(SLURP, part, "\n")
  if (n == 0) { n = 1; part[1] = "" }
  for (i = 1; i <= n; i++) FP[fd, i] = part[i]
  FPN[fd] = n; FK[fd] = 1; FO[fd] = 0
  FOPEN[fd] = 1; FNAME[fd] = name; FW[fd] = 0
  fret2(fd, 0)
}

function fclose(   fd) {            # CLOSE-FILE ( fid -- ior )
  fd = M[sp]
  if (fid_ok(fd)) {
    if (FW[fd]) close(FNAME[fd])
    FOPEN[fd] = 0; M[sp] = 0
  } else M[sp] = MOD - 37
}

function freadline(   a, u, fd, k, s, n) {   # READ-LINE ( a u fid -- u2 f ior )
  fd = M[sp]; u = M[sp + C]; a = M[sp + C + C]
  if (!fid_ok(fd)) { fret3(0, 0, -37); return }
  k = FK[fd]
  if (k > FPN[fd] || (k == FPN[fd] && FO[fd] >= length(FP[fd, k]))) {
    fret3(0, 0, 0); return          # the end of the file
  }
  s = substr(FP[fd, k], FO[fd] + 1)
  sub(/\r$/, "", s)
  n = length(s)
  if (n > u) { n = u; FO[fd] += n }   # the buffer is full: the rest of the line comes next
  else { FK[fd] = k + 1; FO[fd] = 0 }
  strmem(s, a, n)
  fret3(n, MASK, 0)
}

function fread(   a, u, fd, n, k, s, m) {   # READ-FILE ( a u fid -- u2 ior )
  fd = M[sp]; u = M[sp + C]; a = M[sp + C + C]; sp += C
  if (!fid_ok(fd)) { fret2(0, -37); return }
  n = 0
  while (n < u && FK[fd] <= FPN[fd]) {
    k = FK[fd]; s = FP[fd, k]
    m = length(s) - FO[fd]
    if (m > 0) {                    # the text of the part
      if (m > u - n) m = u - n
      strmem(substr(s, FO[fd] + 1, m), a + n, m)
      FO[fd] += m; n += m
    } else {                        # the text is out: now its LF, if there is one
      if (k < FPN[fd]) { setb(a + n, 10); n++ }
      FK[fd] = k + 1; FO[fd] = 0
    }
  }
  fret2(n, 0)
}

function strmem(s, a, n,   j) {     # the first n characters of s to address a
  for (j = 0; j < n; j++) setb(a + j, ORD[substr(s, j + 1, 1)])
}

# Some awks lose the byte 0 in a string, but printf "%c", 0 writes it.
# So the text goes out in pieces between the bytes 0.
function fwrite(   a, u, fd, j, b, s) {   # WRITE-FILE ( a u fid -- ior )
  fd = M[sp]; u = M[sp + C]; a = M[sp + C + C]; sp += C + C
  if (!fid_ok(fd)) { M[sp] = MOD - 37; return }
  s = ""
  for (j = 0; j < u; j++) {
    b = getb(a + j)
    if (b) s = s CH[b]
    else { printf "%s", s >> FNAME[fd]; printf "%c", 0 >> FNAME[fd]; s = "" }
  }
  printf "%s", s >> FNAME[fd]
  FW[fd] = 1; M[sp] = 0
}

# ==== Genesis: awk compiles core.fs (doc/genesis.md) =====================
# The outer interpreter: find the word, then execute or compile it; else
# convert a number. PARSE-NAME, PARSE, HEADER, REVEAL, IMMEDIATE, ",",
# "C,", "[" and "]" are host primitives until core.fs defines Forth
# versions. Genesis handles "?:", "\" and "(" itself until core.fs has them.

# GLN: the line of CORE; GSKIP, GSKIPF: "?:" skips; XT_CATCH, XT_LIT: xts;
# BOOT_COLON, BOOT_SEMI: the first ":" and ";".
# GNT[nt] marks the headers that Genesis removes at the end.
# L[j] holds the codes of the line in TIB.

function gfail(s) {
  msg("error: " s)
  msg(FILES[0] ":" GLN ": loading stopped")
  msg(FILES[0] " is not complete; the system cannot start")
  bye(1)
}

function gerr(n, s) {               # a THROW code during Genesis
  if (s != "") gfail(throw_text(n) ": " s); else gfail(throw_text(n))
}

function pname(   i, n, st) {       # PARSE-NAME on TIB: PA PN; >IN moves
  i = M[TOIN_A]; n = M[NTIB_A]
  while (i < n && L[i] <= 32) i++
  st = i
  while (i < n && L[i] > 32) i++
  PN = i - st; PA = TIB + st
  if (i < n) i++
  M[TOIN_A] = i
}

function pparse(c,   i, n, st) {    # PARSE up to the character c: PA PN
  i = M[TOIN_A]; n = M[NTIB_A]; st = i
  while (i < n && L[i] != c) i++
  PN = i - st; PA = TIB + st
  if (i < n) i++
  M[TOIN_A] = i
}

function comma(x,   t) { t = M[DP_A]; M[t - t % C] = x % MOD; M[DP_A] = t + C }

function mkhead(name, wl, flags,   n, nt, j) {   # a visible header; the xt is the result
  n = length(name); nt = M[DP_A]; M[nt] = M[wl]
  setb(nt + C, n + flags)
  for (j = 0; j < n; j++) setb(nt + C + 1 + j, ORD[substr(name, j + 1, 1)])
  M[wl] = nt; M[LATEST_A] = nt; M[DP_A] = ntxt(nt)
  return M[DP_A]
}

function gheader(   a, n, nt, xt, j, name, r) {   # HEADER ( a u -- ): a new header in CURRENT, not visible
  n = M[sp]; a = M[sp + C]; sp += C + C
  if (n == 0) gerr(-16)
  if (n > 31) gerr(-19)
  name = memstr(a, n)
  keymem(a, n); r = wlfind(M[CURRENT_A])
  if (r && !(r in GNT)) msg("redefined " name)
  while (M[DP_A] % C) { setb(M[DP_A], 0); M[DP_A]++ }
  nt = M[DP_A]; xt = int((nt + C + 1 + n + A) / C) * C
  if (xt + 4 * C > DICT_END) gerr(-8)
  M[nt] = M[M[CURRENT_A]]
  setb(nt + C, n)
  for (j = 0; j < n; j++) setb(nt + C + 1 + j, getb(a + j))
  for (j = nt + C + 1 + n; j < xt; j++) setb(j, 0)
  M[DP_A] = xt; M[LATEST_A] = nt
  if (name == ":" && !BOOT_COLON) { BOOT_COLON = nt; GNT[nt] = 1 }
  if (name == ";" && !BOOT_SEMI) { BOOT_SEMI = nt; GNT[nt] = 1 }
}

function gnumber(s,   w, base, neg, v, d, c, j, over) {   # GNUM = the number; status 0: no number
  if (length(s) == 3 && substr(s, 1, 1) == "'" && substr(s, 3, 1) == "'") {
    GNUM = ORD[substr(s, 2, 1)]; return 1
  }
  w = s; base = M[BASE_A]; neg = 0; v = 0; over = 0
  if (substr(w, 1, 1) == "-" && index("$#%", substr(w, 2, 1)) && length(w) > 1) { neg = 1; w = substr(w, 2) }
  c = substr(w, 1, 1)
  if (c == "$") { base = 16; w = substr(w, 2) }
  else if (c == "#") { base = 10; w = substr(w, 2) }
  else if (c == "%") { base = 2; w = substr(w, 2) }
  if (substr(w, 1, 1) == "-") { neg = !neg; w = substr(w, 2) }
  if (w == "" || base < 2 || base > 36) return 0
  for (j = 1; j <= length(w); j++) {
    c = ORD[substr(w, j, 1)]
    if (c >= 48 && c <= 57) d = c - 48
    else if (c >= 65 && c <= 90) d = c - 55
    else if (c >= 97 && c <= 122) d = c - 87
    else return 0
    if (d >= base) return 0
    if (!over) { v = v * base + d; if (v > MASK) over = 1 }
  }
  if (over) gerr(-24, s)
  if (neg) v = -v
  if (v < -SIGN || v > MASK) gerr(-24, s)
  GNUM = v < 0 ? v + MOD : v
  return 1
}

function gexec(xt,   code) {        # execute xt; through CATCH once CORE has it
  if (XT_CATCH) {
    push(xt); run_xt(XT_CATCH)
    code = M[sp]; sp += C
    if (code) gerr(signed(code))
  } else run_xt(xt)
}

function gline(line,   n, j, c, word, nt, xt, fl, save, u) {   # interpret one line of CORE
  n = length(line)
  if (n > TIB_SIZE) gfail("line too long (" n " bytes, at most " TIB_SIZE ")")
  for (j = 0; j < n; j++) { c = ORD[substr(line, j + 1, 1)]; L[j] = c; setb(TIB + j, c) }
  M[NTIB_A] = n; M[TOIN_A] = 0
  while (1) {
    pname()
    if (!PN) return
    word = substr(line, PA - TIB + 1, PN)
    if (GSKIP) {                    # "?:" skips a definition
      if (word == "\\") M[TOIN_A] = M[NTIB_A]
      else if (word == "(") pparse(41)
      else if (word == ";") { GSKIP = 0; GSKIPF = 1 }
      continue
    }
    if (GSKIPF) {                   # and the flags after it
      u = toupper(word)
      if (u == "IMMEDIATE" || u == "UNCHECKED" || u == "COMPILE-ONLY") continue
      GSKIPF = 0
    }
    if (word == "?:") {
      save = M[TOIN_A]
      pname()
      if (!PN) gerr(-16)
      keymem(PA, PN)
      if (gfind()) { GSKIP = 1; continue }
      M[TOIN_A] = save
      keystr(":"); gexec(ntxt(gfind()))
      continue
    }
    keymem(PA, PN); nt = gfind()
    if (nt) {
      xt = ntxt(nt); fl = M[nt + C] % 256
      if (M[STATE_A] == 0 || fl >= 128) {
        if (M[STATE_A] == 0 && int(fl / 64) % 2) gerr(-14, word)
        gexec(xt)
      } else comma(xt)
      continue
    }
    if (word == "\\") { M[TOIN_A] = M[NTIB_A]; continue }   # comments until CORE defines them
    if (word == "(") { pparse(41); continue }
    if (!gnumber(word)) gerr(-13, word)
    if (M[STATE_A]) { comma(XT_LIT); comma(GNUM) } else push(GNUM)
  }
}

function genesis(   i, op, wl, nt, prev, nx, xt) {   # compile CORE; then remove the host primitives
  M[HALT_XT] = 127; M[HALT_ADDR] = HALT_XT
  M[DP_A] = DICT_START + GUARD; M[VP_A] = VP_START; M[BASE_A] = 10
  M[WL_FORTH + C] = 0; M[WL_INTERNALS + C] = WL_FORTH
  M[VOCLINK_A] = WL_INTERNALS; sp = S0; rp = R0
  for (i = 0; i < NPRIM; i++) {     # a header for each primitive
    op = PRIM_OP[i]
    xt = mkhead(NAME[op], VOC[op] == "F" ? WL_FORTH : WL_INTERNALS, FLAG[op])
    M[xt] = op; M[DP_A] = xt + C
    if (op == 5) XT_LIT = xt
    if (op >= 100) GNT[M[LATEST_A]] = 1
  }
  for (i = 0; i < NSV; i++) {       # constants for the system variables
    xt = mkhead(SV_NAME[i], SV_VOC[i] == "F" ? WL_FORTH : WL_INTERNALS, 0)
    M[xt] = 2; M[xt + C] = SV_VAL[i]; M[DP_A] = xt + C + C
  }
  M[NORDER_A] = 2; M[CONTEXT_A] = WL_FORTH
  M[CONTEXT_A + C] = WL_INTERNALS; M[CURRENT_A] = WL_FORTH
  if (!readlines(FILES[0])) { msg("cannot read " FILES[0]); bye(1) }
  for (GLN = 1; GLN <= FLN; GLN++) {
    gline(FL[GLN])
    if (!XT_CATCH) { keystr("CATCH"); nt = gfind(); if (nt) XT_CATCH = ntxt(nt) }
  }
  GLN = FLN
  wl = M[VOCLINK_A]                 # remove the host primitives and the first ":" ";"
  while (wl) {
    prev = 0; nt = M[wl]
    while (nt) {
      nx = M[nt]
      if (nt in GNT) { if (prev) M[prev] = nx; else M[wl] = nx }
      else prev = nt
      nt = nx
    }
    wl = M[wl + C]
  }
}

# ==== Images: Intel HEX ==================================================
# An image is a text file (doc/vm.md, section 10). The first line names
# the cell size and the start words; then come Intel HEX records for the
# system variables, the RAM data up to VP and the dictionary up to DP.

function hexseg(a, e, f,   n, j, b, sum, data, hi) {   # the bytes from a up to e (exclusive)
  while (a < e) {
    hi = int(a / 65536)
    if (hi != ULBA) {               # extended linear address: the upper 16 bits
      ULBA = hi
      printf ":02000004%04X%02X\n", hi, (256 - (6 + int(hi / 256) + hi % 256) % 256) % 256 > f
    }
    n = e - a; if (n > 16) n = 16
    if (a % 65536 + n > 65536) n = 65536 - a % 65536
    sum = n + int(a / 256) % 256 + a % 256
    data = ""
    for (j = 0; j < n; j++) { b = getb(a + j); sum += b; data = data sprintf("%02X", b) }
    printf ":%02X%04X00%s%02X\n", n, a % 65536, data, (256 - sum % 256) % 256 > f
    a += n
  }
}

function write_image(f, boot) {     # status 1 if it works
  if (!canwrite(f)) return 0
  ULBA = 0                          # upper address bits; a record only above $FFFF
  printf "; ucf image bits=%d boot=%d quit=%d throw=%d prims=%s\n", \
         BITS, boot, XT_QUIT, XT_THROW, PRIMS > f
  hexseg(SYS, DATA_START, f)
  hexseg(DATA_START, M[VP_A], f)
  hexseg(DICT_START, M[DP_A], f)
  printf ":00000001FF\n" > f
  close(f)
  return 1
}

function hex2(s, i) { return HEXV[substr(s, i, 1)] * 16 + HEXV[substr(s, i + 1, 1)] }

function hexread(f, mode,   lines, nl, k, line, n, a, t, j, b, sum, hi, r) {   # mode "load" or "check"
  NDIFF = 0; FIRSTDIFF = ""; hi = 0
  if (slurp(f) < 0) { printf "ucf.awk: cannot read %s\n", f > "/dev/stderr"; exit 1 }
  nl = split(SLURP, lines, "\n")
  for (k = 1; k <= nl; k++) {
    line = lines[k]; sub(/\r$/, "", line)
    if (substr(line, 1, 1) == ";") { if (index(line, " boot=")) META = line; continue }
    if (substr(line, 1, 1) != ":") continue
    n = hex2(line, 2); a = hex2(line, 4) * 256 + hex2(line, 6); t = hex2(line, 8)
    sum = n + int(a / 256) + a % 256 + t + hex2(line, 10 + 2 * n)
    for (j = 0; j < n; j++) sum += hex2(line, 10 + 2 * j)
    if (sum % 256) { printf "ucf.awk: %s:%d: wrong checksum\n", f, k > "/dev/stderr"; exit 1 }
    if (t == 0) {
      for (j = 0; j < n; j++) {
        b = hex2(line, 10 + 2 * j)
        if (mode == "load") setb(hi + a + j, b)
        else {
          r = getb(hi + a + j)
          if (r != b) {
            NDIFF++
            if (FIRSTDIFF == "") FIRSTDIFF = sprintf("$%X (here %d, there %d)", hi + a + j, r, b)
          }
        }
      }
    } else if (t == 1) break
    else if (t == 4) hi = (hex2(line, 10) * 256 + hex2(line, 12)) * 65536
  }
}

function metaval(key,   i, v) {     # the number of key in the first line of an image
  i = index(META, " " key "=")
  if (!i) return 0
  v = substr(META, i + length(key) + 2)
  sub(/ .*/, "", v)
  return v
}

function load_image(f,   list, n, r, i, lo, hi, op, parts) {   # memory, start words, primitives
  META = ""
  hexread(f, "load")
  if (META == "") { printf "ucf.awk: %s is not an image of uCForth\n", f > "/dev/stderr"; exit 1 }
  XT_BOOT = metaval("boot") + 0; XT_QUIT = metaval("quit") + 0; XT_THROW = metaval("throw") + 0
  list = metaval("prims")
  n = split(list, r, ",")
  for (i = 1; i <= n; i++) {        # each range lo-hi: this VM must have it
    if (split(r[i], parts, "-") == 2) { lo = parts[1] + 0; hi = parts[2] + 0 }
    else { lo = r[i] + 0; hi = lo }
    for (op = lo; op <= hi; op++)
      if (!HAVE[op]) { printf "ucf.awk: %s needs primitive %d\n", f, op > "/dev/stderr"; exit 1 }
  }
}

function check_image(name, f) {     # compare the memory with a reference
  if (!readable(f)) { msg("check " name ": cannot read " f); CHECKFAIL = 1; return }
  hexread(f, "check")
  if (NDIFF) { msg("check " name ": " NDIFF " bytes differ from " f ", the first at " FIRSTDIFF); CHECKFAIL = 1 }
  else msg("check " name ": the same as " f)
}

function save_image(   f) {         # SAVE-IMAGE ( -- ior )
  f = ENVIRON["UCF_IMAGE"]; if (f == "") f = "ucf.hex"
  if (write_image(f, XT_BOOT ? XT_BOOT : XT_QUIT)) { msg("saved: " f); push(0) }
  else push(-37)
}

# ==== Start ==============================================================

function words_in_forth(   nt, n) {
  n = 0; nt = M[WL_FORTH]
  while (nt) { n++; nt = M[nt] }
  return n
}

function core_end() {               # Genesis is complete: seed and check
  if (DUMP_CORE != "" && !write_image(DUMP_CORE, XT_QUIT)) msg("cannot write " DUMP_CORE)
  if (CHECK) check_image("core" BITS, PFX "seed/core" BITS ".hex")
}

function build_end(   here, t) {    # base is complete (doc/genesis.md, section 6)
  here = M[DP_A]; M[BASEEND_A] = here; M[FROZENEND_A] = here
  M[NORDER_A] = 1; M[CONTEXT_A] = WL_FORTH; M[CURRENT_A] = WL_FORTH
  M[STATE_A] = 0
  BUILD_END = -2
  XT_BOOT = sysxt("BOOT")
  if (DUMP != "" && !write_image(DUMP, XT_BOOT)) msg("cannot write " DUMP)
  if (CHECK) {
    OB = ""                         # the prompt of QUIT is not wanted here
    check_image("base" BITS, PFX "seed/base" BITS ".hex")
    bye(CHECKFAIL)
  }
  t = int((now_ms() - T0 + 500) / 1000)
  printf "uCForth - awk bootstrap VM - %d bit%s - %d words in FORTH - built in %d s\n", \
         BITS, PURE ? " - pure" : "", words_in_forth(), t
}

function start(   i) {
  T0 = now_ms(); CHECKFAIL = 0; OB = ""; ERRSEEN = 0
  TTY = system("test -t 0") == 0
  if (IMAGE != "") {
    load_image(IMAGE)
    for (i = 0; i < NARGS; i++) FILES[NFILES++] = ARGS[i]
    find_refill()
    printf "uCForth - awk bootstrap VM - %d bit - %d words in FORTH - image %s\n", BITS, words_in_forth(), IMAGE
    sp = S0; rp = R0; M[STATE_A] = 0; M[HANDLER_A] = 0
    run_xt(XT_BOOT ? XT_BOOT : XT_QUIT)
  } else {
    FILES[NFILES++] = CORE_FILE; FILES[NFILES++] = BASE_FILE
    for (i = 0; i < NARGS; i++) FILES[NFILES++] = ARGS[i]
    genesis()
    XT_THROW = sysxt("THROW"); XT_QUIT = sysxt("QUIT")
    if (!XT_QUIT) gfail(FILES[0] " does not define QUIT")
    find_refill()
    core_end()
    FI = 1; BUILD_END = 1
    run_xt(XT_QUIT)
  }
  bye(0)
}
