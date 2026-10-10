#!/usr/bin/env python3
# ucf.py - the Python bootstrap VM of uCForth
#
# It is the virtual machine of uCForth in Python: the memory, the
# registers, the primitives and a small HAL (doc/vm.md). Its Genesis, a
# small outer interpreter, compiles forth/core.fs once (doc/genesis.md).
# From then on QUIT in the image reads each line, as on a board: first
# forth/base.fs, then the files of the command line, then the keyboard
# (or a pipe).
#
# The parts and their names are the same as in the C bootstrap VM
# (bootstrap/c/ucf.c, the reference). The README in this folder tells
# what is special in Python.
#
# Use:  python3 bootstrap/python/ucf.py [OPTION ...] [FILE ...]
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
# in the environment variable UCF_IMAGE. Ctrl-C stops the word that runs.
# Python 3.6 or newer, only the standard library.
#
# Limits:
# - ?RX gets whole lines: the terminal sends a line with the return key.
# - No checks of the stacks. An address wraps around in the memory of
#   the VM (64 KB or 256 KB), so no address reaches other memory.
# - REBOOT starts BOOT or QUIT again, but does not load the image again.
#   ERASE-IMAGE does nothing.
# - OPEN-FILE with W/O or R/W writes at the end of the file.
# - TX! does not send the byte 0.

import os
import signal
import sys
import time

# ==== The system =========================================================
# The clock, a wait, the test for a keyboard, and Ctrl-C. Python has
# them on each system, so this part is the same for POSIX and Windows.

brk = False                             # Ctrl-C came: stop the word that runs
reading = False                         # the program waits for a line from the keyboard


def now_ms():
    return int(time.monotonic() * 1000)


def wait_us(us):
    time.sleep(us / 1000000)


def is_tty():
    return sys.stdin.isatty()


def on_int(sig, frame):
    global brk
    brk = True
    if reading:                         # Ctrl-C also ends the wait for a line
        raise KeyboardInterrupt


def catch_int():
    signal.signal(signal.SIGINT, on_int)


# ==== Options ============================================================
# The default files lie in the project. The program looks for them in the
# current folder (ucf), then two folders above (a start in bootstrap/python).

BITS = 16                               # the cell size
pure = check = False                    # -pure, -check
core_file = base_file = image_file = dump_file = dump_core_file = None
pfx = ''                                # '' or '../../': where the project is
args = []                               # the files of the command line


def readable(name):
    try:
        with open(name, 'rb'):
            return True
    except OSError:
        return False


def usage():
    sys.stderr.write('use: python3 ucf.py [-32] [-pure] [-core FILE] [-base FILE] [-image FILE]\n'
                     '                    [-dump-core FILE] [-dump FILE] [-check] [FILE ...]\n')
    sys.exit(2)


def options(argv):
    global BITS, pure, check, core_file, base_file, image_file, dump_file, dump_core_file, pfx
    if not readable('forth/core.fs') and readable('../../forth/core.fs'):
        pfx = '../../'
    core_file = pfx + 'forth/core.fs'
    base_file = pfx + 'forth/base.fs'
    i = 1
    while i < len(argv):
        a = argv[i]
        if a == '--':
            pass
        elif a == '-32':
            BITS = 32
        elif a == '-pure':
            pure = True
        elif a == '-check':
            check = pure = True
        elif a in ('-core', '-base', '-image', '-dump-core', '-dump'):
            if i + 1 >= len(argv):
                usage()
            i += 1
            if a == '-core':
                core_file = argv[i]
            elif a == '-base':
                base_file = argv[i]
            elif a == '-image':
                image_file = argv[i]
            elif a == '-dump-core':
                dump_core_file = argv[i]
            else:
                dump_file = argv[i]
        elif a.startswith('-'):
            usage()
        else:
            args.append(a)
        i += 1
    if image_file:                      # the first line of an image names the cell size
        text = slurp(image_file)
        if text is None:
            sys.stderr.write('ucf.py: cannot read %s\n' % image_file)
            sys.exit(1)
        BITS = 32 if b'bits=32' in text else 16


# ==== Memory map (doc/vm.md, section 3) ==================================
# The memory of the VM is a bytearray. A cell is 2 or 4 bytes, the low
# byte first (little endian).

mem = bytearray()                       # the memory: 64 KB or 256 KB (memory_map)


def memory_map():
    global C, MASK, SIGN, AMASK, SYS, DATA_START, DATA_END, TIB, PAD, R0, S0
    global DICT_START, GUARD, DICT_END, MEM_END, HALT_XT, HALT_ADDR, RUN_ADDR, THROW_ADDR
    global WL_FORTH, WL_INTERNALS, VP_START, get, put, mem
    global DP_A, VP_A, LATEST_A, STATE_A, BASE_A, TOIN_A, NTIB_A, CURRENT_A, NORDER_A
    global CONTEXT_A, VOCLINK_A, CASESENSE_A, BASEEND_A, FROZENEND_A, HANDLER_A
    k = 1 if BITS == 16 else 2
    C = BITS // 8                       # bytes in a cell: 2 or 4
    MASK = (1 << BITS) - 1              # the biggest cell
    SIGN = 1 << (BITS - 1)              # the top bit of a cell
    SYS = 256
    DATA_START = 512 * k
    DATA_END = 4096 if k == 1 else 65536
    TIB = DATA_END
    PAD = TIB + 256
    R0 = PAD + 256 + 256 * C            # the return stack: 256 cells below R0
    S0 = R0 + 256 * C                   # the data stack: 256 cells below S0
    DICT_START = S0
    GUARD = 8 * C                       # the first word comes 8 cells above S0:
                                        # a stack underflow lands in these free cells
    MEM_END = 65536 if k == 1 else 262144
    DICT_END = MEM_END - 16
    AMASK = MEM_END - 1                 # addresses wrap around
    HALT_XT = DATA_START - 16           # the two HALT cells
    HALT_ADDR = HALT_XT + C
    RUN_ADDR = MEM_END - 2 * C          # two cells at the top: run one word, then HALT
    THROW_ADDR = MEM_END - 4 * C        # two cells at the top: run THROW
    WL_FORTH = DATA_START
    WL_INTERNALS = WL_FORTH + 3 * C
    VP_START = WL_INTERNALS + 3 * C
    mem = bytearray(MEM_END + 4)        # 4 more bytes: a cell at the end needs no test
    # the system variables (doc/vm.md, section 4), one cell each from SYS
    DP_A, VP_A, LATEST_A, STATE_A, BASE_A, TOIN_A, NTIB_A, CURRENT_A, NORDER_A = \
        (SYS + i * C for i in range(9))
    CONTEXT_A = SYS + 9 * C
    VOCLINK_A, CASESENSE_A, BASEEND_A, FROZENEND_A = (SYS + i * C for i in range(25, 29))
    HANDLER_A = SYS + 32 * C
    get, put = (get2, put2) if C == 2 else (get4, put4)


# The names of the system variables for Genesis: name, cells, vocabulary
# (F = FORTH, I = INTERNALS). The addresses follow one after the other.
SV = [('DP', 1, 'I'), ('VP', 1, 'I'), ('LATEST', 1, 'I'), ('STATE', 1, 'I'),
      ('BASE', 1, 'F'), ('>IN', 1, 'I'), ('#TIB', 1, 'I'), ('CURRENT', 1, 'I'),
      ('#ORDER', 1, 'I'), ('CONTEXT', 16, 'I'), ('VOC-LINK', 1, 'I'),
      ('CASE-SENSE', 1, 'I'), ('BASE-END', 1, 'I'), ('FROZEN-END', 1, 'I'),
      ('AUTORUN', 1, 'I'), ('ERRMSG', 2, 'I'), ('HANDLER', 1, 'I')]

# ==== Primitives =========================================================
# Number, name, vocabulary, flags (64 = compile-only, 128 = immediate).
# 0-3 are the code field types, 4-31 the other mandatory primitives,
# 32-65 the optional ones, 80-88 the HAL, 100-108 exist only for Genesis.

PRIM = [(4, 'EXIT', 'F', 64), (5, 'LIT', 'I', 64), (6, 'BRANCH', 'I', 64),
        (7, '?BRANCH', 'I', 64), (8, 'EXECUTE', 'F', 0), (9, '@', 'F', 0),
        (10, '!', 'F', 0), (11, 'C@', 'F', 0), (12, 'C!', 'F', 0),
        (13, 'DUP', 'F', 0), (14, 'DROP', 'F', 0), (15, 'SWAP', 'F', 0),
        (16, 'OVER', 'F', 0), (17, '>R', 'F', 64), (18, 'R>', 'F', 64),
        (19, 'R@', 'F', 64), (20, 'SP@', 'I', 0), (21, 'SP!', 'I', 0),
        (22, 'RP@', 'I', 0), (23, 'RP!', 'I', 0), (24, 'AND', 'F', 0),
        (25, 'OR', 'F', 0), (26, 'XOR', 'F', 0), (27, '0<', 'F', 0),
        (28, 'UM+', 'I', 0), (29, '?RX', 'I', 0), (30, 'TX!', 'I', 0),
        (31, '!IO', 'I', 0),
        (32, '+', 'F', 0), (33, '-', 'F', 0), (34, '1+', 'F', 0),
        (35, '1-', 'F', 0), (36, 'NEGATE', 'F', 0), (37, 'INVERT', 'F', 0),
        (38, '2*', 'F', 0), (39, '2/', 'F', 0), (40, 'LSHIFT', 'F', 0),
        (41, 'RSHIFT', 'F', 0), (42, '=', 'F', 0), (43, '0=', 'F', 0),
        (44, '<', 'F', 0), (45, 'U<', 'F', 0), (46, '*', 'F', 0),
        (47, 'UM*', 'F', 0), (48, 'UM/MOD', 'F', 0), (49, 'ROT', 'F', 0),
        (50, 'NIP', 'F', 0), (51, '?DUP', 'F', 0), (52, '2DUP', 'F', 0),
        (53, '2DROP', 'F', 0), (54, '+!', 'F', 0), (55, 'MOVE', 'F', 0),
        (56, 'FILL', 'F', 0), (57, 'COUNT', 'F', 0), (58, 'FIND-NAME', 'I', 0),
        (59, '(DO)', 'I', 64), (60, '(?DO)', 'I', 64), (61, '(LOOP)', 'I', 64),
        (62, '(+LOOP)', 'I', 64), (63, 'I', 'F', 64), (64, 'J', 'F', 64),
        (65, 'UNLOOP', 'F', 64),
        (80, 'MILLIS', 'F', 0), (81, 'MS', 'F', 0), (82, 'US', 'F', 0),
        (83, 'BIS!', 'F', 0), (84, 'BIC!', 'F', 0), (85, 'SAVE-IMAGE', 'I', 0),
        (86, 'ERASE-IMAGE', 'I', 0), (87, 'REBOOT', 'I', 0), (88, 'BYE', 'F', 0),
        (100, 'PARSE-NAME', 'I', 0), (101, 'PARSE', 'I', 0), (102, 'HEADER', 'I', 0),
        (103, 'REVEAL', 'I', 0), (104, 'IMMEDIATE', 'F', 0), (105, ',', 'F', 0),
        (106, 'C,', 'F', 0), (107, '[', 'F', 128), (108, ']', 'F', 0)]

prims = ''                              # the primitives with a header, as "0-31,80-88"


def with_header(op):                    # does Genesis make a header for op?
    return op < 32 or op >= 80 or not pure


def have(op):                           # can this VM run op? An image may use only these.
    return op <= 65 or 80 <= op <= 93 or op in (98, 99)


def primitives():                       # prims: the numbers below 100 with a header, as ranges
    global prims
    ops = [0, 1, 2, 3] + [op for op, _, _, _ in PRIM if op < 100 and with_header(op)]
    ranges, first = [], ops[0]
    for i in range(1, len(ops) + 1):
        if i < len(ops) and ops[i] == ops[i - 1] + 1:
            continue
        prev = ops[i - 1]
        ranges.append(str(first) if first == prev else '%d-%d' % (first, prev))
        if i < len(ops):
            first = ops[i]
    prims = ','.join(ranges)


# ==== Helpers ============================================================
# The access to the memory, the stacks, and the messages of this program.


def getb(a):
    return mem[a & AMASK]


def setb(a, b):
    mem[a & AMASK] = b & 255


def get2(a):                            # the cell at a: the low byte first
    a &= AMASK
    return mem[a] | mem[a + 1] << 8


def get4(a):
    a &= AMASK
    return mem[a] | mem[a + 1] << 8 | mem[a + 2] << 16 | mem[a + 3] << 24


def put2(a, x):                         # store the cell x at a
    a &= AMASK
    mem[a] = x & 255
    mem[a + 1] = x >> 8 & 255


def put4(a, x):
    a &= AMASK
    mem[a] = x & 255
    mem[a + 1] = x >> 8 & 255
    mem[a + 2] = x >> 16 & 255
    mem[a + 3] = x >> 24 & 255


ip = w = sp = rp = 0                    # the registers (doc/vm.md, section 5)


def push(x):
    global sp
    sp -= C
    put(sp, x & MASK)


def pop():
    global sp
    x = get(sp)
    sp += C
    return x


def sgn(x):                             # a cell as a signed number
    return x - (MASK + 1) if x & SIGN else x


def ntxt(nt):                           # the xt of the header nt
    return (nt + C + 1 + (getb(nt + C) & 31) + C - 1) // C * C


out = sys.stdout.buffer                 # the output: bytes, not text
ob = bytearray()                        # the output line that does not show yet


def msg(s):                             # a message of this program on its own line
    if ob:
        out.write(ob + b'\n')
        ob.clear()
    out.write(b'  ' + s.encode('utf-8', 'replace') + b'\n')


def bye(n):
    if ob:
        out.write(ob + b'\n')
    out.flush()
    sys.exit(n)


THROW_TEXT = {-3: 'data stack overflow', -4: 'data stack underflow', -8: 'dictionary overflow',
              -10: 'division by zero', -11: 'result out of range', -13: 'undefined word',
              -14: 'compile-only word', -16: 'missing name', -19: 'name too long',
              -21: 'unsupported operation', -24: 'number out of range'}


def throw_text(n):
    return THROW_TEXT.get(n, 'error %d' % n)


# ==== Search in the dictionary ===========================================
# key holds the name; without CASE-SENSE in upper case.

key = b''


def upper(c, cs):                       # a to z as A to Z, without CASE-SENSE
    return c - 32 if not cs and 97 <= c <= 122 else c


def keymem(a, n):                       # the key from memory
    global key
    cs = get(CASESENSE_A)
    key = bytes(upper(getb(a + j), cs) for j in range(min(n, 32)))
    if n > 32:
        key += b'?' * (n - 32)          # too long for a name: no match


def keystr(s):                          # the key from a string
    global key
    cs = get(CASESENSE_A)
    key = bytes(upper(c, cs) for c in s.encode('latin-1'))


def wlfind(wl):                         # the nt of the key in the wordlist wl, or 0
    cs = get(CASESENSE_A)
    kn = len(key)
    nt = get(wl)
    while nt:
        if getb(nt + C) & 31 == kn:
            a = nt + C + 1
            for j in range(kn):
                if upper(getb(a + j), cs) != key[j]:
                    break
            else:
                return nt
        nt = get(nt)
    return 0


def gfind():                            # the search order, the top first: the nt or 0
    if not 0 < len(key) <= 31:
        return 0
    for i in range(get(NORDER_A) - 1, -1, -1):
        nt = wlfind(get(CONTEXT_A + i * C))
        if nt:
            return nt
    return 0


def sysxt(s):                           # the xt of the oldest word with exactly this name
    name = s.encode('latin-1')
    for wl in (WL_FORTH, WL_INTERNALS):
        found = 0
        nt = get(wl)
        while nt:
            if getb(nt + C) & 31 == len(name) and \
                    all(getb(nt + C + 1 + j) == name[j] for j in range(len(name))):
                found = nt
            nt = get(nt)
        if found:
            return ntxt(found)
    return 0


# ==== The virtual machine ================================================
# ip, w, sp and rp are the registers. SP and RP point to the top cell; the
# stacks grow down. A flag is MASK (true) or 0 (false). Each primitive is
# a function; P[op] is the function of the primitive op.

XT_THROW = XT_QUIT = XT_BOOT = 0        # from the image
XT_CATCH = XT_LIT = 0                   # for Genesis
OUT_A = 0                               # the address of OUT, the column of the output


def throw_(n):                          # THROW n in the image: the next word is THROW
    global ip
    if not XT_THROW:
        gfail('%s (no THROW yet)' % throw_text(n))
    push(n)
    put(THROW_ADDR, XT_THROW)
    put(THROW_ADDR + C, HALT_XT)
    ip = THROW_ADDR


def run_xt(xt):                         # execute xt; return when it is complete
    run_later(xt)
    vm()


def run_later(xt):                      # the VM goes on with xt, then HALT
    global ip
    put(RUN_ADDR, xt)
    put(RUN_ADDR + C, HALT_XT)
    ip = RUN_ADDR


in_str = b''                            # the current input line, with LF
in_pos = 0


def do_break():                         # Ctrl-C between two primitives (doc/vm.md, section 9)
    global brk, in_pos, sp, rp
    brk = False
    in_pos = len(in_str)                # the rest of the input line goes
    out.write(ob + b'\n')               # a new line
    out.flush()
    ob.clear()
    if OUT_A:
        put(OUT_A, 0)
    if get(HANDLER_A):
        throw_(-28)                     # a CATCH gets "user interrupt"
        return
    sp, rp = S0, R0                     # else: empty stacks, then QUIT
    put(STATE_A, 0)
    run_later(XT_QUIT)


def vm():                               # NEXT and the primitives, until primitive 127 (HALT) comes
    global ip, w
    fetch, table, cell = get, P, C      # local names: Python finds them faster
    while True:
        if brk:
            do_break()
        w = fetch(ip)                   # NEXT
        ip += cell
        op = fetch(w)
        while op == 8:                  # EXECUTE
            w = pop()
            op = fetch(w)
        if op == 127:                   # HALT: back to Python
            return
        if op < 66:
            table[op]()
        else:
            hal(op)


def p_docol():
    global rp, ip
    rp -= C
    put(rp, ip)
    ip = w + C


def p_dodoes():
    global rp, ip
    push(w + 2 * C)
    rp -= C
    put(rp, ip)
    ip = get(w + C)


def p_exit():
    global rp, ip
    ip = get(rp)
    rp += C


def p_lit():
    global ip
    push(get(ip))
    ip += C


def p_branch():
    global ip
    ip = get(ip)


def p_qbranch():
    global ip
    if pop():
        ip += C
    else:
        ip = get(ip)


def p_store():
    a = pop()
    put(a, pop())


def p_cstore():
    a = pop()
    setb(a, pop())


def p_drop():
    global sp
    sp += C


def p_swap():
    x = get(sp)
    put(sp, get(sp + C))
    put(sp + C, x)


def p_tor():
    global rp
    x = pop()
    rp -= C
    put(rp, x)


def p_rfrom():
    global rp
    push(get(rp))
    rp += C


def p_spstore():
    global sp
    sp = get(sp)


def p_rpstore():
    global rp
    rp = pop()


def p_umplus():                         # UM+ ( u1 u2 -- u3 c )
    x = pop()
    t = get(sp) + x
    put(sp, t & MASK)
    push(1 if t > MASK else 0)


def binary(f):                          # a primitive ( x1 x2 -- x3 ) from a function f(x1, x2)
    def p():
        x = pop()
        put(sp, f(get(sp), x) & MASK)
    return p


def unary(f):                           # a primitive ( x1 -- x2 ) from a function f(x1)
    def p():
        put(sp, f(get(sp)) & MASK)
    return p


def flag(f):
    return MASK if f else 0


def p_umstar():                         # UM* ( u1 u2 -- ud ): Python has big numbers
    t = get(sp + C) * get(sp)
    put(sp + C, t & MASK)
    put(sp, t >> BITS)


def p_ummod():                          # UM/MOD ( ud u -- rem quot )
    u = pop()
    hi, lo = get(sp), get(sp + C)
    if u == 0:
        throw_(-10)
        return
    if hi >= u:                         # the quotient would not fit into a cell
        throw_(-11)
        return
    q, r = divmod(hi << BITS | lo, u)
    put(sp + C, r)
    put(sp, q)


def p_rot():                            # ROT ( x1 x2 x3 -- x2 x3 x1 )
    x = get(sp + 2 * C)
    put(sp + 2 * C, get(sp + C))
    put(sp + C, get(sp))
    put(sp, x)


def p_nip():
    x = pop()
    put(sp, x)


def p_qdup():
    x = get(sp)
    if x:
        push(x)


def p_2dup():
    x, y = get(sp), get(sp + C)
    push(y)
    push(x)


def p_2drop():
    global sp
    sp += 2 * C


def p_plusstore():                      # +! ( n a -- )
    a = pop()
    put(a, (get(a) + pop()) & MASK)


def p_move():                           # MOVE ( a1 a2 u -- ), also for areas that overlap
    u, a2, a1 = pop(), pop(), pop()
    order = range(u - 1, -1, -1) if a1 < a2 else range(u)
    for j in order:
        setb(a2 + j, getb(a1 + j))


def p_fill():                           # FILL ( a u c -- )
    c, u, a = pop() & 255, pop(), pop()
    for j in range(u):
        setb(a + j, c)


def p_count():                          # COUNT ( a -- a+1 c )
    a = get(sp)
    put(sp, (a + 1) & MASK)
    push(getb(a))


def p_findname():                       # FIND-NAME ( a u -- nt | 0 )
    u = pop()
    a = get(sp)
    if u == 0 or u > 31:
        put(sp, 0)
    else:
        keymem(a, u)
        put(sp, gfind())


def p_do():                             # (DO) ( limit index -- ) R: ( -- a limit index )
    global rp, sp, ip
    rp -= 3 * C
    put(rp + 2 * C, get(ip))
    put(rp + C, get(sp + C))
    put(rp, get(sp))
    sp += 2 * C
    ip += C


def p_qdo():                            # (?DO): no loop if index = limit
    global sp, ip
    if get(sp) == get(sp + C):
        ip = get(ip)
        sp += 2 * C
    else:
        p_do()


def p_loop():                           # (LOOP)
    global rp, ip
    t = (get(rp) + 1) & MASK
    if t == get(rp + C):
        rp += 3 * C
        ip += C
    else:
        put(rp, t)
        ip = get(ip)


def p_plusloop():                       # (+LOOP): the end when the index crosses limit-1 | limit
    global rp, ip
    n = pop()
    x = (get(rp) - get(rp + C)) & MASK  # index - limit
    y = (x + n) & MASK                  # the same after the step
    put(rp, (get(rp) + n) & MASK)
    if (x ^ y) & (x ^ n) & SIGN:
        rp += 3 * C
        ip += C
    else:
        ip = get(ip)


def p_unloop():
    global rp
    rp += 3 * C


def p_spfetch():                        # SP@: the value of SP before
    push(sp)


P = [None] * 66                         # the functions of the primitives 0 to 65


def make_table():
    P[0] = p_docol
    P[1] = lambda: push(w + 2 * C)                                # DOVAR
    P[2] = lambda: push(get(w + C))                               # DOCON
    P[3] = p_dodoes
    P[4] = p_exit
    P[5] = p_lit
    P[6] = p_branch
    P[7] = p_qbranch
    P[9] = lambda: put(sp, get(get(sp)))                          # @
    P[10] = p_store                                               # !
    P[11] = lambda: put(sp, getb(get(sp)))                        # C@
    P[12] = p_cstore                                              # C!
    P[13] = lambda: push(get(sp))                                 # DUP
    P[14] = p_drop
    P[15] = p_swap
    P[16] = lambda: push(get(sp + C))                             # OVER
    P[17] = p_tor
    P[18] = p_rfrom
    P[19] = lambda: push(get(rp))                                 # R@
    P[20] = p_spfetch
    P[21] = p_spstore
    P[22] = lambda: push(rp)                                      # RP@
    P[23] = p_rpstore
    P[24] = binary(lambda x, y: x & y)                            # AND
    P[25] = binary(lambda x, y: x | y)                            # OR
    P[26] = binary(lambda x, y: x ^ y)                            # XOR
    P[27] = unary(lambda x: flag(x & SIGN))                       # 0<
    P[28] = p_umplus
    P[29] = rx                                                    # ?RX
    P[30] = tx                                                    # TX!
    P[31] = lambda: None                                          # !IO
    P[32] = binary(lambda x, y: x + y)                            # +
    P[33] = binary(lambda x, y: x - y)                            # -
    P[34] = unary(lambda x: x + 1)                                # 1+
    P[35] = unary(lambda x: x - 1)                                # 1-
    P[36] = unary(lambda x: -x)                                   # NEGATE
    P[37] = unary(lambda x: ~x)                                   # INVERT
    P[38] = unary(lambda x: x << 1)                               # 2*
    P[39] = unary(lambda x: x >> 1 | x & SIGN)                    # 2/
    P[40] = binary(lambda x, u: 0 if u >= BITS else x << u)       # LSHIFT
    P[41] = binary(lambda x, u: 0 if u >= BITS else x >> u)       # RSHIFT
    P[42] = binary(lambda x, y: flag(x == y))                     # =
    P[43] = unary(lambda x: flag(x == 0))                         # 0=
    P[44] = binary(lambda x, y: flag(sgn(x) < sgn(y)))            # <
    P[45] = binary(lambda x, y: flag(x < y))                      # U<
    P[46] = binary(lambda x, y: x * y)                            # *
    P[47] = p_umstar
    P[48] = p_ummod
    P[49] = p_rot
    P[50] = p_nip
    P[51] = p_qdup
    P[52] = p_2dup
    P[53] = p_2drop
    P[54] = p_plusstore
    P[55] = p_move
    P[56] = p_fill
    P[57] = p_count
    P[58] = p_findname
    P[59] = p_do
    P[60] = p_qdo
    P[61] = p_loop
    P[62] = p_plusloop
    P[63] = lambda: push(get(rp))                                 # I
    P[64] = lambda: push(get(rp + 3 * C))                         # J
    P[65] = p_unloop
    for op in range(66):
        if P[op] is None:
            P[op] = lambda op=op: hal(op)


def reboot():                           # REBOOT ( f -- ): empty stacks, then BOOT or QUIT
    global sp, rp
    f = get(sp)
    sp, rp = S0, R0
    put(STATE_A, 0)
    put(HANDLER_A, 0)
    run_later(XT_BOOT if f and XT_BOOT else XT_QUIT)


# ==== Terminal: input lines for ?RX, output for TX! ======================
# The input comes from the files of the command line, then from the
# keyboard or a pipe. A line of a file hides the prompt of QUIT.


def readlines(name):                    # the lines of a file (doc/genesis.md, section 2), or None
    text = slurp(name)
    if text is None:
        return None
    lines = text.split(b'\n')           # the text after the last LF is a line too
    return [line[:-1] if line.endswith(b'\r') else line for line in lines]


files = []                              # core, base (not with -image), then the files of the command line
fi = 0                                  # the next file to open
curfile = None                          # the file that loads now
curln = 0                               # its line number
cf = []                                 # its lines
build_at = -1                           # after the file with this number, the system is complete
errseen = False                         # a line with "error:" came
tty = False                             # the input comes from a keyboard


def setline(s):                         # the next line for ?RX
    global in_str, in_pos
    if s.endswith(b'\r'):
        s = s[:-1]
    in_str = s + b'\n'
    in_pos = 0


def read_line():                        # a line from stdin without its LF, or None at the end
    global reading, brk
    while True:
        reading = True
        try:
            line = sys.stdin.buffer.readline()
        except KeyboardInterrupt:       # Ctrl-C at the keyboard: a new line, the prompt again
            reading = brk = False
            if tty:
                out.write(b'\n' + prompt)
                out.flush()
            continue
        reading = brk = False
        if not line:
            return None
        return line[:-1] if line.endswith(b'\n') else line


prompt = b''                            # what showed before the keyboard: the prompt


def next_line():                        # the next line for ?RX
    global curfile, curln, cf, fi, errseen, prompt
    while True:
        if curfile and errseen:         # an error stops all files
            msg('%s:%d: loading stopped' % (curfile, curln))
            curfile = None
            if build_at >= 0 and fi <= build_at + 1:
                fi = build_at + 1
                build_end()
            fi = len(files)
        errseen = False
        if curfile:
            if curln < len(cf):
                curln += 1
                ob.clear()              # the prompt of QUIT does not show
                setline(cf[curln - 1])
                return
            curfile = None
            if fi == build_at + 1:
                build_end()
            continue
        if fi < len(files):
            curfile = files[fi]
            fi += 1
            curln = 0
            cf = readlines(curfile)
            if cf is None:
                msg('cannot read %s' % curfile)
                curfile = None
                fi = len(files)
            continue
        if tty:                         # the keyboard: the terminal shows the line
            prompt = bytes(ob)
            out.write(ob)
            ob.clear()
            out.flush()
            line = read_line()
            if line is None:
                out.write(b'\n')
                bye(0)
        else:                           # a pipe: the line shows after the prompt
            out.flush()
            line = read_line()
            if line is None:
                bye(0)
            out.write(ob + line + b'\n')
            ob.clear()
        setline(line)
        return


XT_REFILL = REFILL_END = 0              # the body of REFILL: QUIT waits for a line there


def refilling():                        # True if a return address points into REFILL
    r = rp
    while r < rp + 8 * C and r < R0:
        if XT_REFILL < get(r) < REFILL_END:
            return True
        r += C
    return False


def rx():                               # ?RX ( -- c true | false )
    global in_pos
    if in_pos >= len(in_str):
        if not refilling() and (curfile or fi < len(files) or not tty):
            push(0)                     # a word reads keys while files load: no key (doc/vm.md 9)
            return
        next_line()                     # QUIT waits in REFILL, or a word waits at the keyboard
    push(in_str[in_pos])
    in_pos += 1
    push(MASK)


def bodyend(xt):                        # the next header in memory after xt
    best = MEM_END
    wl = get(VOCLINK_A)
    while wl:
        nt = get(wl)
        while nt:
            if xt < nt < best:
                best = nt
            nt = get(nt)
        wl = get(wl + C)
    return best


def find_words():                       # the words of the image that this program needs
    global XT_REFILL, REFILL_END, OUT_A
    XT_REFILL = sysxt('REFILL')
    REFILL_END = bodyend(XT_REFILL)
    xt = sysxt('OUT')
    OUT_A = get(xt + C) if xt else 0


def tx():                               # TX! ( c -- )
    global errseen
    c = pop() & 255
    if c == 10:
        out.write(ob + b'\n')
        out.flush()                     # show the line now, also while a word runs
        if b'error:' in ob:
            errseen = True
        ob.clear()
    elif c:
        ob.append(c)


# ==== Files: HAL 89-93, 98, 99 (doc/vm.md, section 8) ====================
# A file id (fid) is a number from 10 to 29. OPEN-FILE reads the whole
# file into memory; READ-LINE and READ-FILE take the bytes from there.
# WRITE-FILE writes into the file at once: CREATE-FILE makes it new,
# OPEN-FILE with W/O or R/W writes at its end.


class File:
    def __init__(self, out, buf):
        self.out = out                  # where WRITE-FILE writes, or None
        self.buf = buf                  # the file as OPEN-FILE found it
        self.pos = 0                    # the next byte to read


fds = [None] * 20


def memstr(a, n):                       # n bytes at a
    return bytes(getb(a + j) for j in range(n))


def ret2(x, y):
    put(sp + C, x & MASK)
    put(sp, y & MASK)


def ret3(x, y, z):
    put(sp + 2 * C, x & MASK)
    put(sp + C, y & MASK)
    put(sp, z & MASK)


def fid(x):                             # the index of an open fid, or -1
    return x - 10 if 10 <= x < 30 and fds[x - 10] else -1


def fopen_(create):                     # OPEN-FILE, CREATE-FILE ( a u fam -- fid ior )
    fam = pop() & 3
    name = memstr(get(sp + C), get(sp))
    k = next((i for i in range(20) if not fds[i]), -1)
    if k < 0:
        ret2(0, -37)
        return
    try:
        if create:
            try:
                os.remove(name)         # a new file, not the old one shorter: macOS keeps
            except OSError:             # the signature of a program for each file
                pass
            f = File(open(name, 'wb'), b'')
        else:
            buf = b''
            if fam != 1:
                buf = slurp(name)
                if buf is None:
                    ret2(0, -38)
                    return
            f = File(open(name, 'ab') if fam else None, buf)
    except OSError:
        ret2(0, -37)
        return
    fds[k] = f
    ret2(k + 10, 0)


def fclose_():                          # CLOSE-FILE ( fid -- ior )
    k = fid(get(sp))
    if k < 0:
        put(sp, -37 & MASK)
        return
    if fds[k].out:
        fds[k].out.close()
    fds[k] = None
    put(sp, 0)


def freadline():                        # READ-LINE ( a u fid -- u2 f ior )
    k = fid(get(sp))
    u, a = get(sp + C), get(sp + 2 * C)
    if k < 0:
        ret3(0, 0, -37)
        return
    f = fds[k]
    if f.pos >= len(f.buf):             # the end of the file
        ret3(0, 0, 0)
        return
    e = f.buf.find(b'\n', f.pos)
    if e < 0:
        e = len(f.buf)
    n = e - f.pos
    if n and f.buf[e - 1] == 13:        # CR LF ends a line too
        n -= 1
    if n > u:                           # the buffer is full: the rest of the line comes next
        n = u
        e = f.pos + u - 1
    for j in range(n):
        setb(a + j, f.buf[f.pos + j])
    f.pos = e + 1
    ret3(n, MASK, 0)


def fread_():                           # READ-FILE ( a u fid -- u2 ior )
    k = fid(pop())
    u, a = get(sp), get(sp + C)
    if k < 0:
        ret2(0, -37)
        return
    f = fds[k]
    data = f.buf[f.pos:f.pos + u]
    for j in range(len(data)):
        setb(a + j, data[j])
    f.pos += len(data)
    ret2(len(data), 0)


def fwrite_():                          # WRITE-FILE ( a u fid -- ior )
    k = fid(pop())
    u = pop()
    a = get(sp)
    if k < 0 or not fds[k].out:
        put(sp, -37 & MASK)
        return
    try:
        fds[k].out.write(memstr(a, u))
        put(sp, 0)
    except OSError:
        put(sp, -37 & MASK)


def slurp(name):                        # the whole file as bytes, or None
    try:
        with open(name, 'rb') as f:
            return f.read()
    except OSError:
        return None


# ==== Genesis: Python compiles core.fs (doc/genesis.md) ==================
# The outer interpreter: find the word, then execute or compile it; else
# convert a number. PARSE-NAME, PARSE, HEADER, REVEAL, IMMEDIATE, ",",
# "C,", "[" and "]" are host primitives until core.fs defines Forth
# versions. Genesis handles "?:", "\" and "(" itself until core.fs has them.

gln = 0                                 # the line of core.fs
gnt = set()                             # the headers that Genesis removes at the end
boot_colon = boot_semi = 0              # the first ":" and ";" (line 1 and 2 of core.fs)
gskip = gskipf = False                  # "?:" skips a definition, then its flags
pa = pn = 0                             # the word that PARSE-NAME or PARSE found


def gfail(s):
    core = files[0] if files else core_file
    msg('error: ' + s)
    msg('%s:%d: loading stopped' % (core, gln))
    msg('%s is not complete; the system cannot start' % core)
    bye(1)


def gerr(n, word=''):                   # a THROW code while Genesis runs
    gfail('%s: %s' % (throw_text(n), word) if word else throw_text(n))


def pname():                            # PARSE-NAME on TIB: pa pn; >IN moves
    global pa, pn
    i, n = get(TOIN_A), get(NTIB_A)
    while i < n and getb(TIB + i) <= 32:
        i += 1
    st = i
    while i < n and getb(TIB + i) > 32:
        i += 1
    pa, pn = TIB + st, i - st
    if i < n:
        i += 1
    put(TOIN_A, i)


def pparse(c):                          # PARSE up to the character c: pa pn
    global pa, pn
    i, n = get(TOIN_A), get(NTIB_A)
    st = i
    while i < n and getb(TIB + i) != c:
        i += 1
    pa, pn = TIB + st, i - st
    if i < n:
        i += 1
    put(TOIN_A, i)


def comma(x):                           # , ( x -- )
    t = get(DP_A)
    put(t - t % C, x)
    put(DP_A, t + C)


def mkhead(name, wl, flags):            # a visible header; the result is the xt
    n = len(name)
    nt = get(DP_A)
    put(nt, get(wl))
    setb(nt + C, n + flags)
    for j, c in enumerate(name.encode('latin-1')):
        setb(nt + C + 1 + j, c)
    put(wl, nt)
    put(LATEST_A, nt)
    put(DP_A, ntxt(nt))
    return get(DP_A)


def gheader():                          # HEADER ( a u -- ): a new header in CURRENT, not visible yet
    global boot_colon, boot_semi
    n, a = pop(), pop()
    if n == 0:
        gerr(-16)
    if n > 31:
        gerr(-19)
    name = memstr(a, n)
    keymem(a, n)
    r = wlfind(get(CURRENT_A))
    if r and r not in gnt:
        msg('redefined ' + name.decode('latin-1'))
    while get(DP_A) % C:
        setb(get(DP_A), 0)
        put(DP_A, get(DP_A) + 1)
    nt = get(DP_A)
    xt = (nt + C + 1 + n + C - 1) // C * C
    if xt + 4 * C > DICT_END:
        gerr(-8)
    put(nt, get(get(CURRENT_A)))        # the link: the newest word of CURRENT
    setb(nt + C, n)
    for j in range(n):
        setb(nt + C + 1 + j, name[j])
    for j in range(nt + C + 1 + n, xt):
        setb(j, 0)
    put(DP_A, xt)
    put(LATEST_A, nt)
    if name == b':' and not boot_colon:
        boot_colon = nt
        gnt.add(nt)
    if name == b';' and not boot_semi:
        boot_semi = nt
        gnt.add(nt)


def gnumber(s):                         # the value of the word s, or None if it is no number
    if len(s) == 3 and s[0] == 39 and s[2] == 39:     # 'c'
        return s[1]
    base, neg, p = get(BASE_A), False, s
    if len(p) > 1 and p[0] == 45 and p[1] in b'$#%':  # a minus before the prefix
        neg, p = True, p[1:]
    if p[:1] == b'$':
        base, p = 16, p[1:]
    elif p[:1] == b'#':
        base, p = 10, p[1:]
    elif p[:1] == b'%':
        base, p = 2, p[1:]
    if p[:1] == b'-':
        neg, p = not neg, p[1:]
    if not p or not 2 <= base <= 36:
        return None
    v = 0
    for c in p:
        if 48 <= c <= 57:
            d = c - 48                  # 0-9
        elif 65 <= c <= 90:
            d = c - 55                  # A-Z
        elif 97 <= c <= 122:
            d = c - 87                  # a-z
        else:
            return None
        if d >= base:
            return None
        v = v * base + d
    if v > MASK or (neg and v > SIGN):
        gerr(-24, s.decode('latin-1'))
    return -v & MASK if neg else v


def gexec(xt):                          # execute xt; through CATCH once core.fs has it
    if XT_CATCH:
        push(xt)
        run_xt(XT_CATCH)
        code = pop()
        if code:
            gerr(sgn(code))
    else:
        run_xt(xt)


def gline(line):                        # interpret one line of core.fs
    global gskip, gskipf
    if len(line) > 256:
        gfail('line too long (%d bytes, at most 256)' % len(line))
    for j, c in enumerate(line):
        setb(TIB + j, c)
    put(NTIB_A, len(line))
    put(TOIN_A, 0)
    while True:
        pname()
        if not pn:
            return
        word = memstr(pa, pn)
        if gskip:                       # "?:" skips a definition
            if word == b'\\':
                put(TOIN_A, get(NTIB_A))
            elif word == b'(':
                pparse(41)
            elif word == b';':
                gskip, gskipf = False, True
            continue
        if gskipf:                      # and the flags after it
            if word.upper() in (b'IMMEDIATE', b'UNCHECKED', b'COMPILE-ONLY'):
                continue
            gskipf = False
        if word == b'?:':
            save = get(TOIN_A)
            pname()
            if not pn:
                gerr(-16)
            keymem(pa, pn)
            if gfind():
                gskip = True
                continue
            put(TOIN_A, save)
            keystr(':')
            gexec(ntxt(gfind()))
            continue
        keymem(pa, pn)
        nt = gfind()
        if nt:
            xt = ntxt(nt)
            fl = getb(nt + C)
            if get(STATE_A) == 0 or fl & 128:
                if get(STATE_A) == 0 and fl & 64:
                    gerr(-14, word.decode('latin-1'))
                gexec(xt)
            else:
                comma(xt)
            continue
        if word == b'\\':               # comments until core.fs defines them
            put(TOIN_A, get(NTIB_A))
            continue
        if word == b'(':
            pparse(41)
            continue
        num = gnumber(word)
        if num is None:
            gerr(-13, word.decode('latin-1'))
        if get(STATE_A):
            comma(XT_LIT)
            comma(num)
        else:
            push(num)


def genesis():                          # compile core.fs; then remove the host primitives
    global sp, rp, XT_LIT, XT_CATCH, gln
    put(HALT_XT, 127)
    put(HALT_ADDR, HALT_XT)
    put(DP_A, DICT_START + GUARD)
    put(VP_A, VP_START)
    put(BASE_A, 10)
    put(WL_FORTH + C, 0)
    put(WL_INTERNALS + C, WL_FORTH)
    put(VOCLINK_A, WL_INTERNALS)
    sp, rp = S0, R0
    for op, name, voc, flags in PRIM:   # a header for each primitive
        if not with_header(op):
            continue
        xt = mkhead(name, WL_FORTH if voc == 'F' else WL_INTERNALS, flags)
        put(xt, op)
        put(DP_A, xt + C)
        if op == 5:
            XT_LIT = xt
        if op >= 100:
            gnt.add(get(LATEST_A))
    consts, a = [], SYS                 # constants for the system variables, then 9 more
    for name, cells, voc in SV:
        consts.append((name, a, voc))
        a += cells * C
    for name, value in (('TIB', TIB), ('PAD', PAD), ('S0', S0), ('R0', R0),
                        ('DATA-END', DATA_END), ('WL-FORTH', WL_FORTH),
                        ('WL-INTERNALS', WL_INTERNALS), ('CELL', C), ('DICT-END', DICT_END)):
        consts.append((name, value, 'I'))
    for name, value, voc in consts:
        xt = mkhead(name, WL_FORTH if voc == 'F' else WL_INTERNALS, 0)
        put(xt, 2)
        put(xt + C, value)
        put(DP_A, xt + 2 * C)
    put(NORDER_A, 2)
    put(CONTEXT_A, WL_FORTH)
    put(CONTEXT_A + C, WL_INTERNALS)
    put(CURRENT_A, WL_FORTH)
    lines = readlines(files[0])
    if lines is None:
        msg('cannot read %s' % files[0])
        bye(1)
    for gln, line in enumerate(lines, 1):
        gline(line)
        if not XT_CATCH:
            keystr('CATCH')
            nt = gfind()
            if nt:
                XT_CATCH = ntxt(nt)
    wl = get(VOCLINK_A)                 # remove the host primitives and the first ":" ";"
    while wl:
        prev, nt = 0, get(wl)
        while nt:
            nx = get(nt)
            if nt in gnt:
                put(prev if prev else wl, nx)
            else:
                prev = nt
            nt = nx
        wl = get(wl + C)


# ==== Images: Intel HEX ===================================================
# An image is a text file (doc/vm.md, section 10). The first line names
# the cell size and the start words; then come Intel HEX records for the
# system variables, the RAM data up to VP and the dictionary up to DP.


def hexseg(a, e, lines, state):         # the records for the bytes from a up to e (exclusive)
    while a < e:
        hi = a >> 16
        if hi != state[0]:              # extended linear address: the upper 16 bits
            state[0] = hi
            lines.append(':02000004%04X%02X' % (hi, (256 - (6 + (hi >> 8) + (hi & 255)) % 256) % 256))
        n = min(e - a, 16, 65536 - (a & 0xFFFF))
        data = [getb(a + j) for j in range(n)]
        s = n + (a >> 8 & 255) + (a & 255) + sum(data)
        lines.append(':%02X%04X00%s%02X' % (n, a & 0xFFFF, ''.join('%02X' % b for b in data),
                                            (256 - s % 256) % 256))
        a += n


def write_image(name, boot):            # True if it works
    lines = ['; ucf image bits=%d boot=%d quit=%d throw=%d prims=%s'
             % (BITS, boot, XT_QUIT, XT_THROW, prims)]
    state = [0]                         # a record for the upper bits only above $FFFF
    hexseg(SYS, DATA_START, lines, state)
    hexseg(DATA_START, get(VP_A), lines, state)
    hexseg(DICT_START, get(DP_A), lines, state)
    lines.append(':00000001FF')
    try:
        try:
            os.remove(name)             # a new file (see CREATE-FILE)
        except OSError:
            pass
        with open(name, 'wb') as f:
            f.write(('\n'.join(lines) + '\n').encode('ascii'))
        return True
    except OSError:
        return False


meta = ''                               # the first line of an image


def hexread(name, compare):             # the records into memory, or compare them with it
    global meta
    lines = readlines(name)
    if lines is None:
        sys.stderr.write('ucf.py: cannot read %s\n' % name)
        sys.exit(1)
    hi, diffs, first = 0, 0, ''
    for k, line in enumerate(lines, 1):
        s = line.decode('latin-1')
        if s.startswith(';'):
            if ' boot=' in s:
                meta = s
            continue
        if not s.startswith(':'):
            continue
        rec = bytes.fromhex(s[1:])
        n, a, t = rec[0], rec[1] << 8 | rec[2], rec[3]
        if sum(rec) % 256:
            sys.stderr.write('ucf.py: %s:%d: wrong checksum\n' % (name, k))
            sys.exit(1)
        if t == 0:
            for j in range(n):
                b = rec[4 + j]
                if not compare:
                    setb(hi + a + j, b)
                else:
                    r = getb(hi + a + j)
                    if r != b:
                        if not diffs:
                            first = '$%X (here %d, there %d)' % (hi + a + j, r, b)
                        diffs += 1
        elif t == 1:
            break
        elif t == 4:
            hi = (rec[4] << 8 | rec[5]) << 16
    return diffs, first


def metaval(field):                     # the number after " field=" in the first line
    for part in meta.split():
        if part.startswith(field + '='):
            return int(part[len(field) + 1:])
    return 0


def load_image(name):                   # memory, start words, primitives
    global meta, XT_BOOT, XT_QUIT, XT_THROW
    meta = ''
    hexread(name, False)
    if not meta:
        sys.stderr.write('ucf.py: %s is not an image of uCForth\n' % name)
        sys.exit(1)
    XT_BOOT, XT_QUIT, XT_THROW = metaval('boot'), metaval('quit'), metaval('throw')
    for part in meta.split():
        if part.startswith('prims='):
            for r in part[6:].split(','):    # each range lo-hi: this VM must have it
                lo, _, hi = r.partition('-')
                for op in range(int(lo), int(hi or lo) + 1):
                    if not have(op):
                        sys.stderr.write('ucf.py: %s needs primitive %d\n' % (name, op))
                        sys.exit(1)


checkfail = 0


def check_image(what, name):            # compare the memory with a reference
    global checkfail
    if not readable(name):
        msg('check %s: cannot read %s' % (what, name))
        checkfail = 1
        return
    diffs, first = hexread(name, True)
    if diffs:
        msg('check %s: %d bytes differ from %s, the first at %s' % (what, diffs, name, first))
        checkfail = 1
    else:
        msg('check %s: the same as %s' % (what, name))


def save_image():                       # SAVE-IMAGE ( -- ior )
    f = os.environ.get('UCF_IMAGE') or 'ucf.hex'
    if write_image(f, XT_BOOT or XT_QUIT):
        msg('saved: ' + f)
        push(0)
    else:
        push(-37)


# ==== The HAL (doc/vm.md, section 8) and the host primitives =============

T0 = 0                                  # the clock at the start


def wait_ms(ms):                        # in steps of 100 ms, so that Ctrl-C comes through
    while ms > 0 and not brk:
        step = min(ms, 100)
        wait_us(step * 1000)
        ms -= step


def hal(op):
    global sp
    if op == 80:                        # MILLIS
        push(now_ms() - T0)
    elif op == 81:                      # MS
        wait_ms(pop())
    elif op == 82:                      # US
        wait_us(pop())
    elif op == 83:                      # BIS!
        a, t = pop(), pop() & 255
        setb(a, getb(a) | t)
    elif op == 84:                      # BIC!
        a, t = pop(), pop() & 255
        setb(a, getb(a) & ~t)
    elif op == 85:                      # SAVE-IMAGE
        save_image()
    elif op == 86:                      # ERASE-IMAGE
        pass
    elif op == 87:                      # REBOOT
        reboot()
    elif op == 88:                      # BYE
        bye(0)
    elif op == 89:                      # OPEN-FILE
        fopen_(False)
    elif op == 90:                      # CREATE-FILE
        fopen_(True)
    elif op == 91:                      # CLOSE-FILE
        fclose_()
    elif op == 92:                      # READ-FILE
        fread_()
    elif op == 93:                      # WRITE-FILE
        fwrite_()
    elif op == 98:                      # READ-LINE
        freadline()
    elif op == 99:                      # (BYE)
        bye(pop() & 255)
    elif op == 100:                     # PARSE-NAME
        pname()
        push(pa)
        push(pn)
    elif op == 101:                     # PARSE
        pparse(pop() & 255)
        push(pa)
        push(pn)
    elif op == 102:                     # HEADER
        gheader()
    elif op == 103:                     # REVEAL
        t = get(LATEST_A)
        if t:
            put(get(CURRENT_A), t)
    elif op == 104:                     # IMMEDIATE
        a = get(LATEST_A) + C
        setb(a, getb(a) | 128)
    elif op == 105:                     # ,
        comma(pop())
    elif op == 106:                     # C,
        t = get(DP_A)
        setb(t, pop())
        put(DP_A, t + 1)
    elif op == 107:                     # [
        put(STATE_A, 0)
    elif op == 108:                     # ]
        put(STATE_A, MASK)
    else:                               # an unknown primitive
        throw_(-21)


# ==== Start ================================================================


def words_in_forth():
    n, nt = 0, get(WL_FORTH)
    while nt:
        n += 1
        nt = get(nt)
    return n


def core_end():                         # Genesis is complete: the seed, and its check
    if dump_core_file and not write_image(dump_core_file, XT_QUIT):
        msg('cannot write ' + dump_core_file)
    if check:
        check_image('core%d' % BITS, '%sseed/core%d.hex' % (pfx, BITS))


def build_end():                        # base.fs is complete (doc/genesis.md, section 6)
    global build_at, XT_BOOT
    here = get(DP_A)
    put(BASEEND_A, here)
    put(FROZENEND_A, here)
    put(NORDER_A, 1)
    put(CONTEXT_A, WL_FORTH)
    put(CURRENT_A, WL_FORTH)
    put(STATE_A, 0)
    build_at = -2
    XT_BOOT = sysxt('BOOT')
    if dump_file and not write_image(dump_file, XT_BOOT):
        msg('cannot write ' + dump_file)
    if check:
        ob.clear()                      # the prompt of QUIT is not wanted here
        check_image('base%d' % BITS, '%sseed/base%d.hex' % (pfx, BITS))
        bye(checkfail)
    out.write(('uCForth - Python bootstrap VM - %d bit%s - %d words in FORTH - built in %.1f s\n'
               % (BITS, ' - pure' if pure else '', words_in_forth(), (now_ms() - T0) / 1000))
              .encode('ascii'))


def main():
    global T0, tty, sp, rp, fi, build_at, XT_THROW, XT_QUIT
    options(sys.argv)
    memory_map()
    primitives()
    make_table()
    T0 = now_ms()
    tty = is_tty()
    if image_file:
        load_image(image_file)
        files.extend(args)
        find_words()
        out.write(('uCForth - Python bootstrap VM - %d bit - %d words in FORTH - image %s\n'
                   % (BITS, words_in_forth(), image_file)).encode('utf-8', 'replace'))
        sp, rp = S0, R0
        put(STATE_A, 0)
        put(HANDLER_A, 0)
        catch_int()
        run_xt(XT_BOOT or XT_QUIT)
    else:
        files.extend([core_file, base_file] + args)
        genesis()
        XT_THROW = sysxt('THROW')
        XT_QUIT = sysxt('QUIT')
        if not XT_QUIT:
            gfail('%s does not define QUIT' % files[0])
        find_words()
        core_end()
        fi, build_at = 1, 1
        catch_int()
        run_xt(XT_QUIT)
    bye(0)


if __name__ == '__main__':
    main()
