/* ucf.c - the C bootstrap VM of uCForth
 *
 * It is the virtual machine of uCForth in C: the memory, the registers,
 * the primitives and a small HAL (doc/vm.md). Its Genesis, a small outer
 * interpreter, compiles forth/core.fs once (doc/genesis.md). From then on
 * QUIT in the image reads each line, as on a board: first forth/base.fs,
 * then the files of the command line, then the keyboard (or a pipe).
 *
 * This program is the reference among the bootstrap VMs: its parts come
 * in the order of the contract, each with a comment. The README in this
 * folder explains it part by part.
 *
 * Build:  cc -O2 -o build/ucf-c bootstrap/c/ucf.c
 * Use:    build/ucf-c [OPTION ...] [FILE ...]
 *
 *   FILE              a source file for QUIT; then the keyboard
 *   -32               32-bit cells (default: 16 bit)
 *   -pure             only the 32 mandatory primitives and the HAL: the
 *                     words with "?:" stay Forth words
 *   -core FILE        Genesis compiles FILE (default: forth/core.fs)
 *   -base FILE        QUIT compiles FILE first (default: forth/base.fs)
 *   -image FILE       start from an image, not from core and base
 *   -dump-core FILE   write the image after core to FILE (the seed)
 *   -dump FILE        write the image after base to FILE
 *   -check            build with -pure, compare the images with
 *                     seed/core16.hex and seed/base16.hex (32 bit: ...32),
 *                     then stop; exit status 0 if they are the same
 *
 * SAVE-IMAGE (and so FREEZE) writes the image to ucf.hex, or to the file
 * in the environment variable UCF_IMAGE. Ctrl-C stops the word that runs.
 *
 * The program is ANSI C (C89) with its standard library. Only the part
 * "The system" uses the operating system: the clock, a wait, the test
 * for a keyboard and Ctrl-C, for POSIX (Linux, macOS, BSD) or Windows.
 *
 * Limits:
 * - ?RX gets whole lines: the terminal sends a line with the return key.
 * - No checks of the stacks. An address wraps around in the memory of
 *   the VM (64 KB or 256 KB), so no address reaches other memory.
 * - REBOOT starts BOOT or QUIT again, but does not load the image again.
 *   ERASE-IMAGE does nothing.
 * - OPEN-FILE with W/O or R/W writes at the end of the file.
 * - TX! does not send the byte 0.
 */

#ifndef _WIN32
#define _POSIX_C_SOURCE 200112L
#endif

#include <signal.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#ifdef _WIN32
#include <io.h>
#include <windows.h>
#else
#include <sys/time.h>
#include <time.h>
#include <unistd.h>
#endif

/* ==== The system ========================================================
 * The clock in milliseconds, a wait in microseconds, the test "does the
 * input come from a keyboard", and the signal of Ctrl-C: only these
 * functions differ between POSIX and Windows.
 */

static volatile sig_atomic_t brk;       /* Ctrl-C came: stop the word that runs */

#ifdef _WIN32

static unsigned long now_ms(void) { return (unsigned long)GetTickCount(); }
static void wait_us(unsigned long us) { Sleep((DWORD)(us / 1000)); }
static int is_tty(void) { return _isatty(_fileno(stdin)); }
static void on_int(int sig) { (void)sig; brk = 1; signal(SIGINT, on_int); }
static void catch_int(void) { signal(SIGINT, on_int); }

#else

static unsigned long now_ms(void)
{
  struct timeval tv;
  gettimeofday(&tv, NULL);
  return (unsigned long)tv.tv_sec * 1000UL + (unsigned long)tv.tv_usec / 1000UL;
}

static void wait_us(unsigned long us)   /* Ctrl-C ends the wait early */
{
  struct timespec ts;
  ts.tv_sec = (time_t)(us / 1000000UL);
  ts.tv_nsec = (long)(us % 1000000UL) * 1000L;
  nanosleep(&ts, NULL);
}

static int is_tty(void) { return isatty(0); }

static void on_int(int sig) { (void)sig; brk = 1; }

static void catch_int(void)   /* no SA_RESTART: Ctrl-C also ends a read at the keyboard */
{
  struct sigaction sa;
  memset(&sa, 0, sizeof sa);
  sa.sa_handler = on_int;
  sigemptyset(&sa.sa_mask);
  sigaction(SIGINT, &sa, NULL);
}

#endif

/* ==== Declarations ======================================================
 * C wants to know a function before its first use. These functions are
 * used before the place where they are.
 */

static void vm(void);
static void rx(void);
static void tx(void);
static void hal(unsigned long op);
static void gfail(const char *s);
static void build_end(void);
static int slurp(const char *name, char **text, unsigned long *len);

/* ==== Options ===========================================================
 * The default files lie in the project. The program looks for them in
 * the current folder (ucf), then two folders above (ucf/bootstrap/c).
 */

static int BITS = 16;                   /* the cell size */
static int pure, check;                 /* -pure, -check */
static const char *core_file, *base_file, *image_file, *dump_file, *dump_core_file;
static const char *pfx = "";            /* "" or "../../": where the project is */
static char core_default[32], base_default[32];
static char **args;                     /* the files of the command line */
static int nargs;

static int readable(const char *name)
{
  FILE *f = fopen(name, "rb");
  if (!f) return 0;
  fclose(f);
  return 1;
}

static void usage(void)
{
  fputs("use: ucf-c [-32] [-pure] [-core FILE] [-base FILE] [-image FILE]\n"
        "           [-dump-core FILE] [-dump FILE] [-check] [FILE ...]\n", stderr);
  exit(2);
}

static void options(int argc, char **argv)
{
  int i;
  char *text;
  unsigned long len;

  if (!readable("forth/core.fs") && readable("../../forth/core.fs")) pfx = "../../";
  sprintf(core_default, "%sforth/core.fs", pfx);
  sprintf(base_default, "%sforth/base.fs", pfx);
  core_file = core_default;
  base_file = base_default;
  args = (char **)malloc((size_t)argc * sizeof *args);
  if (!args) exit(1);
  for (i = 1; i < argc; i++) {
    char *a = argv[i];
    if (!strcmp(a, "--")) continue;
    if (!strcmp(a, "-32")) BITS = 32;
    else if (!strcmp(a, "-pure")) pure = 1;
    else if (!strcmp(a, "-check")) { check = 1; pure = 1; }
    else if (!strcmp(a, "-core") || !strcmp(a, "-base") || !strcmp(a, "-image")
             || !strcmp(a, "-dump-core") || !strcmp(a, "-dump")) {
      if (i + 1 >= argc) usage();
      if (!strcmp(a, "-core")) core_file = argv[++i];
      else if (!strcmp(a, "-base")) base_file = argv[++i];
      else if (!strcmp(a, "-image")) image_file = argv[++i];
      else if (!strcmp(a, "-dump-core")) dump_core_file = argv[++i];
      else dump_file = argv[++i];
    }
    else if (a[0] == '-') usage();
    else args[nargs++] = a;
  }
  if (image_file) {                     /* the first line of an image names the cell size */
    if (!slurp(image_file, &text, &len)) {
      fprintf(stderr, "ucf-c: cannot read %s\n", image_file);
      exit(1);
    }
    BITS = strstr(text, "bits=32") ? 32 : 16;
    free(text);
  }
}

/* ==== Memory map (doc/vm.md, section 3) =================================
 * The memory of the VM is an array of bytes. A cell is 2 or 4 bytes, the
 * low byte first (little endian), whatever the computer does itself.
 */

static unsigned char *mem;              /* 64 KB (16 bit) or 256 KB (32 bit), from calloc */
static unsigned long C;                 /* bytes in a cell: 2 or 4 */
static unsigned long MASK;              /* the biggest cell: $FFFF or $FFFFFFFF */
static unsigned long SIGN;              /* the top bit of a cell */
static unsigned long AMASK;             /* the size of the memory - 1: addresses wrap around */
static unsigned long SYS, DATA_START, DATA_END, TIB, PAD, R0, S0;
static unsigned long DICT_START, DICT_END, MEM_END;
static unsigned long GUARD;             /* free bytes at DICT_START: a stack underflow lands there */
static unsigned long HALT_XT, HALT_ADDR;   /* the two HALT cells */
static unsigned long RUN_ADDR;          /* two cells at the top: run one word, then HALT */
static unsigned long THROW_ADDR;        /* two cells at the top: run THROW */
static unsigned long WL_FORTH, WL_INTERNALS, VP_START;

/* The system variables (doc/vm.md, section 4), one cell each from SYS */
#define DP_A        (SYS)
#define VP_A        (SYS + C)
#define LATEST_A    (SYS + 2 * C)
#define STATE_A     (SYS + 3 * C)
#define BASE_A      (SYS + 4 * C)
#define TOIN_A      (SYS + 5 * C)
#define NTIB_A      (SYS + 6 * C)
#define CURRENT_A   (SYS + 7 * C)
#define NORDER_A    (SYS + 8 * C)
#define CONTEXT_A   (SYS + 9 * C)
#define VOCLINK_A   (SYS + 25 * C)
#define CASESENSE_A (SYS + 26 * C)
#define BASEEND_A   (SYS + 27 * C)
#define FROZENEND_A (SYS + 28 * C)
#define HANDLER_A   (SYS + 32 * C)

/* The names of the system variables for Genesis: name, cells, vocabulary
 * (F = FORTH, I = INTERNALS). The addresses follow one after the other.
 */
static const struct { const char *name; int cells; char voc; } SV[] = {
  { "DP", 1, 'I' }, { "VP", 1, 'I' }, { "LATEST", 1, 'I' }, { "STATE", 1, 'I' },
  { "BASE", 1, 'F' }, { ">IN", 1, 'I' }, { "#TIB", 1, 'I' }, { "CURRENT", 1, 'I' },
  { "#ORDER", 1, 'I' }, { "CONTEXT", 16, 'I' }, { "VOC-LINK", 1, 'I' },
  { "CASE-SENSE", 1, 'I' }, { "BASE-END", 1, 'I' }, { "FROZEN-END", 1, 'I' },
  { "AUTORUN", 1, 'I' }, { "ERRMSG", 2, 'I' }, { "HANDLER", 1, 'I' }
};
#define NSV ((int)(sizeof SV / sizeof SV[0]))

static void memory_map(void)
{
  unsigned long k = BITS == 16 ? 1 : 2;
  C = (unsigned long)BITS / 8;
  MASK = BITS == 16 ? 0xFFFFUL : 0xFFFFFFFFUL;
  SIGN = MASK / 2 + 1;
  SYS = 256;
  DATA_START = 512 * k;
  DATA_END = k == 1 ? 4096UL : 65536UL;
  TIB = DATA_END;
  PAD = TIB + 256;
  R0 = PAD + 256 + 256 * C;             /* the return stack: 256 cells below R0 */
  S0 = R0 + 256 * C;                    /* the data stack: 256 cells below S0 */
  DICT_START = S0;
  GUARD = 8 * C;                        /* the first word comes 8 cells above S0 */
  MEM_END = k == 1 ? 65536UL : 262144UL;
  DICT_END = MEM_END - 16;
  AMASK = MEM_END - 1;
  HALT_XT = DATA_START - 16;
  HALT_ADDR = HALT_XT + C;
  RUN_ADDR = MEM_END - 2 * C;
  THROW_ADDR = MEM_END - 4 * C;
  WL_FORTH = DATA_START;
  WL_INTERNALS = WL_FORTH + 3 * C;
  VP_START = WL_INTERNALS + 3 * C;
  mem = (unsigned char *)calloc((size_t)MEM_END, 1);   /* each byte 0 */
  if (!mem) {
    fputs("ucf-c: not enough memory\n", stderr);
    exit(1);
  }
}

/* ==== Primitives ========================================================
 * Number, name, vocabulary, flags (64 = compile-only, 128 = immediate).
 * 0-3 are the code field types, 4-31 the other mandatory primitives,
 * 32-65 the optional ones, 80-88 the HAL, 100-108 exist only for Genesis.
 */

static const struct { int op; const char *name; char voc; int flags; } PRIM[] = {
  { 4, "EXIT", 'F', 64 }, { 5, "LIT", 'I', 64 }, { 6, "BRANCH", 'I', 64 },
  { 7, "?BRANCH", 'I', 64 }, { 8, "EXECUTE", 'F', 0 }, { 9, "@", 'F', 0 },
  { 10, "!", 'F', 0 }, { 11, "C@", 'F', 0 }, { 12, "C!", 'F', 0 },
  { 13, "DUP", 'F', 0 }, { 14, "DROP", 'F', 0 }, { 15, "SWAP", 'F', 0 },
  { 16, "OVER", 'F', 0 }, { 17, ">R", 'F', 64 }, { 18, "R>", 'F', 64 },
  { 19, "R@", 'F', 64 }, { 20, "SP@", 'I', 0 }, { 21, "SP!", 'I', 0 },
  { 22, "RP@", 'I', 0 }, { 23, "RP!", 'I', 0 }, { 24, "AND", 'F', 0 },
  { 25, "OR", 'F', 0 }, { 26, "XOR", 'F', 0 }, { 27, "0<", 'F', 0 },
  { 28, "UM+", 'I', 0 }, { 29, "?RX", 'I', 0 }, { 30, "TX!", 'I', 0 },
  { 31, "!IO", 'I', 0 },
  { 32, "+", 'F', 0 }, { 33, "-", 'F', 0 }, { 34, "1+", 'F', 0 },
  { 35, "1-", 'F', 0 }, { 36, "NEGATE", 'F', 0 }, { 37, "INVERT", 'F', 0 },
  { 38, "2*", 'F', 0 }, { 39, "2/", 'F', 0 }, { 40, "LSHIFT", 'F', 0 },
  { 41, "RSHIFT", 'F', 0 }, { 42, "=", 'F', 0 }, { 43, "0=", 'F', 0 },
  { 44, "<", 'F', 0 }, { 45, "U<", 'F', 0 }, { 46, "*", 'F', 0 },
  { 47, "UM*", 'F', 0 }, { 48, "UM/MOD", 'F', 0 }, { 49, "ROT", 'F', 0 },
  { 50, "NIP", 'F', 0 }, { 51, "?DUP", 'F', 0 }, { 52, "2DUP", 'F', 0 },
  { 53, "2DROP", 'F', 0 }, { 54, "+!", 'F', 0 }, { 55, "MOVE", 'F', 0 },
  { 56, "FILL", 'F', 0 }, { 57, "COUNT", 'F', 0 }, { 58, "FIND-NAME", 'I', 0 },
  { 59, "(DO)", 'I', 64 }, { 60, "(?DO)", 'I', 64 }, { 61, "(LOOP)", 'I', 64 },
  { 62, "(+LOOP)", 'I', 64 }, { 63, "I", 'F', 64 }, { 64, "J", 'F', 64 },
  { 65, "UNLOOP", 'F', 64 },
  { 80, "MILLIS", 'F', 0 }, { 81, "MS", 'F', 0 }, { 82, "US", 'F', 0 },
  { 83, "BIS!", 'F', 0 }, { 84, "BIC!", 'F', 0 }, { 85, "SAVE-IMAGE", 'I', 0 },
  { 86, "ERASE-IMAGE", 'I', 0 }, { 87, "REBOOT", 'I', 0 }, { 88, "BYE", 'F', 0 },
  { 100, "PARSE-NAME", 'I', 0 }, { 101, "PARSE", 'I', 0 }, { 102, "HEADER", 'I', 0 },
  { 103, "REVEAL", 'I', 0 }, { 104, "IMMEDIATE", 'F', 0 }, { 105, ",", 'F', 0 },
  { 106, "C,", 'F', 0 }, { 107, "[", 'F', 128 }, { 108, "]", 'F', 0 }
};
#define NPRIM ((int)(sizeof PRIM / sizeof PRIM[0]))

static char prims[64];                  /* the primitives with a header, as "0-31,80-88" */

static int with_header(int op)          /* does Genesis make a header for op? */
{
  return op < 32 || op >= 80 || !pure;
}

static int have(unsigned long op)       /* can this VM run op? An image may use only these. */
{
  return op <= 65 || (op >= 80 && op <= 93) || op == 98 || op == 99;
}

static void primitives(void)   /* prims: the numbers below 100 with a header, as ranges */
{
  int ops[80], n = 0, i, first, prev;
  char *p = prims;
  for (i = 0; i < 4; i++) ops[n++] = i;
  for (i = 0; i < NPRIM; i++)
    if (PRIM[i].op < 100 && with_header(PRIM[i].op)) ops[n++] = PRIM[i].op;
  first = prev = ops[0];
  for (i = 1; i <= n; i++) {
    if (i < n && ops[i] == prev + 1) { prev = ops[i]; continue; }
    if (p != prims) *p++ = ',';
    if (first == prev) p += sprintf(p, "%d", first);
    else p += sprintf(p, "%d-%d", first, prev);
    if (i < n) first = prev = ops[i];
  }
}

/* ==== Helpers ===========================================================
 * The access to the memory, the stacks, and the messages of this program.
 */

static unsigned long getb(unsigned long a) { return mem[a & AMASK]; }
static void setb(unsigned long a, unsigned long b) { mem[a & AMASK] = (unsigned char)(b & 255); }

static unsigned long get(unsigned long a)   /* the cell at a: the low byte first */
{
  unsigned long x = mem[a & AMASK] | (unsigned long)mem[(a + 1) & AMASK] << 8;
  if (C == 4)
    x |= (unsigned long)mem[(a + 2) & AMASK] << 16 | (unsigned long)mem[(a + 3) & AMASK] << 24;
  return x;
}

static void put(unsigned long a, unsigned long x)   /* store the cell x at a */
{
  mem[a & AMASK] = (unsigned char)(x & 255);
  mem[(a + 1) & AMASK] = (unsigned char)(x >> 8 & 255);
  if (C == 4) {
    mem[(a + 2) & AMASK] = (unsigned char)(x >> 16 & 255);
    mem[(a + 3) & AMASK] = (unsigned char)(x >> 24 & 255);
  }
}

static unsigned long ip, w, sp, rp;     /* the registers (doc/vm.md, section 5) */

static void push(unsigned long x) { sp -= C; put(sp, x & MASK); }
static unsigned long pop(void) { unsigned long x = get(sp); sp += C; return x; }

static long sgn(unsigned long x)        /* a cell as a signed number */
{
  return x & SIGN ? -(long)(MASK - x) - 1 : (long)x;
}

static unsigned long ntxt(unsigned long nt)   /* the xt of the header nt */
{
  return (nt + C + 1 + (getb(nt + C) & 31) + C - 1) / C * C;
}

static void die(const char *s)
{
  fflush(stdout);
  fprintf(stderr, "ucf-c: %s\n", s);
  exit(1);
}

static void *more(void *p, unsigned long size)   /* realloc, or stop */
{
  p = realloc(p, (size_t)size);
  if (!p) die("out of memory");
  return p;
}

/* The output: TX! collects the characters of a line in ob, and the line
 * shows at LF. So the prompts of QUIT stay hidden while a file loads.
 */
static char *ob;                        /* the output line that does not show yet */
static unsigned long obn, obcap;

static void ob_add(int c)
{
  if (obn + 2 > obcap) { obcap = obcap * 2 + 256; ob = (char *)more(ob, obcap); }
  ob[obn++] = (char)c;
  ob[obn] = 0;
}

static void ob_clear(void) { obn = 0; if (ob) ob[0] = 0; }

static char mbuf[1200];                 /* the text of a message */

static void msg(const char *s)          /* a message of this program on its own line */
{
  if (obn) { printf("%s\n", ob); ob_clear(); }
  printf("  %s\n", s);
}

static void bye(int n)
{
  if (obn) printf("%s\n", ob);
  fflush(stdout);
  exit(n);
}

static const char *throw_text(long n)
{
  static char s[24];
  switch (n) {
  case -3: return "data stack overflow";
  case -4: return "data stack underflow";
  case -8: return "dictionary overflow";
  case -10: return "division by zero";
  case -11: return "result out of range";
  case -13: return "undefined word";
  case -14: return "compile-only word";
  case -16: return "missing name";
  case -19: return "name too long";
  case -21: return "unsupported operation";
  case -24: return "number out of range";
  }
  sprintf(s, "error %ld", n);
  return s;
}

/* ==== Search in the dictionary ==========================================
 * key[0..kn-1] holds the name; without CASE-SENSE in upper case.
 */

static unsigned char key[32];
static unsigned long kn;

static int upper(unsigned long c, int cs)   /* a to z as A to Z, without CASE-SENSE */
{
  return (int)(!cs && c >= 97 && c <= 122 ? c - 32 : c);
}

static void keymem(unsigned long a, unsigned long n)   /* the key from memory */
{
  unsigned long j;
  int cs = get(CASESENSE_A) != 0;
  kn = n;
  for (j = 0; j < n && j < 32; j++) key[j] = (unsigned char)upper(getb(a + j), cs);
}

static void keystr(const char *s)       /* the key from a C string */
{
  unsigned long j;
  int cs = get(CASESENSE_A) != 0;
  kn = strlen(s);
  for (j = 0; j < kn && j < 32; j++) key[j] = (unsigned char)upper((unsigned char)s[j], cs);
}

static unsigned long wlfind(unsigned long wl)   /* the nt of the key in the wordlist wl, or 0 */
{
  unsigned long nt = get(wl), a, j;
  int cs = get(CASESENSE_A) != 0;
  while (nt) {
    if ((getb(nt + C) & 31) == kn) {
      a = nt + C + 1;
      for (j = 0; j < kn; j++)
        if ((unsigned long)upper(getb(a + j), cs) != key[j]) break;
      if (j == kn) return nt;
    }
    nt = get(nt);
  }
  return 0;
}

static unsigned long gfind(void)        /* the search order, the top first: the nt or 0 */
{
  unsigned long i, nt;
  if (kn == 0 || kn > 31) return 0;
  for (i = get(NORDER_A); i > 0; i--) {
    nt = wlfind(get(CONTEXT_A + (i - 1) * C));
    if (nt) return nt;
  }
  return 0;
}

static unsigned long sysxt(const char *s)   /* the xt of the oldest word with exactly this name */
{
  unsigned long n = strlen(s), nt, found, j;
  int wl;
  for (wl = 0; wl < 2; wl++) {
    found = 0;
    nt = get(wl ? WL_INTERNALS : WL_FORTH);
    while (nt) {
      if ((getb(nt + C) & 31) == n) {
        for (j = 0; j < n; j++) if (getb(nt + C + 1 + j) != (unsigned char)s[j]) break;
        if (j == n) found = nt;
      }
      nt = get(nt);
    }
    if (found) return ntxt(found);
  }
  return 0;
}

/* ==== The virtual machine ===============================================
 * ip, w, sp and rp are the registers. SP and RP point to the top cell;
 * the stacks grow down. A flag is MASK (true) or 0 (false).
 */

static unsigned long XT_THROW, XT_QUIT, XT_BOOT;   /* from the image */
static unsigned long XT_CATCH, XT_LIT;  /* for Genesis */
static unsigned long OUT_A;             /* the address of OUT, the column of the output */

static void throw_(long n)              /* THROW n in the image: the next word is THROW */
{
  if (!XT_THROW) { sprintf(mbuf, "%s (no THROW yet)", throw_text(n)); gfail(mbuf); }
  push((unsigned long)n);
  put(THROW_ADDR, XT_THROW);
  put(THROW_ADDR + C, HALT_XT);
  ip = THROW_ADDR;
}

static void run_xt(unsigned long xt)    /* execute xt; return when it is complete */
{
  put(RUN_ADDR, xt);
  put(RUN_ADDR + C, HALT_XT);
  ip = RUN_ADDR;
  vm();
}

static void run_later(unsigned long xt) /* the VM goes on with xt, then HALT */
{
  put(RUN_ADDR, xt);
  put(RUN_ADDR + C, HALT_XT);
  ip = RUN_ADDR;
}

static unsigned long in_pos, in_len;    /* the current input line (section "Terminal") */

static void do_break(void)              /* Ctrl-C between two primitives (doc/vm.md, section 9) */
{
  brk = 0;
  in_pos = in_len;                      /* the rest of the input line goes */
  printf("%s\n", ob);                   /* a new line */
  ob_clear();
  if (OUT_A) put(OUT_A, 0);
  if (get(HANDLER_A)) { throw_(-28); return; }   /* a CATCH gets "user interrupt" */
  sp = S0; rp = R0;                     /* else: empty stacks, then QUIT */
  put(STATE_A, 0);
  run_later(XT_QUIT);
}

static void um_star(void)               /* UM* ( u1 u2 -- ud ): a product with two cells */
{
  unsigned long x = get(sp + C), y = get(sp), lo, hi;
  if (BITS == 16) {                     /* 16 x 16 bits fit into an unsigned long */
    lo = x * y;
    hi = lo >> 16;
    lo &= 0xFFFFUL;
  } else {                              /* C89 has no 64-bit type: four products of 16 bits */
    unsigned long xl = x & 0xFFFFUL, xh = x >> 16, yl = y & 0xFFFFUL, yh = y >> 16;
    unsigned long ll = xl * yl, lh = xl * yh, hl = xh * yl, hh = xh * yh, mid;
    mid = (ll >> 16) + (lh & 0xFFFFUL) + (hl & 0xFFFFUL);
    lo = ((mid & 0xFFFFUL) << 16 | (ll & 0xFFFFUL)) & MASK;
    hi = (hh + (lh >> 16) + (hl >> 16) + (mid >> 16)) & MASK;
  }
  put(sp + C, lo);
  put(sp, hi);
}

static void um_slash_mod(void)          /* UM/MOD ( ud u -- rem quot ) */
{
  unsigned long u = pop(), hi = get(sp), lo = get(sp + C), q = 0, r = hi, top;
  int i;
  if (u == 0) { throw_(-10); return; }
  if (hi >= u) { throw_(-11); return; }  /* the quotient would not fit into a cell */
  for (i = 0; i < BITS; i++) {          /* division as on paper, one bit at a time */
    top = r & SIGN;
    r = ((r << 1) | (lo >> (BITS - 1))) & MASK;
    lo = (lo << 1) & MASK;
    q = (q << 1) & MASK;
    if (top || r >= u) { r = (r - u) & MASK; q |= 1; }
  }
  put(sp + C, r);
  put(sp, q);
}

static void move_(void)                 /* MOVE ( a1 a2 u -- ), also for areas that overlap */
{
  unsigned long u = pop(), a2 = pop(), a1 = pop(), j;
  if (a1 < a2) { for (j = u; j > 0; j--) setb(a2 + j - 1, getb(a1 + j - 1)); }
  else { for (j = 0; j < u; j++) setb(a2 + j, getb(a1 + j)); }
}

static void fill_(void)                 /* FILL ( a u c -- ) */
{
  unsigned long c = pop() & 255, u = pop(), a = pop(), j;
  for (j = 0; j < u; j++) setb(a + j, c);
}

static void findname(void)              /* FIND-NAME ( a u -- nt | 0 ) */
{
  unsigned long u = pop(), a = get(sp);
  if (u == 0 || u > 31) put(sp, 0);
  else { keymem(a, u); put(sp, gfind()); }
}

static void reboot(void)                /* REBOOT ( f -- ): empty stacks, then BOOT or QUIT */
{
  unsigned long f = get(sp);
  sp = S0; rp = R0;
  put(STATE_A, 0);
  put(HANDLER_A, 0);
  run_later(f && XT_BOOT ? XT_BOOT : XT_QUIT);
}

/* vm: NEXT and the primitives, until primitive 127 (HALT) comes. */
static void vm(void)
{
  unsigned long op, x, y, t, a;
  for (;;) {
    if (brk) do_break();
    w = get(ip); ip += C;               /* NEXT */
    op = get(w);
    while (op == 8) { w = pop(); op = get(w); }   /* EXECUTE */
    switch (op) {
    case 0: rp -= C; put(rp, ip); ip = w + C; break;                  /* DOCOL */
    case 1: push(w + 2 * C); break;                                   /* DOVAR */
    case 2: push(get(w + C)); break;                                  /* DOCON */
    case 3: push(w + 2 * C); rp -= C; put(rp, ip); ip = get(w + C); break;   /* DODOES */
    case 4: ip = get(rp); rp += C; break;                             /* EXIT */
    case 5: push(get(ip)); ip += C; break;                            /* LIT */
    case 6: ip = get(ip); break;                                      /* BRANCH */
    case 7: if (pop()) ip += C; else ip = get(ip); break;             /* ?BRANCH */
    case 9: put(sp, get(get(sp))); break;                             /* @ */
    case 10: a = pop(); x = pop(); put(a, x); break;                  /* ! */
    case 11: put(sp, getb(get(sp))); break;                           /* C@ */
    case 12: a = pop(); x = pop(); setb(a, x); break;                 /* C! */
    case 13: push(get(sp)); break;                                    /* DUP */
    case 14: sp += C; break;                                          /* DROP */
    case 15: x = get(sp); put(sp, get(sp + C)); put(sp + C, x); break;   /* SWAP */
    case 16: push(get(sp + C)); break;                                /* OVER */
    case 17: x = pop(); rp -= C; put(rp, x); break;                   /* >R */
    case 18: push(get(rp)); rp += C; break;                           /* R> */
    case 19: push(get(rp)); break;                                    /* R@ */
    case 20: x = sp; push(x); break;                                  /* SP@ */
    case 21: sp = get(sp); break;                                     /* SP! */
    case 22: push(rp); break;                                         /* RP@ */
    case 23: rp = pop(); break;                                       /* RP! */
    case 24: x = pop(); put(sp, get(sp) & x); break;                  /* AND */
    case 25: x = pop(); put(sp, get(sp) | x); break;                  /* OR */
    case 26: x = pop(); put(sp, get(sp) ^ x); break;                  /* XOR */
    case 27: put(sp, get(sp) & SIGN ? MASK : 0); break;               /* 0< */
    case 28: x = pop(); y = get(sp);                                  /* UM+ */
      t = (x + y) & MASK; put(sp, t); push(t < x ? 1 : 0); break;
    case 29: rx(); break;                                             /* ?RX */
    case 30: tx(); break;                                             /* TX! */
    case 31: break;                                                   /* !IO */
    case 32: x = pop(); put(sp, (get(sp) + x) & MASK); break;         /* + */
    case 33: x = pop(); put(sp, (get(sp) - x) & MASK); break;         /* - */
    case 34: put(sp, (get(sp) + 1) & MASK); break;                    /* 1+ */
    case 35: put(sp, (get(sp) - 1) & MASK); break;                    /* 1- */
    case 36: put(sp, (0 - get(sp)) & MASK); break;                    /* NEGATE */
    case 37: put(sp, ~get(sp) & MASK); break;                         /* INVERT */
    case 38: put(sp, (get(sp) << 1) & MASK); break;                   /* 2* */
    case 39: x = get(sp); put(sp, x >> 1 | (x & SIGN)); break;        /* 2/ */
    case 40: t = pop(); x = get(sp);                                  /* LSHIFT */
      put(sp, t >= (unsigned long)BITS ? 0 : (x << t) & MASK); break;
    case 41: t = pop(); x = get(sp);                                  /* RSHIFT */
      put(sp, t >= (unsigned long)BITS ? 0 : x >> t); break;
    case 42: x = pop(); put(sp, get(sp) == x ? MASK : 0); break;      /* = */
    case 43: put(sp, get(sp) ? 0 : MASK); break;                      /* 0= */
    case 44: x = pop(); y = get(sp);                                  /* < (signed) */
      put(sp, (y ^ SIGN) < (x ^ SIGN) ? MASK : 0); break;
    case 45: x = pop(); put(sp, get(sp) < x ? MASK : 0); break;       /* U< */
    case 46: x = pop(); put(sp, (get(sp) * x) & MASK); break;         /* * */
    case 47: um_star(); break;                                        /* UM* */
    case 48: um_slash_mod(); break;                                   /* UM/MOD */
    case 49: x = get(sp + 2 * C); put(sp + 2 * C, get(sp + C));       /* ROT */
      put(sp + C, get(sp)); put(sp, x); break;
    case 50: x = pop(); put(sp, x); break;                            /* NIP */
    case 51: x = get(sp); if (x) push(x); break;                      /* ?DUP */
    case 52: x = get(sp); y = get(sp + C); push(y); push(x); break;   /* 2DUP */
    case 53: sp += 2 * C; break;                                      /* 2DROP */
    case 54: a = pop(); x = pop(); put(a, (get(a) + x) & MASK); break;   /* +! */
    case 55: move_(); break;                                          /* MOVE */
    case 56: fill_(); break;                                          /* FILL */
    case 57: a = get(sp); put(sp, (a + 1) & MASK); push(getb(a)); break;   /* COUNT */
    case 58: findname(); break;                                       /* FIND-NAME */
    case 59: rp -= 3 * C;                                             /* (DO) */
      put(rp + 2 * C, get(ip)); put(rp + C, get(sp + C)); put(rp, get(sp));
      sp += 2 * C; ip += C; break;
    case 60: if (get(sp) == get(sp + C)) ip = get(ip);                /* (?DO) */
      else {
        rp -= 3 * C;
        put(rp + 2 * C, get(ip)); put(rp + C, get(sp + C)); put(rp, get(sp));
        ip += C;
      }
      sp += 2 * C; break;
    case 61: t = (get(rp) + 1) & MASK;                                /* (LOOP) */
      if (t == get(rp + C)) { rp += 3 * C; ip += C; } else { put(rp, t); ip = get(ip); }
      break;
    case 62: t = pop();                                               /* (+LOOP) */
      x = (get(rp) - get(rp + C)) & MASK;   /* index - limit */
      y = (x + t) & MASK;                   /* the same after the step */
      put(rp, (get(rp) + t) & MASK);
      if ((x ^ y) & (x ^ t) & SIGN) { rp += 3 * C; ip += C; }   /* it crossed limit-1 | limit */
      else ip = get(ip);
      break;
    case 63: push(get(rp)); break;                                    /* I */
    case 64: push(get(rp + 3 * C)); break;                            /* J */
    case 65: rp += 3 * C; break;                                      /* UNLOOP */
    case 127: return;                                                 /* HALT: back to C */
    default: hal(op);
    }
  }
}

/* ==== Terminal: input lines for ?RX, output for TX! =====================
 * The input comes from the files of the command line, then from the
 * keyboard or a pipe. A line of a file hides the prompt of QUIT.
 */

typedef struct {                        /* the lines of a file (doc/genesis.md, section 2) */
  char *text;                           /* the file */
  unsigned long *start, *len;           /* each line: where it starts, its length without LF and CR */
  unsigned long count;
} Lines;

static int readlines(const char *name, Lines *L)   /* 0 if the file cannot be read */
{
  unsigned long len, i, n = 1, k = 0, st = 0;
  if (!slurp(name, &L->text, &len)) return 0;
  for (i = 0; i < len; i++) if (L->text[i] == '\n') n++;   /* the text after the last LF is a line too */
  L->start = (unsigned long *)more(NULL, n * sizeof *L->start);
  L->len = (unsigned long *)more(NULL, n * sizeof *L->len);
  for (i = 0; i <= len; i++) {
    if (i == len || L->text[i] == '\n') {
      L->start[k] = st;
      L->len[k] = i - st;
      if (L->len[k] && L->text[i - 1] == '\r') L->len[k]--;
      k++;
      st = i + 1;
    }
  }
  L->count = n;
  return 1;
}

static void freelines(Lines *L)
{
  free(L->text); free(L->start); free(L->len);
  L->text = NULL; L->start = L->len = NULL; L->count = 0;
}

static char **files;                    /* core, base (not with -image), then the files of the command line */
static int nfiles, fi;                  /* fi: the next file to open */
static const char *curfile;             /* the file that loads now, or NULL */
static unsigned long curln;             /* its line number */
static Lines cf;                        /* its lines */
static int build_at = -1;               /* after the file with this number, the system is complete */
static int errseen;                     /* a line with "error:" came */
static int tty;                         /* the input comes from a keyboard */
static char *in_str;                    /* the current input line, with LF */
static unsigned long in_cap;
static char *kl;                        /* a line from the keyboard or a pipe */
static unsigned long kl_cap;
static char *prompt;                    /* what showed before the keyboard: the prompt */
static unsigned long prompt_cap;

static void setline(const char *s, unsigned long n)   /* the next line for ?RX */
{
  if (n && s[n - 1] == '\r') n--;
  if (n + 2 > in_cap) { in_cap = n + 256; in_str = (char *)more(in_str, in_cap); }
  memcpy(in_str, s, (size_t)n);
  in_str[n] = '\n';
  in_len = n + 1;
  in_pos = 0;
}

static long read_line(void)             /* a line from stdin into kl; -1: the end; -2: Ctrl-C */
{
  long n = 0;
  int c;
  for (;;) {
    c = getc(stdin);
    if (c == EOF) {
      if (brk && n == 0) return -2;
      return n ? n : -1;
    }
    if (c == '\n') return n;
    if ((unsigned long)n + 2 > kl_cap) { kl_cap = kl_cap * 2 + 256; kl = (char *)more(kl, kl_cap); }
    kl[n++] = (char)c;
  }
}

static void keep_prompt(void)           /* the prompt again, after Ctrl-C at the keyboard */
{
  if (obn + 1 > prompt_cap) { prompt_cap = obn + 64; prompt = (char *)more(prompt, prompt_cap); }
  memcpy(prompt, ob, (size_t)obn + 1);
}

static void next_line(void)             /* the next line for ?RX */
{
  long n;
  for (;;) {
    if (curfile && errseen) {           /* an error stops all files */
      sprintf(mbuf, "%.1000s:%lu: loading stopped", curfile, curln);
      msg(mbuf);
      curfile = NULL;
      freelines(&cf);
      if (build_at >= 0 && fi <= build_at + 1) { fi = build_at + 1; build_end(); }
      fi = nfiles;
    }
    errseen = 0;
    if (curfile) {
      if (curln < cf.count) {
        curln++;
        ob_clear();                     /* the prompt of QUIT does not show */
        setline(cf.text + cf.start[curln - 1], cf.len[curln - 1]);
        return;
      }
      curfile = NULL;
      freelines(&cf);
      if (fi == build_at + 1) build_end();
      continue;
    }
    if (fi < nfiles) {
      curfile = files[fi++];
      curln = 0;
      if (!readlines(curfile, &cf)) {
        sprintf(mbuf, "cannot read %.1000s", curfile);
        msg(mbuf);
        curfile = NULL;
        fi = nfiles;
      }
      continue;
    }
    if (tty) {                          /* the keyboard: the terminal shows the line */
      keep_prompt();
      fputs(ob, stdout); ob_clear(); fflush(stdout);
      while ((n = read_line()) == -2) { /* Ctrl-C: a new line, the prompt again */
        brk = 0; clearerr(stdin);
        printf("\n%s", prompt); fflush(stdout);
      }
      brk = 0;
      if (n < 0) { printf("\n"); bye(0); }
    } else {                            /* a pipe: the line shows after the prompt */
      fflush(stdout);
      while ((n = read_line()) == -2) { brk = 0; clearerr(stdin); }
      if (n < 0) bye(0);
      printf("%s%.*s\n", ob, (int)n, kl);
      ob_clear();
    }
    setline(kl, (unsigned long)n);
    return;
  }
}

static unsigned long XT_REFILL, REFILL_END;   /* the body of REFILL: QUIT waits for a line there */

static int refilling(void)              /* 1 if a return address points into REFILL */
{
  unsigned long r;
  for (r = rp; r < rp + 8 * C && r < R0; r += C)
    if (get(r) > XT_REFILL && get(r) < REFILL_END) return 1;
  return 0;
}

static void rx(void)                    /* ?RX ( -- c true | false ) */
{
  if (in_pos >= in_len) {
    if (!refilling() && (curfile || fi < nfiles || !tty)) {
      push(0);                          /* a word reads keys while files load: no key (doc/vm.md 9) */
      return;
    }
    next_line();                        /* QUIT waits in REFILL, or a word waits at the keyboard */
  }
  push((unsigned char)in_str[in_pos++]);
  push(MASK);
}

static unsigned long bodyend(unsigned long xt)   /* the next header in memory after xt */
{
  unsigned long best = MEM_END, wl = get(VOCLINK_A), nt;
  while (wl) {
    for (nt = get(wl); nt; nt = get(nt)) if (nt > xt && nt < best) best = nt;
    wl = get(wl + C);
  }
  return best;
}

static void find_words(void)            /* the words of the image that this program needs */
{
  unsigned long xt;
  XT_REFILL = sysxt("REFILL");
  REFILL_END = bodyend(XT_REFILL);
  xt = sysxt("OUT");
  OUT_A = xt ? get(xt + C) : 0;
}

static void tx(void)                    /* TX! ( c -- ) */
{
  int c = (int)(pop() & 255);
  if (c == 10) {
    printf("%s\n", ob);
    if (strstr(ob, "error:")) errseen = 1;
    ob_clear();
  } else if (c) ob_add(c);
}

/* ==== Files: HAL 89-93, 98, 99 (doc/vm.md, section 8) ===================
 * A file id (fid) is a number from 10 to 29. OPEN-FILE reads the whole
 * file into memory; READ-LINE and READ-FILE take the bytes from there.
 * WRITE-FILE writes into the file at once: CREATE-FILE makes it new,
 * OPEN-FILE with W/O or R/W writes at its end.
 */

#define NFID 20

static struct {
  int open;
  FILE *out;                            /* where WRITE-FILE writes, or NULL */
  char *buf;                            /* the file as OPEN-FILE found it */
  unsigned long len, pos;               /* its size, and the next byte to read */
} fds[NFID];

static int memstr(unsigned long a, unsigned long n, char *s, unsigned long size)   /* n bytes at a as a C string */
{
  unsigned long j;
  if (n >= size) return 0;
  for (j = 0; j < n; j++) s[j] = (char)getb(a + j);
  s[n] = 0;
  return 1;
}

static void ret2(unsigned long x, long y) { put(sp + C, x); put(sp, (unsigned long)y & MASK); }

static void ret3(unsigned long x, unsigned long y, long z)
{
  put(sp + 2 * C, x); put(sp + C, y); put(sp, (unsigned long)z & MASK);
}

static int fid(unsigned long x)         /* the index of an open fid, or -1 */
{
  return x >= 10 && x < 10 + NFID && fds[x - 10].open ? (int)(x - 10) : -1;
}

static void fopen_(int create)          /* OPEN-FILE, CREATE-FILE ( a u fam -- fid ior ) */
{
  unsigned long fam = pop() & 3, u = get(sp), a = get(sp + C), len = 0;
  char name[1024], *buf = NULL;
  FILE *out = NULL;
  int k;
  for (k = 0; k < NFID && fds[k].open; k++) ;
  if (k == NFID || !memstr(a, u, name, sizeof name)) { ret2(0, -37); return; }
  if (create) {
    remove(name);                       /* a new file, not the old one shorter: macOS keeps */
    out = fopen(name, "wb");            /* the signature of a program for each file */
    if (!out) { ret2(0, -37); return; }
  } else {
    if (fam != 1 && !slurp(name, &buf, &len)) { ret2(0, -38); return; }
    if (fam != 0) {
      out = fopen(name, "ab");
      if (!out) { free(buf); ret2(0, fam == 1 ? -38 : -37); return; }
    }
  }
  fds[k].open = 1; fds[k].out = out; fds[k].buf = buf; fds[k].len = len; fds[k].pos = 0;
  ret2((unsigned long)k + 10, 0);
}

static void fclose_(void)               /* CLOSE-FILE ( fid -- ior ) */
{
  int k = fid(get(sp));
  if (k < 0) { put(sp, (unsigned long)-37L & MASK); return; }
  if (fds[k].out) fclose(fds[k].out);
  free(fds[k].buf);
  fds[k].open = 0; fds[k].out = NULL; fds[k].buf = NULL;
  put(sp, 0);
}

static void freadline(void)             /* READ-LINE ( a u fid -- u2 f ior ) */
{
  int k = fid(get(sp));
  unsigned long u = get(sp + C), a = get(sp + 2 * C), e, n, j;
  if (k < 0) { ret3(0, 0, -37); return; }
  if (fds[k].pos >= fds[k].len) { ret3(0, 0, 0); return; }   /* the end of the file */
  for (e = fds[k].pos; e < fds[k].len && fds[k].buf[e] != '\n'; e++) ;
  n = e - fds[k].pos;
  if (n && fds[k].buf[e - 1] == '\r') n--;   /* CR LF ends a line too */
  if (n > u) { n = u; e = fds[k].pos + u - 1; }   /* the buffer is full: the rest of the line comes next */
  for (j = 0; j < n; j++) setb(a + j, (unsigned char)fds[k].buf[fds[k].pos + j]);
  fds[k].pos = e + 1;
  ret3(n, MASK, 0);
}

static void fread_(void)                /* READ-FILE ( a u fid -- u2 ior ) */
{
  int k = fid(pop());
  unsigned long u = get(sp), a = get(sp + C), n, j;
  if (k < 0) { ret2(0, -37); return; }
  n = fds[k].len - fds[k].pos;
  if (n > u) n = u;
  for (j = 0; j < n; j++) setb(a + j, (unsigned char)fds[k].buf[fds[k].pos + j]);
  fds[k].pos += n;
  ret2(n, 0);
}

static void fwrite_(void)               /* WRITE-FILE ( a u fid -- ior ) */
{
  int k = fid(pop());
  unsigned long u = pop(), a = get(sp), j;
  if (k < 0 || !fds[k].out) { put(sp, (unsigned long)-37L & MASK); return; }
  for (j = 0; j < u; j++) putc((int)getb(a + j), fds[k].out);
  put(sp, ferror(fds[k].out) ? (unsigned long)-37L & MASK : 0);
}

/* slurp: the whole file into memory, with a byte 0 after it. 0 if the
 * file cannot be read.
 */
static int slurp(const char *name, char **text, unsigned long *len)
{
  FILE *f = fopen(name, "rb");
  char *buf = NULL;
  unsigned long n = 0, cap = 0;
  size_t got;
  if (!f) return 0;
  for (;;) {
    if (cap - n < 4096) { cap = cap * 2 + 8192; buf = (char *)more(buf, cap + 1); }
    got = fread(buf + n, 1, (size_t)(cap - n), f);
    if (got == 0) break;
    n += (unsigned long)got;
  }
  fclose(f);
  buf[n] = 0;
  *text = buf;
  *len = n;
  return 1;
}

/* ==== Genesis: C compiles core.fs (doc/genesis.md) ======================
 * The outer interpreter: find the word, then execute or compile it; else
 * convert a number. PARSE-NAME, PARSE, HEADER, REVEAL, IMMEDIATE, ",",
 * "C,", "[" and "]" are host primitives until core.fs defines Forth
 * versions. Genesis handles "?:", "\" and "(" itself until core.fs has them.
 */

static unsigned long gln;               /* the line of core.fs */
static unsigned long gnt[16];           /* the headers that Genesis removes at the end */
static int ngnt;
static unsigned long boot_colon, boot_semi;   /* the first ":" and ";" (line 1 and 2 of core.fs) */
static int gskip, gskipf;               /* "?:" skips a definition, then its flags */
static unsigned long pa, pn;            /* the word that PARSE-NAME or PARSE found */

static void gfail(const char *s)
{
  char m[1200];
  const char *core = nfiles ? files[0] : core_file;
  sprintf(m, "error: %.1100s", s);
  msg(m);
  sprintf(m, "%.1000s:%lu: loading stopped", core, gln);
  msg(m);
  sprintf(m, "%.1000s is not complete; the system cannot start", core);
  msg(m);
  bye(1);
}

static void gerr(long n, const char *word)   /* a THROW code while Genesis runs */
{
  char m[400];
  if (word && *word) { sprintf(m, "%s: %.300s", throw_text(n), word); gfail(m); }
  gfail(throw_text(n));
}

static int is_gnt(unsigned long nt)
{
  int i;
  for (i = 0; i < ngnt; i++) if (gnt[i] == nt) return 1;
  return 0;
}

static void pname(void)                 /* PARSE-NAME on TIB: pa pn; >IN moves */
{
  unsigned long i = get(TOIN_A), n = get(NTIB_A), st;
  while (i < n && getb(TIB + i) <= 32) i++;
  st = i;
  while (i < n && getb(TIB + i) > 32) i++;
  pn = i - st;
  pa = TIB + st;
  if (i < n) i++;
  put(TOIN_A, i);
}

static void pparse(unsigned long c)     /* PARSE up to the character c: pa pn */
{
  unsigned long i = get(TOIN_A), n = get(NTIB_A), st = i;
  while (i < n && getb(TIB + i) != c) i++;
  pn = i - st;
  pa = TIB + st;
  if (i < n) i++;
  put(TOIN_A, i);
}

static void comma(unsigned long x)      /* , ( x -- ) */
{
  unsigned long t = get(DP_A);
  put(t - t % C, x);
  put(DP_A, t + C);
}

static unsigned long mkhead(const char *name, unsigned long wl, int flags)   /* a visible header; the result is the xt */
{
  unsigned long n = strlen(name), nt = get(DP_A), j;
  put(nt, get(wl));
  setb(nt + C, n + (unsigned long)flags);
  for (j = 0; j < n; j++) setb(nt + C + 1 + j, (unsigned char)name[j]);
  put(wl, nt);
  put(LATEST_A, nt);
  put(DP_A, ntxt(nt));
  return get(DP_A);
}

static void gheader(void)               /* HEADER ( a u -- ): a new header in CURRENT, not visible yet */
{
  unsigned long n = pop(), a = pop(), nt, xt, j, r;
  char name[32];
  if (n == 0) gerr(-16, NULL);
  if (n > 31) gerr(-19, NULL);
  memstr(a, n, name, sizeof name);
  keymem(a, n);
  r = wlfind(get(CURRENT_A));
  if (r && !is_gnt(r)) { sprintf(mbuf, "redefined %s", name); msg(mbuf); }
  while (get(DP_A) % C) { setb(get(DP_A), 0); put(DP_A, get(DP_A) + 1); }
  nt = get(DP_A);
  xt = (nt + C + 1 + n + C - 1) / C * C;
  if (xt + 4 * C > DICT_END) gerr(-8, NULL);
  put(nt, get(get(CURRENT_A)));         /* the link: the newest word of CURRENT */
  setb(nt + C, n);
  for (j = 0; j < n; j++) setb(nt + C + 1 + j, getb(a + j));
  for (j = nt + C + 1 + n; j < xt; j++) setb(j, 0);
  put(DP_A, xt);
  put(LATEST_A, nt);
  if (n == 1 && name[0] == ':' && !boot_colon) { boot_colon = nt; gnt[ngnt++] = nt; }
  if (n == 1 && name[0] == ';' && !boot_semi) { boot_semi = nt; gnt[ngnt++] = nt; }
}

static int gnumber(const char *s, unsigned long *value)   /* 0 if s is no number */
{
  unsigned long base = get(BASE_A), v = 0, d, n = strlen(s);
  int neg = 0, over = 0;
  const char *p = s;
  if (n == 3 && s[0] == '\'' && s[2] == '\'') { *value = (unsigned char)s[1]; return 1; }
  if (p[0] == '-' && p[1] && strchr("$#%", p[1])) { neg = 1; p++; }
  if (*p == '$') { base = 16; p++; }
  else if (*p == '#') { base = 10; p++; }
  else if (*p == '%') { base = 2; p++; }
  if (*p == '-') { neg = !neg; p++; }
  if (!*p || base < 2 || base > 36) return 0;
  for (; *p; p++) {
    unsigned long c = (unsigned char)*p;
    if (c >= 48 && c <= 57) d = c - 48;          /* 0-9 */
    else if (c >= 65 && c <= 90) d = c - 55;     /* A-Z */
    else if (c >= 97 && c <= 122) d = c - 87;    /* a-z */
    else return 0;
    if (d >= base) return 0;
    if (!over) {
      if (v > (MASK - d) / base) over = 1;
      else v = v * base + d;
    }
  }
  if (over || (neg && v > SIGN)) gerr(-24, s);
  *value = neg ? (0 - v) & MASK : v;
  return 1;
}

static void gexec(unsigned long xt)     /* execute xt; through CATCH once core.fs has it */
{
  unsigned long code;
  if (XT_CATCH) {
    push(xt);
    run_xt(XT_CATCH);
    code = pop();
    if (code) gerr(sgn(code), NULL);
  } else run_xt(xt);
}

static void gline(const char *line, unsigned long n)   /* interpret one line of core.fs */
{
  unsigned long j, nt, xt, fl, save, num;
  char word[260], up[260];
  if (n > 256) {
    sprintf(mbuf, "line too long (%lu bytes, at most 256)", n);
    gfail(mbuf);
  }
  for (j = 0; j < n; j++) setb(TIB + j, (unsigned char)line[j]);
  put(NTIB_A, n);
  put(TOIN_A, 0);
  for (;;) {
    pname();
    if (!pn) return;
    memstr(pa, pn, word, sizeof word);
    if (gskip) {                        /* "?:" skips a definition */
      if (!strcmp(word, "\\")) put(TOIN_A, get(NTIB_A));
      else if (!strcmp(word, "(")) pparse(41);
      else if (!strcmp(word, ";")) { gskip = 0; gskipf = 1; }
      continue;
    }
    if (gskipf) {                       /* and the flags after it */
      for (j = 0; j <= pn; j++) up[j] = (char)upper((unsigned char)word[j], 0);
      if (!strcmp(up, "IMMEDIATE") || !strcmp(up, "UNCHECKED") || !strcmp(up, "COMPILE-ONLY")) continue;
      gskipf = 0;
    }
    if (!strcmp(word, "?:")) {
      save = get(TOIN_A);
      pname();
      if (!pn) gerr(-16, NULL);
      keymem(pa, pn);
      if (gfind()) { gskip = 1; continue; }
      put(TOIN_A, save);
      keystr(":");
      gexec(ntxt(gfind()));
      continue;
    }
    keymem(pa, pn);
    nt = gfind();
    if (nt) {
      xt = ntxt(nt);
      fl = getb(nt + C);
      if (get(STATE_A) == 0 || fl & 128) {
        if (get(STATE_A) == 0 && fl & 64) gerr(-14, word);
        gexec(xt);
      } else comma(xt);
      continue;
    }
    if (!strcmp(word, "\\")) { put(TOIN_A, get(NTIB_A)); continue; }   /* comments until core.fs defines them */
    if (!strcmp(word, "(")) { pparse(41); continue; }
    if (!gnumber(word, &num)) gerr(-13, word);
    if (get(STATE_A)) { comma(XT_LIT); comma(num); } else push(num);
  }
}

static void genesis(void)               /* compile core.fs; then remove the host primitives */
{
  unsigned long a = SYS, xt, wl, nt, prev, nx, value;
  int i;
  Lines L;
  put(HALT_XT, 127);
  put(HALT_ADDR, HALT_XT);
  put(DP_A, DICT_START + GUARD);
  put(VP_A, VP_START);
  put(BASE_A, 10);
  put(WL_FORTH + C, 0);
  put(WL_INTERNALS + C, WL_FORTH);
  put(VOCLINK_A, WL_INTERNALS);
  sp = S0; rp = R0;
  for (i = 0; i < NPRIM; i++) {         /* a header for each primitive */
    if (!with_header(PRIM[i].op)) continue;
    xt = mkhead(PRIM[i].name, PRIM[i].voc == 'F' ? WL_FORTH : WL_INTERNALS, PRIM[i].flags);
    put(xt, (unsigned long)PRIM[i].op);
    put(DP_A, xt + C);
    if (PRIM[i].op == 5) XT_LIT = xt;
    if (PRIM[i].op >= 100) gnt[ngnt++] = get(LATEST_A);
  }
  for (i = 0; i < NSV + 9; i++) {       /* constants for the system variables, then 9 more */
    static const char *more_names[] = { "TIB", "PAD", "S0", "R0", "DATA-END",
                                        "WL-FORTH", "WL-INTERNALS", "CELL", "DICT-END" };
    unsigned long more_values[9];
    const char *name;
    more_values[0] = TIB; more_values[1] = PAD; more_values[2] = S0; more_values[3] = R0;
    more_values[4] = DATA_END; more_values[5] = WL_FORTH; more_values[6] = WL_INTERNALS;
    more_values[7] = C; more_values[8] = DICT_END;
    if (i < NSV) { name = SV[i].name; value = a; a += (unsigned long)SV[i].cells * C; }
    else { name = more_names[i - NSV]; value = more_values[i - NSV]; }
    xt = mkhead(name, i < NSV && SV[i].voc == 'F' ? WL_FORTH : WL_INTERNALS, 0);
    put(xt, 2);
    put(xt + C, value);
    put(DP_A, xt + 2 * C);
  }
  put(NORDER_A, 2);
  put(CONTEXT_A, WL_FORTH);
  put(CONTEXT_A + C, WL_INTERNALS);
  put(CURRENT_A, WL_FORTH);
  if (!readlines(files[0], &L)) {
    sprintf(mbuf, "cannot read %.1000s", files[0]);
    msg(mbuf);
    bye(1);
  }
  for (gln = 1; gln <= L.count; gln++) {
    gline(L.text + L.start[gln - 1], L.len[gln - 1]);
    if (!XT_CATCH) { keystr("CATCH"); nt = gfind(); if (nt) XT_CATCH = ntxt(nt); }
  }
  gln = L.count;
  freelines(&L);
  for (wl = get(VOCLINK_A); wl; wl = get(wl + C)) {   /* remove the host primitives and the first ":" ";" */
    prev = 0;
    for (nt = get(wl); nt; nt = nx) {
      nx = get(nt);
      if (is_gnt(nt)) { if (prev) put(prev, nx); else put(wl, nx); }
      else prev = nt;
    }
  }
}

/* ==== Images: Intel HEX ==================================================
 * An image is a text file (doc/vm.md, section 10). The first line names
 * the cell size and the start words; then come Intel HEX records for the
 * system variables, the RAM data up to VP and the dictionary up to DP.
 */

static unsigned long ulba;              /* the upper 16 bits of the addresses so far */

static void hexseg(unsigned long a, unsigned long e, FILE *f)   /* the bytes from a up to e (exclusive) */
{
  unsigned long n, j, b, sum, hi;
  while (a < e) {
    hi = a >> 16;
    if (hi != ulba) {                   /* extended linear address: the upper 16 bits */
      ulba = hi;
      fprintf(f, ":02000004%04lX%02lX\n", hi, (256 - (6 + (hi >> 8) + (hi & 255)) % 256) % 256);
    }
    n = e - a;
    if (n > 16) n = 16;
    if ((a & 0xFFFFUL) + n > 65536UL) n = 65536UL - (a & 0xFFFFUL);
    sum = n + (a >> 8 & 255) + (a & 255);
    fprintf(f, ":%02lX%04lX00", n, a & 0xFFFFUL);
    for (j = 0; j < n; j++) { b = getb(a + j); sum += b; fprintf(f, "%02lX", b); }
    fprintf(f, "%02lX\n", (256 - sum % 256) % 256);
    a += n;
  }
}

static int write_image(const char *name, unsigned long boot)   /* 1 if it works */
{
  FILE *f;
  remove(name);                         /* a new file (see CREATE-FILE) */
  f = fopen(name, "wb");
  if (!f) return 0;
  ulba = 0;                             /* a record for the upper bits only above $FFFF */
  fprintf(f, "; ucf image bits=%d boot=%lu quit=%lu throw=%lu prims=%s\n",
          BITS, boot, XT_QUIT, XT_THROW, prims);
  hexseg(SYS, DATA_START, f);
  hexseg(DATA_START, get(VP_A), f);
  hexseg(DICT_START, get(DP_A), f);
  fprintf(f, ":00000001FF\n");
  return fclose(f) == 0;
}

static int hexval(int c)                /* the value of a hex digit, or -1 */
{
  if (c >= 48 && c <= 57) return c - 48;
  if (c >= 65 && c <= 70) return c - 55;
  if (c >= 97 && c <= 102) return c - 87;
  return -1;
}

static unsigned long hex2(const char *s)   /* two hex digits */
{
  return (unsigned long)(hexval((unsigned char)s[0]) * 16 + hexval((unsigned char)s[1]));
}

static char meta[512];                  /* the first line of an image */
static unsigned long ndiff;             /* -check: the bytes that differ */
static char firstdiff[80];

static void hexread(const char *name, int compare)   /* the records into memory, or compare them with it */
{
  Lines L;
  unsigned long k, n, a, t, j, b, sum, hi = 0, r;
  const char *s;
  ndiff = 0; firstdiff[0] = 0;
  if (!readlines(name, &L)) { fprintf(stderr, "ucf-c: cannot read %s\n", name); exit(1); }
  for (k = 0; k < L.count; k++) {
    s = L.text + L.start[k];
    if (s[0] == ';' && L.len[k] < sizeof meta) {   /* a comment; the first line has " boot=" */
      char line[sizeof meta];
      memcpy(line, s, (size_t)L.len[k]);
      line[L.len[k]] = 0;
      if (strstr(line, " boot=")) strcpy(meta, line);
      continue;
    }
    if (s[0] != ':' || L.len[k] < 11) continue;
    n = hex2(s + 1); a = hex2(s + 3) * 256 + hex2(s + 5); t = hex2(s + 7);
    if (L.len[k] < 11 + 2 * n) { fprintf(stderr, "ucf-c: %s:%lu: short record\n", name, k + 1); exit(1); }
    sum = n + (a >> 8) + (a & 255) + t + hex2(s + 9 + 2 * n);
    for (j = 0; j < n; j++) sum += hex2(s + 9 + 2 * j);
    if (sum % 256) { fprintf(stderr, "ucf-c: %s:%lu: wrong checksum\n", name, k + 1); exit(1); }
    if (t == 0) {
      for (j = 0; j < n; j++) {
        b = hex2(s + 9 + 2 * j);
        if (!compare) setb(hi + a + j, b);
        else {
          r = getb(hi + a + j);
          if (r != b && ndiff++ == 0)
            sprintf(firstdiff, "$%lX (here %lu, there %lu)", hi + a + j, r, b);
        }
      }
    } else if (t == 1) break;
    else if (t == 4) hi = (hex2(s + 9) * 256 + hex2(s + 11)) << 16;
  }
  freelines(&L);
}

static unsigned long metaval(const char *field)   /* the number after " field=" in the first line */
{
  char pat[32];
  const char *p;
  sprintf(pat, " %.20s=", field);
  p = strstr(meta, pat);
  return p ? strtoul(p + strlen(pat), NULL, 10) : 0;
}

static void load_image(const char *name)   /* memory, start words, primitives */
{
  const char *p;
  unsigned long lo, hi, op;
  char *end;
  meta[0] = 0;
  hexread(name, 0);
  if (!meta[0]) { fprintf(stderr, "ucf-c: %s is not an image of uCForth\n", name); exit(1); }
  XT_BOOT = metaval("boot"); XT_QUIT = metaval("quit"); XT_THROW = metaval("throw");
  p = strstr(meta, " prims=");
  if (!p) return;
  for (p += 7; *p >= 48 && *p <= 57; ) {   /* each range lo-hi: this VM must have it */
    lo = hi = strtoul(p, &end, 10);
    p = end;
    if (*p == '-') { hi = strtoul(p + 1, &end, 10); p = end; }
    for (op = lo; op <= hi; op++)
      if (!have(op)) { fprintf(stderr, "ucf-c: %s needs primitive %lu\n", name, op); exit(1); }
    if (*p == ',') p++;
  }
}

static int checkfail;

static void check_image(const char *what, const char *name)   /* compare the memory with a reference */
{
  if (!readable(name)) { sprintf(mbuf, "check %s: cannot read %.1000s", what, name); msg(mbuf); checkfail = 1; return; }
  hexread(name, 1);
  if (ndiff) {
    sprintf(mbuf, "check %s: %lu bytes differ from %.1000s, the first at %s", what, ndiff, name, firstdiff);
    msg(mbuf);
    checkfail = 1;
  } else {
    sprintf(mbuf, "check %s: the same as %.1000s", what, name);
    msg(mbuf);
  }
}

static void save_image(void)            /* SAVE-IMAGE ( -- ior ) */
{
  const char *f = getenv("UCF_IMAGE");
  if (!f || !*f) f = "ucf.hex";
  if (write_image(f, XT_BOOT ? XT_BOOT : XT_QUIT)) {
    sprintf(mbuf, "saved: %.1000s", f);
    msg(mbuf);
    push(0);
  } else push((unsigned long)-37L);
}

/* ==== The HAL (doc/vm.md, section 8) and the host primitives ============ */

static unsigned long T0;                /* the clock at the start */

static void wait_ms(unsigned long ms)   /* in steps of 100 ms, so that Ctrl-C comes through */
{
  unsigned long step;
  while (ms > 0 && !brk) {
    step = ms > 100 ? 100 : ms;
    wait_us(step * 1000);
    ms -= step;
  }
}

static void hal(unsigned long op)
{
  unsigned long a, t;
  switch (op) {
  case 80: push(now_ms() - T0); break;                                /* MILLIS */
  case 81: wait_ms(pop()); break;                                     /* MS */
  case 82: wait_us(pop()); break;                                     /* US */
  case 83: a = pop(); t = pop() & 255; setb(a, getb(a) | t); break;   /* BIS! */
  case 84: a = pop(); t = pop() & 255; setb(a, getb(a) & ~t); break;  /* BIC! */
  case 85: save_image(); break;                                       /* SAVE-IMAGE */
  case 86: break;                                                     /* ERASE-IMAGE */
  case 87: reboot(); break;                                           /* REBOOT */
  case 88: bye(0); break;                                             /* BYE */
  case 89: fopen_(0); break;                                          /* OPEN-FILE */
  case 90: fopen_(1); break;                                          /* CREATE-FILE */
  case 91: fclose_(); break;                                          /* CLOSE-FILE */
  case 92: fread_(); break;                                           /* READ-FILE */
  case 93: fwrite_(); break;                                          /* WRITE-FILE */
  case 98: freadline(); break;                                        /* READ-LINE */
  case 99: bye((int)(pop() & 255)); break;                            /* (BYE) */
  case 100: pname(); push(pa); push(pn); break;                       /* PARSE-NAME */
  case 101: pparse(pop() & 255); push(pa); push(pn); break;           /* PARSE */
  case 102: gheader(); break;                                         /* HEADER */
  case 103: t = get(LATEST_A); if (t) put(get(CURRENT_A), t); break;  /* REVEAL */
  case 104: a = get(LATEST_A) + C; setb(a, getb(a) | 128); break;     /* IMMEDIATE */
  case 105: comma(pop()); break;                                      /* , */
  case 106: t = get(DP_A); setb(t, pop()); put(DP_A, t + 1); break;   /* C, */
  case 107: put(STATE_A, 0); break;                                   /* [ */
  case 108: put(STATE_A, MASK); break;                                /* ] */
  default: throw_(-21);                                               /* an unknown primitive */
  }
}

/* ==== Start ============================================================== */

static unsigned long words_in_forth(void)
{
  unsigned long n = 0, nt;
  for (nt = get(WL_FORTH); nt; nt = get(nt)) n++;
  return n;
}

static void core_end(void)              /* Genesis is complete: the seed, and its check */
{
  char name[64], what[16];
  if (dump_core_file && !write_image(dump_core_file, XT_QUIT)) {
    sprintf(mbuf, "cannot write %.1000s", dump_core_file);
    msg(mbuf);
  }
  if (check) {
    sprintf(name, "%sseed/core%d.hex", pfx, BITS);
    sprintf(what, "core%d", BITS);
    check_image(what, name);
  }
}

static void build_end(void)             /* base.fs is complete (doc/genesis.md, section 6) */
{
  unsigned long here = get(DP_A);
  char name[64], what[16];
  put(BASEEND_A, here);
  put(FROZENEND_A, here);
  put(NORDER_A, 1);
  put(CONTEXT_A, WL_FORTH);
  put(CURRENT_A, WL_FORTH);
  put(STATE_A, 0);
  build_at = -2;
  XT_BOOT = sysxt("BOOT");
  if (dump_file && !write_image(dump_file, XT_BOOT)) {
    sprintf(mbuf, "cannot write %.1000s", dump_file);
    msg(mbuf);
  }
  if (check) {
    ob_clear();                         /* the prompt of QUIT is not wanted here */
    sprintf(name, "%sseed/base%d.hex", pfx, BITS);
    sprintf(what, "base%d", BITS);
    check_image(what, name);
    bye(checkfail);
  }
  printf("uCForth - C bootstrap VM - %d bit%s - %lu words in FORTH - built in %lu ms\n",
         BITS, pure ? " - pure" : "", words_in_forth(), now_ms() - T0);
}

int main(int argc, char **argv)
{
  int i;
  options(argc, argv);
  memory_map();
  primitives();
  T0 = now_ms();
  tty = is_tty();
  ob = (char *)more(NULL, 256); obcap = 256; ob_clear();
  kl = (char *)more(NULL, 256); kl_cap = 256;
  in_str = (char *)more(NULL, 258); in_cap = 258;
  files = (char **)more(NULL, ((unsigned long)nargs + 2) * sizeof *files);
  if (image_file) {
    load_image(image_file);
    for (i = 0; i < nargs; i++) files[nfiles++] = args[i];
    find_words();
    printf("uCForth - C bootstrap VM - %d bit - %lu words in FORTH - image %s\n",
           BITS, words_in_forth(), image_file);
    sp = S0; rp = R0;
    put(STATE_A, 0);
    put(HANDLER_A, 0);
    catch_int();
    run_xt(XT_BOOT ? XT_BOOT : XT_QUIT);
  } else {
    files[nfiles++] = (char *)core_file;
    files[nfiles++] = (char *)base_file;
    for (i = 0; i < nargs; i++) files[nfiles++] = args[i];
    genesis();
    XT_THROW = sysxt("THROW");
    XT_QUIT = sysxt("QUIT");
    if (!XT_QUIT) { sprintf(mbuf, "%.1000s does not define QUIT", files[0]); gfail(mbuf); }
    find_words();
    core_end();
    fi = 1;
    build_at = 1;
    catch_int();
    run_xt(XT_QUIT);
  }
  bye(0);
  return 0;
}
