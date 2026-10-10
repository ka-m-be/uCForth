// ucf.js - the JavaScript bootstrap VM of uCForth
//
// It is the virtual machine of uCForth in JavaScript: the memory, the
// registers, the primitives and a small HAL (doc/vm.md). Its Genesis, a
// small outer interpreter, compiles forth/core.fs once (doc/genesis.md).
// From then on QUIT in the image reads each line, as on a board: first
// forth/base.fs, then the files of the command line, then the keyboard
// (or a pipe).
//
// The same file runs in a web browser (ucf.html) and in Node.js. The
// parts and their names are the same as in the C bootstrap VM
// (bootstrap/c/ucf.c, the reference); the README in this folder tells
// what is special in JavaScript.
//
// Use in Node.js:  node bootstrap/js/ucf.js [OPTION ...] [FILE ...]
// In a browser:    open bootstrap/js/ucf.html (README.md)
//
//   FILE              a source file for QUIT; then the keyboard
//   -32               32-bit cells (default: 16 bit)
//   -pure             only the 32 mandatory primitives and the HAL: the
//                     words with "?:" stay Forth words
//   -core FILE        Genesis compiles FILE (default: forth/core.fs)
//   -base FILE        QUIT compiles FILE first (default: forth/base.fs)
//   -image FILE       start from an image, not from core and base
//   -dump-core FILE   write the image after core to FILE (the seed)
//   -dump FILE        write the image after base to FILE
//   -check            build with -pure, compare the images with
//                     seed/core16.hex and seed/base16.hex (32 bit: ...32),
//                     then stop; exit status 0 if they are the same
//
// SAVE-IMAGE (and so FREEZE) writes the image to ucf.hex, or to the file
// in the environment variable UCF_IMAGE. Ctrl-C stops the word that runs.
//
// Limits:
// - ?RX gets whole lines: the terminal sends a line with the return key.
// - No checks of the stacks. An address wraps around in the memory of
//   the VM (64 KB or 256 KB), so no address reaches other memory.
// - REBOOT starts BOOT or QUIT again, but does not load the image again.
//   ERASE-IMAGE does nothing.
// - OPEN-FILE with W/O or R/W writes at the end of the file.
// - TX! does not send the byte 0.

'use strict';

// makeVM: a new VM for a host. The host is the system around the VM:
// Node.js or the web page (at the end of this file, and in ucf.html).
//
//   host.tty                 true: a person types; the terminal shows the typed lines
//   host.poll                true: a word that asks for a key does not wait for a
//                            line (a web page): ?RX answers "no key"
//   host.write(bytes)        output
//   host.error(text)         a message of the program itself (stderr)
//   host.readFile(name)      the bytes of the file, or null
//   host.openWrite(name, fresh)   a handle to write to: fresh: a new file,
//                            else at the end of the file; null if it fails
//   host.writeTo(h, bytes)   false if it fails
//   host.closeFile(h)
//   host.env(name)           an environment variable, or ''
//   host.now()               the clock in milliseconds
//   host.later(fn, ms)       run fn after ms milliseconds
//   host.exit(code)          the end of the program
//
// The VM gives back: start(args), input(line), end() (no more input),
// interrupt() (Ctrl-C).

function makeVM(host) {

  // ==== The system ======================================================
  // JavaScript cannot wait: the VM runs in slices (run). A primitive that
  // must wait (?RX for a line, MS) ends the slice; the host calls run
  // again when the line comes, or after the time.

  let brk = false;                      // Ctrl-C came: stop the word that runs
  const SLICE = 1000000;                // steps of the VM in one slice
  const DONE = 0, MORE = 1, INPUT = 2, WAIT = 3;   // how a slice ends
  let waitUntil = 0;                    // MS: the time to go on
  let pendingRx = false;                // ?RX waits for a line: it runs again when the line comes
  let running = false, stopped = false;

  class Bye {                           // BYE: the program ends; a throw leaves each loop
    constructor(code) { this.code = code; }
  }

  const bytesOf = (s) => Uint8Array.from(s, (ch) => ch.charCodeAt(0) & 255);
  const textOf = (b) => String.fromCharCode.apply(null, Array.from(b));

  // ==== Options =========================================================

  let BITS = 16, pure = false, check = false;
  let coreFile, baseFile, imageFile = null, dumpFile = null, dumpCoreFile = null;
  let pfx = '';                         // '' or '../../': where the project is
  const args = [];

  function usage() {
    host.error('use: ucf.js [-32] [-pure] [-core FILE] [-base FILE] [-image FILE]\n' +
               '            [-dump-core FILE] [-dump FILE] [-check] [FILE ...]\n');
    throw new Bye(2);
  }

  function options(argv) {             // the default files: in the current folder (ucf), or two above
    if (!host.readFile('forth/core.fs') && host.readFile('../../forth/core.fs')) pfx = '../../';
    coreFile = pfx + 'forth/core.fs';
    baseFile = pfx + 'forth/base.fs';
    for (let i = 0; i < argv.length; i++) {
      const a = argv[i];
      if (a === '--') continue;
      if (a === '-32') BITS = 32;
      else if (a === '-pure') pure = true;
      else if (a === '-check') check = pure = true;
      else if (['-core', '-base', '-image', '-dump-core', '-dump'].includes(a)) {
        if (i + 1 >= argv.length) usage();
        const n = argv[++i];
        if (a === '-core') coreFile = n;
        else if (a === '-base') baseFile = n;
        else if (a === '-image') imageFile = n;
        else if (a === '-dump-core') dumpCoreFile = n;
        else dumpFile = n;
      } else if (a.startsWith('-')) usage();
      else args.push(a);
    }
    if (imageFile) {                    // the first line of an image names the cell size
      const text = host.readFile(imageFile);
      if (!text) { host.error('ucf.js: cannot read ' + imageFile + '\n'); throw new Bye(1); }
      BITS = textOf(text.subarray(0, 200)).includes('bits=32') ? 32 : 16;
    }
  }

  // ==== Memory map (doc/vm.md, section 3) ===============================
  // The memory of the VM is a Uint8Array. A cell is 2 or 4 bytes, the low
  // byte first (little endian).

  let mem, C, MASK, SIGN, AMASK, SYS, DATA_START, DATA_END, TIB, PAD, R0, S0;
  let DICT_START, GUARD, DICT_END, MEM_END, HALT_XT, HALT_ADDR, RUN_ADDR, THROW_ADDR;
  let WL_FORTH, WL_INTERNALS, VP_START;
  let DP_A, VP_A, LATEST_A, STATE_A, BASE_A, TOIN_A, NTIB_A, CURRENT_A, NORDER_A;
  let CONTEXT_A, VOCLINK_A, CASESENSE_A, BASEEND_A, FROZENEND_A, HANDLER_A;
  let get, put, cell, sgn;

  function memoryMap() {
    const k = BITS === 16 ? 1 : 2;
    C = BITS / 8;                       // bytes in a cell: 2 or 4
    MASK = BITS === 16 ? 0xFFFF : 0xFFFFFFFF;
    SIGN = BITS === 16 ? 0x8000 : 0x80000000;
    SYS = 256;
    DATA_START = 512 * k;
    DATA_END = k === 1 ? 4096 : 65536;
    TIB = DATA_END;
    PAD = TIB + 256;
    R0 = PAD + 256 + 256 * C;           // the return stack: 256 cells below R0
    S0 = R0 + 256 * C;                  // the data stack: 256 cells below S0
    DICT_START = S0;
    GUARD = 8 * C;                      // the first word comes 8 cells above S0:
                                        // a stack underflow lands in these free cells
    MEM_END = k === 1 ? 65536 : 262144;
    DICT_END = MEM_END - 16;
    AMASK = MEM_END - 1;                // addresses wrap around
    HALT_XT = DATA_START - 16;          // the two HALT cells
    HALT_ADDR = HALT_XT + C;
    RUN_ADDR = MEM_END - 2 * C;         // two cells at the top: run one word, then HALT
    THROW_ADDR = MEM_END - 4 * C;       // two cells at the top: run THROW
    WL_FORTH = DATA_START;
    WL_INTERNALS = WL_FORTH + 3 * C;
    VP_START = WL_INTERNALS + 3 * C;
    mem = new Uint8Array(MEM_END + 4);  // 4 more bytes: a cell at the end needs no test
    // the system variables (doc/vm.md, section 4), one cell each from SYS
    [DP_A, VP_A, LATEST_A, STATE_A, BASE_A, TOIN_A, NTIB_A, CURRENT_A, NORDER_A] =
      [0, 1, 2, 3, 4, 5, 6, 7, 8].map((i) => SYS + i * C);
    CONTEXT_A = SYS + 9 * C;
    [VOCLINK_A, CASESENSE_A, BASEEND_A, FROZENEND_A] = [25, 26, 27, 28].map((i) => SYS + i * C);
    HANDLER_A = SYS + 32 * C;
    if (C === 2) {
      get = (a) => { a &= AMASK; return mem[a] | mem[a + 1] << 8; };
      put = (a, x) => { a &= AMASK; mem[a] = x; mem[a + 1] = x >>> 8; };
      cell = (x) => x & 0xFFFF;         // a number as a cell
      sgn = (x) => (x << 16) >> 16;     // a cell as a signed number
    } else {
      get = (a) => { a &= AMASK; return (mem[a] | mem[a + 1] << 8 | mem[a + 2] << 16 | mem[a + 3] << 24) >>> 0; };
      put = (a, x) => { a &= AMASK; mem[a] = x; mem[a + 1] = x >>> 8; mem[a + 2] = x >>> 16; mem[a + 3] = x >>> 24; };
      cell = (x) => x >>> 0;
      sgn = (x) => x | 0;
    }
  }

  // The names of the system variables for Genesis: name, cells, vocabulary
  // (F = FORTH, I = INTERNALS). The addresses follow one after the other.
  const SV = [['DP', 1, 'I'], ['VP', 1, 'I'], ['LATEST', 1, 'I'], ['STATE', 1, 'I'],
    ['BASE', 1, 'F'], ['>IN', 1, 'I'], ['#TIB', 1, 'I'], ['CURRENT', 1, 'I'],
    ['#ORDER', 1, 'I'], ['CONTEXT', 16, 'I'], ['VOC-LINK', 1, 'I'],
    ['CASE-SENSE', 1, 'I'], ['BASE-END', 1, 'I'], ['FROZEN-END', 1, 'I'],
    ['AUTORUN', 1, 'I'], ['ERRMSG', 2, 'I'], ['HANDLER', 1, 'I']];

  // ==== Primitives ======================================================
  // Number, name, vocabulary, flags (64 = compile-only, 128 = immediate).
  // 0-3 are the code field types, 4-31 the other mandatory primitives,
  // 32-65 the optional ones, 80-88 the HAL, 100-108 exist only for Genesis.

  const PRIM = [[4, 'EXIT', 'F', 64], [5, 'LIT', 'I', 64], [6, 'BRANCH', 'I', 64],
    [7, '?BRANCH', 'I', 64], [8, 'EXECUTE', 'F', 0], [9, '@', 'F', 0],
    [10, '!', 'F', 0], [11, 'C@', 'F', 0], [12, 'C!', 'F', 0],
    [13, 'DUP', 'F', 0], [14, 'DROP', 'F', 0], [15, 'SWAP', 'F', 0],
    [16, 'OVER', 'F', 0], [17, '>R', 'F', 64], [18, 'R>', 'F', 64],
    [19, 'R@', 'F', 64], [20, 'SP@', 'I', 0], [21, 'SP!', 'I', 0],
    [22, 'RP@', 'I', 0], [23, 'RP!', 'I', 0], [24, 'AND', 'F', 0],
    [25, 'OR', 'F', 0], [26, 'XOR', 'F', 0], [27, '0<', 'F', 0],
    [28, 'UM+', 'I', 0], [29, '?RX', 'I', 0], [30, 'TX!', 'I', 0],
    [31, '!IO', 'I', 0],
    [32, '+', 'F', 0], [33, '-', 'F', 0], [34, '1+', 'F', 0],
    [35, '1-', 'F', 0], [36, 'NEGATE', 'F', 0], [37, 'INVERT', 'F', 0],
    [38, '2*', 'F', 0], [39, '2/', 'F', 0], [40, 'LSHIFT', 'F', 0],
    [41, 'RSHIFT', 'F', 0], [42, '=', 'F', 0], [43, '0=', 'F', 0],
    [44, '<', 'F', 0], [45, 'U<', 'F', 0], [46, '*', 'F', 0],
    [47, 'UM*', 'F', 0], [48, 'UM/MOD', 'F', 0], [49, 'ROT', 'F', 0],
    [50, 'NIP', 'F', 0], [51, '?DUP', 'F', 0], [52, '2DUP', 'F', 0],
    [53, '2DROP', 'F', 0], [54, '+!', 'F', 0], [55, 'MOVE', 'F', 0],
    [56, 'FILL', 'F', 0], [57, 'COUNT', 'F', 0], [58, 'FIND-NAME', 'I', 0],
    [59, '(DO)', 'I', 64], [60, '(?DO)', 'I', 64], [61, '(LOOP)', 'I', 64],
    [62, '(+LOOP)', 'I', 64], [63, 'I', 'F', 64], [64, 'J', 'F', 64],
    [65, 'UNLOOP', 'F', 64],
    [80, 'MILLIS', 'F', 0], [81, 'MS', 'F', 0], [82, 'US', 'F', 0],
    [83, 'BIS!', 'F', 0], [84, 'BIC!', 'F', 0], [85, 'SAVE-IMAGE', 'I', 0],
    [86, 'ERASE-IMAGE', 'I', 0], [87, 'REBOOT', 'I', 0], [88, 'BYE', 'F', 0],
    [100, 'PARSE-NAME', 'I', 0], [101, 'PARSE', 'I', 0], [102, 'HEADER', 'I', 0],
    [103, 'REVEAL', 'I', 0], [104, 'IMMEDIATE', 'F', 0], [105, ',', 'F', 0],
    [106, 'C,', 'F', 0], [107, '[', 'F', 128], [108, ']', 'F', 0]];

  let prims = '';                       // the primitives with a header, as "0-31,80-88"
  const withHeader = (op) => op < 32 || op >= 80 || !pure;   // does Genesis make a header for op?
  const have = (op) => op <= 65 || (op >= 80 && op <= 93) || op === 98 || op === 99;

  function primitives() {               // prims: the numbers below 100 with a header, as ranges
    const ops = [0, 1, 2, 3].concat(PRIM.map((p) => p[0]).filter((op) => op < 100 && withHeader(op)));
    const ranges = [];
    let first = ops[0];
    for (let i = 1; i <= ops.length; i++) {
      if (i < ops.length && ops[i] === ops[i - 1] + 1) continue;
      const prev = ops[i - 1];
      ranges.push(first === prev ? String(first) : first + '-' + prev);
      if (i < ops.length) first = ops[i];
    }
    prims = ranges.join(',');
  }

  // ==== Helpers =========================================================

  const getb = (a) => mem[a & AMASK];
  const setb = (a, b) => { mem[a & AMASK] = b; };

  let ip = 0, w = 0, sp = 0, rp = 0;    // the registers (doc/vm.md, section 5)

  const push = (x) => { sp -= C; put(sp, cell(x)); };
  const pop = () => { const x = get(sp); sp += C; return x; };
  const ntxt = (nt) => Math.floor((nt + C + 1 + (getb(nt + C) & 31) + C - 1) / C) * C;

  let ob = [];                          // the output line that does not show yet (bytes)

  function flushLine() { host.write(Uint8Array.from(ob.concat([10]))); ob = []; }

  function showOutput() {               // host.poll: a word waits; its output so far shows
    if (host.poll && ob.length && !curfile && fi >= files.length) { host.write(Uint8Array.from(ob)); ob = []; }
  }

  function msg(s) {                     // a message of this program on its own line
    if (ob.length) flushLine();
    host.write(bytesOf('  ' + s + '\n'));
  }

  function bye(n) {
    if (ob.length) flushLine();
    throw new Bye(n);
  }

  const THROW_TEXT = {
    '-3': 'data stack overflow', '-4': 'data stack underflow', '-8': 'dictionary overflow',
    '-10': 'division by zero', '-11': 'result out of range', '-13': 'undefined word',
    '-14': 'compile-only word', '-16': 'missing name', '-19': 'name too long',
    '-21': 'unsupported operation', '-24': 'number out of range'
  };
  const throwText = (n) => THROW_TEXT[String(n)] || 'error ' + n;

  // ==== Search in the dictionary ========================================
  // key holds the name; without CASE-SENSE in upper case.

  let key = [];
  const upper = (c, cs) => (!cs && c >= 97 && c <= 122 ? c - 32 : c);

  function keymem(a, n) {               // the key from memory
    const cs = get(CASESENSE_A);
    key = [];
    for (let j = 0; j < n; j++) key.push(upper(getb(a + j), cs));
  }

  function keystr(s) {                  // the key from a string
    const cs = get(CASESENSE_A);
    key = Array.from(s, (ch) => upper(ch.charCodeAt(0), cs));
  }

  function wlfind(wl) {                 // the nt of the key in the wordlist wl, or 0
    const cs = get(CASESENSE_A), kn = key.length;
    for (let nt = get(wl); nt; nt = get(nt)) {
      if ((getb(nt + C) & 31) !== kn) continue;
      let j = 0;
      while (j < kn && upper(getb(nt + C + 1 + j), cs) === key[j]) j++;
      if (j === kn) return nt;
    }
    return 0;
  }

  function gfind() {                    // the search order, the top first: the nt or 0
    if (key.length === 0 || key.length > 31) return 0;
    for (let i = get(NORDER_A) - 1; i >= 0; i--) {
      const nt = wlfind(get(CONTEXT_A + i * C));
      if (nt) return nt;
    }
    return 0;
  }

  function sysxt(s) {                   // the xt of the oldest word with exactly this name
    for (const wl of [WL_FORTH, WL_INTERNALS]) {
      let found = 0;
      for (let nt = get(wl); nt; nt = get(nt)) {
        if ((getb(nt + C) & 31) !== s.length) continue;
        let j = 0;
        while (j < s.length && getb(nt + C + 1 + j) === s.charCodeAt(j)) j++;
        if (j === s.length) found = nt;
      }
      if (found) return ntxt(found);
    }
    return 0;
  }

  // ==== The virtual machine =============================================
  // ip, w, sp and rp are the registers. SP and RP point to the top cell;
  // the stacks grow down. A flag is MASK (true) or 0 (false).

  let XT_THROW = 0, XT_QUIT = 0, XT_BOOT = 0;   // from the image
  let XT_CATCH = 0, XT_LIT = 0;         // for Genesis
  let OUT_A = 0;                        // the address of OUT, the column of the output

  function throw_(n) {                  // THROW n in the image: the next word is THROW
    if (!XT_THROW) gfail(throwText(n) + ' (no THROW yet)');
    push(n);
    put(THROW_ADDR, XT_THROW);
    put(THROW_ADDR + C, HALT_XT);
    ip = THROW_ADDR;
  }

  function runLater(xt) {               // the VM goes on with xt, then HALT
    put(RUN_ADDR, xt);
    put(RUN_ADDR + C, HALT_XT);
    ip = RUN_ADDR;
  }

  function runXt(xt) {                  // execute xt; return when it is complete (Genesis)
    runLater(xt);
    while (vm(SLICE) !== DONE) { /* Genesis never waits */ }
  }

  let inStr = [], inPos = 0;            // the current input line, with LF

  function doBreak() {                  // Ctrl-C between two primitives (doc/vm.md, section 9)
    brk = false;
    inPos = inStr.length;               // the rest of the input line goes
    flushLine();                        // a new line
    if (OUT_A) put(OUT_A, 0);
    if (get(HANDLER_A)) { throw_(-28); return; }   // a CATCH gets "user interrupt"
    sp = S0; rp = R0;                   // else: empty stacks, then QUIT
    put(STATE_A, 0);
    runLater(XT_QUIT);
  }

  // vm: NEXT and the primitives, for at most n steps. The result: DONE
  // (primitive 127, HALT, came), MORE (the slice is over), INPUT (?RX
  // waits for a line), WAIT (MS waits).
  function vm(n) {
    let x, y, t, a;
    if (pendingRx) {                    // ?RX of the last slice: the line is there now
      if (!rx()) return INPUT;
      pendingRx = false;
    }
    while (n-- > 0) {
      if (brk) doBreak();
      w = get(ip); ip += C;             // NEXT
      let op = get(w);
      while (op === 8) { w = pop(); op = get(w); }   // EXECUTE
      switch (op) {
        case 0: rp -= C; put(rp, ip); ip = w + C; break;                  // DOCOL
        case 1: push(w + 2 * C); break;                                   // DOVAR
        case 2: push(get(w + C)); break;                                  // DOCON
        case 3: push(w + 2 * C); rp -= C; put(rp, ip); ip = get(w + C); break;   // DODOES
        case 4: ip = get(rp); rp += C; break;                             // EXIT
        case 5: push(get(ip)); ip += C; break;                            // LIT
        case 6: ip = get(ip); break;                                      // BRANCH
        case 7: if (pop()) ip += C; else ip = get(ip); break;             // ?BRANCH
        case 9: put(sp, get(get(sp))); break;                             // @
        case 10: a = pop(); x = pop(); put(a, x); break;                  // !
        case 11: put(sp, getb(get(sp))); break;                           // C@
        case 12: a = pop(); x = pop(); setb(a, x); break;                 // C!
        case 13: push(get(sp)); break;                                    // DUP
        case 14: sp += C; break;                                          // DROP
        case 15: x = get(sp); put(sp, get(sp + C)); put(sp + C, x); break;   // SWAP
        case 16: push(get(sp + C)); break;                                // OVER
        case 17: x = pop(); rp -= C; put(rp, x); break;                   // >R
        case 18: push(get(rp)); rp += C; break;                           // R>
        case 19: push(get(rp)); break;                                    // R@
        case 20: push(sp); break;                                         // SP@: the value of SP before
        case 21: sp = get(sp); break;                                     // SP!
        case 22: push(rp); break;                                         // RP@
        case 23: rp = pop(); break;                                       // RP!
        case 24: x = pop(); put(sp, cell(get(sp) & x)); break;            // AND
        case 25: x = pop(); put(sp, cell(get(sp) | x)); break;            // OR
        case 26: x = pop(); put(sp, cell(get(sp) ^ x)); break;            // XOR
        case 27: put(sp, get(sp) & SIGN ? MASK : 0); break;               // 0<
        case 28: x = pop(); t = get(sp) + x;                              // UM+
          put(sp, cell(t)); push(t > MASK ? 1 : 0); break;
        case 29: if (!rx()) { pendingRx = true; return INPUT; }           // ?RX
          if (waitUntil) return WAIT;   // no key (host.poll): a pause
          break;
        case 30: tx(); break;                                             // TX!
        case 31: break;                                                   // !IO
        case 32: x = pop(); put(sp, cell(get(sp) + x)); break;            // +
        case 33: x = pop(); put(sp, cell(get(sp) - x)); break;            // -
        case 34: put(sp, cell(get(sp) + 1)); break;                       // 1+
        case 35: put(sp, cell(get(sp) - 1)); break;                       // 1-
        case 36: put(sp, cell(-get(sp))); break;                          // NEGATE
        case 37: put(sp, cell(~get(sp))); break;                          // INVERT
        case 38: put(sp, cell(get(sp) << 1)); break;                      // 2*
        case 39: x = get(sp); put(sp, cell(x >>> 1 | x & SIGN)); break;   // 2/
        case 40: t = pop(); x = get(sp);                                  // LSHIFT
          put(sp, t >= BITS ? 0 : cell(x << t)); break;
        case 41: t = pop(); x = get(sp);                                  // RSHIFT
          put(sp, t >= BITS ? 0 : x >>> t); break;
        case 42: x = pop(); put(sp, get(sp) === x ? MASK : 0); break;     // =
        case 43: put(sp, get(sp) ? 0 : MASK); break;                      // 0=
        case 44: x = pop(); put(sp, sgn(get(sp)) < sgn(x) ? MASK : 0); break;   // <
        case 45: x = pop(); put(sp, get(sp) < x ? MASK : 0); break;       // U<
        case 46: x = pop(); put(sp, cell(Math.imul(get(sp), x))); break;  // *
        case 47: umStar(); break;                                         // UM*
        case 48: umSlashMod(); break;                                     // UM/MOD
        case 49: x = get(sp + 2 * C); put(sp + 2 * C, get(sp + C));       // ROT
          put(sp + C, get(sp)); put(sp, x); break;
        case 50: x = pop(); put(sp, x); break;                            // NIP
        case 51: x = get(sp); if (x) push(x); break;                      // ?DUP
        case 52: x = get(sp); y = get(sp + C); push(y); push(x); break;   // 2DUP
        case 53: sp += 2 * C; break;                                      // 2DROP
        case 54: a = pop(); x = pop(); put(a, cell(get(a) + x)); break;   // +!
        case 55: move(); break;                                           // MOVE
        case 56: fill(); break;                                           // FILL
        case 57: a = get(sp); put(sp, cell(a + 1)); push(getb(a)); break; // COUNT
        case 58: findname(); break;                                       // FIND-NAME
        case 59: rp -= 3 * C;                                             // (DO)
          put(rp + 2 * C, get(ip)); put(rp + C, get(sp + C)); put(rp, get(sp));
          sp += 2 * C; ip += C; break;
        case 60: if (get(sp) === get(sp + C)) ip = get(ip);               // (?DO)
          else {
            rp -= 3 * C;
            put(rp + 2 * C, get(ip)); put(rp + C, get(sp + C)); put(rp, get(sp));
            ip += C;
          }
          sp += 2 * C; break;
        case 61: t = cell(get(rp) + 1);                                   // (LOOP)
          if (t === get(rp + C)) { rp += 3 * C; ip += C; } else { put(rp, t); ip = get(ip); }
          break;
        case 62: t = pop();                                               // (+LOOP)
          x = cell(get(rp) - get(rp + C));  // index - limit
          y = cell(x + t);                  // the same after the step
          put(rp, cell(get(rp) + t));
          if ((x ^ y) & (x ^ t) & SIGN) { rp += 3 * C; ip += C; }   // it crossed limit-1 | limit
          else ip = get(ip);
          break;
        case 63: push(get(rp)); break;                                    // I
        case 64: push(get(rp + 3 * C)); break;                            // J
        case 65: rp += 3 * C; break;                                      // UNLOOP
        case 127: return DONE;                                            // HALT: back to JavaScript
        default: if (hal(op)) return WAIT;
      }
    }
    return MORE;
  }

  function umStar() {                   // UM* ( u1 u2 -- ud ): BigInt has 64 bits and more
    const p = BigInt(get(sp + C)) * BigInt(get(sp));
    put(sp + C, Number(p & BigInt(MASK)));
    put(sp, Number(p >> BigInt(BITS)));
  }

  function umSlashMod() {               // UM/MOD ( ud u -- rem quot )
    const u = pop(), hi = get(sp), lo = get(sp + C);
    if (u === 0) { throw_(-10); return; }
    if (hi >= u) { throw_(-11); return; }   // the quotient would not fit into a cell
    const n = BigInt(hi) << BigInt(BITS) | BigInt(lo);
    put(sp + C, Number(n % BigInt(u)));
    put(sp, Number(n / BigInt(u)));
  }

  function move() {                     // MOVE ( a1 a2 u -- ), also for areas that overlap
    const u = pop(), a2 = pop(), a1 = pop();
    if (a1 < a2) for (let j = u - 1; j >= 0; j--) setb(a2 + j, getb(a1 + j));
    else for (let j = 0; j < u; j++) setb(a2 + j, getb(a1 + j));
  }

  function fill() {                     // FILL ( a u c -- )
    const c = pop() & 255, u = pop(), a = pop();
    for (let j = 0; j < u; j++) setb(a + j, c);
  }

  function findname() {                 // FIND-NAME ( a u -- nt | 0 )
    const u = pop(), a = get(sp);
    if (u === 0 || u > 31) put(sp, 0);
    else { keymem(a, u); put(sp, gfind()); }
  }

  function reboot() {                   // REBOOT ( f -- ): empty stacks, then BOOT or QUIT
    const f = get(sp);
    sp = S0; rp = R0;
    put(STATE_A, 0);
    put(HANDLER_A, 0);
    runLater(f && XT_BOOT ? XT_BOOT : XT_QUIT);
  }

  // ==== Terminal: input lines for ?RX, output for TX! ===================
  // The input comes from the files of the command line, then from the
  // keyboard or a pipe. A line of a file hides the prompt of QUIT.

  function readlines(name) {            // the lines of a file (doc/genesis.md, section 2), or null
    const text = host.readFile(name);
    if (!text) return null;
    const lines = [];
    let st = 0;
    for (let i = 0; i <= text.length; i++) {
      if (i === text.length || text[i] === 10) {   // the text after the last LF is a line too
        let e = i;
        if (e > st && text[e - 1] === 13) e--;
        lines.push(text.subarray(st, e));
        st = i + 1;
      }
    }
    return lines;
  }

  const files = [];                     // core, base (not with -image), then the files of the command line
  let fi = 0;                           // the next file to open
  let curfile = null, curln = 0, cf = [];   // the file that loads now, its line number, its lines
  let buildAt = -1;                     // after the file with this number, the system is complete
  let errseen = false;                  // a line with "error:" came
  const keyLines = [];                  // lines from the keyboard or a pipe that wait
  let inputEnd = false;                 // the keyboard or the pipe has no more lines
  let prompt = [];                      // what showed before the keyboard: the prompt
  let promptShown = false;

  function setline(s) {                 // the next line for ?RX
    inStr = Array.from(s);
    if (inStr.length && inStr[inStr.length - 1] === 13) inStr.pop();
    inStr.push(10);
    inPos = 0;
  }

  function nextLine() {                 // the next line for ?RX; false: it must wait for the keyboard
    for (;;) {
      if (curfile && errseen) {         // an error stops all files
        msg(curfile + ':' + curln + ': loading stopped');
        curfile = null;
        if (buildAt >= 0 && fi <= buildAt + 1) { fi = buildAt + 1; buildEnd(); }
        fi = files.length;
      }
      errseen = false;
      if (curfile) {
        if (curln < cf.length) {
          curln++;
          ob = [];                      // the prompt of QUIT does not show
          setline(cf[curln - 1]);
          return true;
        }
        curfile = null;
        if (fi === buildAt + 1) buildEnd();
        continue;
      }
      if (fi < files.length) {
        curfile = files[fi++];
        curln = 0;
        cf = readlines(curfile);
        if (!cf) { msg('cannot read ' + curfile); curfile = null; fi = files.length; }
        continue;
      }
      if (!keyLines.length) {           // the keyboard or a pipe
        if (inputEnd) {
          if (host.tty) host.write(bytesOf('\n'));
          bye(0);
        }
        if (host.tty && !promptShown) { // the terminal shows the prompt, then the typed line
          prompt = ob;
          host.write(Uint8Array.from(ob));
          ob = [];
          promptShown = true;
        }
        return false;
      }
      const line = keyLines.shift();
      if (!host.tty) {                  // a pipe: the line shows after the prompt
        host.write(Uint8Array.from(ob.concat(Array.from(line), [10])));
        ob = [];
      }
      promptShown = false;
      setline(line);
      return true;
    }
  }

  let XT_REFILL = 0, REFILL_END = 0;    // the body of REFILL: QUIT waits for a line there

  function refilling() {                // true if a return address points into REFILL
    for (let r = rp; r < rp + 8 * C && r < R0; r += C) {
      const x = get(r);
      if (x > XT_REFILL && x < REFILL_END) return true;
    }
    return false;
  }

  function rx() {                       // ?RX ( -- c true | false ); false: it must wait
    if (inPos >= inStr.length) {
      const quit = refilling();
      if (!quit && (curfile || fi < files.length || !host.tty)) {
        push(0);                        // a word reads keys while files load: no key (doc/vm.md 9)
        return true;
      }
      if (!quit && host.poll && !keyLines.length) {
        showOutput();                   // a word asks for a key, no line waits: no key, and
        waitUntil = host.now() + 1;     // a pause, so that the page can take a typed line
        push(0);
        return true;
      }
      if (!nextLine()) return false;    // QUIT waits in REFILL, or a word waits at the keyboard
    }
    push(inStr[inPos++]);
    push(MASK);
    return true;
  }

  function bodyend(xt) {                // the next header in memory after xt
    let best = MEM_END;
    for (let wl = get(VOCLINK_A); wl; wl = get(wl + C))
      for (let nt = get(wl); nt; nt = get(nt)) if (nt > xt && nt < best) best = nt;
    return best;
  }

  function findWords() {                // the words of the image that this program needs
    XT_REFILL = sysxt('REFILL');
    REFILL_END = bodyend(XT_REFILL);
    const xt = sysxt('OUT');
    OUT_A = xt ? get(xt + C) : 0;
  }

  function tx() {                       // TX! ( c -- )
    const c = pop() & 255;
    if (c === 10) {
      if (textOf(ob).includes('error:')) errseen = true;
      flushLine();
    } else if (c) ob.push(c);
  }

  // ==== Files: HAL 89-93, 98, 99 (doc/vm.md, section 8) =================
  // A file id (fid) is a number from 10 to 29. OPEN-FILE reads the whole
  // file into memory; READ-LINE and READ-FILE take the bytes from there.
  // WRITE-FILE writes into the file at once: CREATE-FILE makes it new,
  // OPEN-FILE with W/O or R/W writes at its end.

  const fds = new Array(20).fill(null); // each: { out, buf, pos }

  function memstr(a, n) {               // n bytes at a
    const b = new Uint8Array(n);
    for (let j = 0; j < n; j++) b[j] = getb(a + j);
    return b;
  }

  function ret2(x, y) { put(sp + C, cell(x)); put(sp, cell(y)); }
  function ret3(x, y, z) { put(sp + 2 * C, cell(x)); put(sp + C, cell(y)); put(sp, cell(z)); }
  const fid = (x) => (x >= 10 && x < 30 && fds[x - 10] ? x - 10 : -1);

  function fopen(create) {              // OPEN-FILE, CREATE-FILE ( a u fam -- fid ior )
    const fam = pop() & 3;
    const name = textOf(memstr(get(sp + C), get(sp)));
    const k = fds.indexOf(null);
    if (k < 0) { ret2(0, -37); return; }
    let out = null, buf = new Uint8Array(0);
    if (create) {
      out = host.openWrite(name, true); // a new file, not the old one shorter: macOS keeps
      if (!out) { ret2(0, -37); return; }   // the signature of a program for each file
    } else {
      if (fam !== 1) {
        buf = host.readFile(name);
        if (!buf) { ret2(0, -38); return; }
      }
      if (fam !== 0) {
        out = host.openWrite(name, false);
        if (!out) { ret2(0, fam === 1 ? -38 : -37); return; }
      }
    }
    fds[k] = { out: out, buf: buf, pos: 0 };
    ret2(k + 10, 0);
  }

  function fclose() {                   // CLOSE-FILE ( fid -- ior )
    const k = fid(get(sp));
    if (k < 0) { put(sp, cell(-37)); return; }
    if (fds[k].out) host.closeFile(fds[k].out);
    fds[k] = null;
    put(sp, 0);
  }

  function freadline() {                // READ-LINE ( a u fid -- u2 f ior )
    const k = fid(get(sp)), u = get(sp + C), a = get(sp + 2 * C);
    if (k < 0) { ret3(0, 0, -37); return; }
    const f = fds[k];
    if (f.pos >= f.buf.length) { ret3(0, 0, 0); return; }   // the end of the file
    let e = f.buf.indexOf(10, f.pos);
    if (e < 0) e = f.buf.length;
    let n = e - f.pos;
    if (n && f.buf[e - 1] === 13) n--;  // CR LF ends a line too
    if (n > u) { n = u; e = f.pos + u - 1; }   // the buffer is full: the rest of the line comes next
    for (let j = 0; j < n; j++) setb(a + j, f.buf[f.pos + j]);
    f.pos = e + 1;
    ret3(n, MASK, 0);
  }

  function fread() {                    // READ-FILE ( a u fid -- u2 ior )
    const k = fid(pop()), u = get(sp), a = get(sp + C);
    if (k < 0) { ret2(0, -37); return; }
    const f = fds[k];
    const n = Math.max(0, Math.min(u, f.buf.length - f.pos));
    for (let j = 0; j < n; j++) setb(a + j, f.buf[f.pos + j]);
    f.pos += n;
    ret2(n, 0);
  }

  function fwrite() {                   // WRITE-FILE ( a u fid -- ior )
    const k = fid(pop()), u = pop(), a = get(sp);
    if (k < 0 || !fds[k].out) { put(sp, cell(-37)); return; }
    put(sp, host.writeTo(fds[k].out, memstr(a, u)) ? 0 : cell(-37));
  }

  // ==== Genesis: JavaScript compiles core.fs (doc/genesis.md) ===========
  // The outer interpreter: find the word, then execute or compile it; else
  // convert a number. PARSE-NAME, PARSE, HEADER, REVEAL, IMMEDIATE, ",",
  // "C,", "[" and "]" are host primitives until core.fs defines Forth
  // versions. Genesis handles "?:", "\" and "(" itself until core.fs has them.

  let gln = 0;                          // the line of core.fs
  const gnt = new Set();                // the headers that Genesis removes at the end
  let bootColon = 0, bootSemi = 0;      // the first ":" and ";" (line 1 and 2 of core.fs)
  let gskip = false, gskipf = false;    // "?:" skips a definition, then its flags
  let pa = 0, pn = 0;                   // the word that PARSE-NAME or PARSE found

  function gfail(s) {
    const core = files.length ? files[0] : coreFile;
    msg('error: ' + s);
    msg(core + ':' + gln + ': loading stopped');
    msg(core + ' is not complete; the system cannot start');
    bye(1);
  }

  const gerr = (n, word) => gfail(word ? throwText(n) + ': ' + word : throwText(n));

  function pname() {                    // PARSE-NAME on TIB: pa pn; >IN moves
    let i = get(TOIN_A);
    const n = get(NTIB_A);
    while (i < n && getb(TIB + i) <= 32) i++;
    const st = i;
    while (i < n && getb(TIB + i) > 32) i++;
    pa = TIB + st; pn = i - st;
    if (i < n) i++;
    put(TOIN_A, i);
  }

  function pparse(c) {                  // PARSE up to the character c: pa pn
    let i = get(TOIN_A);
    const n = get(NTIB_A), st = i;
    while (i < n && getb(TIB + i) !== c) i++;
    pa = TIB + st; pn = i - st;
    if (i < n) i++;
    put(TOIN_A, i);
  }

  function comma(x) {                   // , ( x -- )
    const t = get(DP_A);
    put(t - t % C, x);
    put(DP_A, t + C);
  }

  function mkhead(name, wl, flags) {    // a visible header; the result is the xt
    const nt = get(DP_A);
    put(nt, get(wl));
    setb(nt + C, name.length + flags);
    for (let j = 0; j < name.length; j++) setb(nt + C + 1 + j, name.charCodeAt(j));
    put(wl, nt);
    put(LATEST_A, nt);
    put(DP_A, ntxt(nt));
    return get(DP_A);
  }

  function gheader() {                  // HEADER ( a u -- ): a new header in CURRENT, not visible yet
    const n = pop(), a = pop();
    if (n === 0) gerr(-16);
    if (n > 31) gerr(-19);
    const name = textOf(memstr(a, n));
    keymem(a, n);
    const r = wlfind(get(CURRENT_A));
    if (r && !gnt.has(r)) msg('redefined ' + name);
    while (get(DP_A) % C) { setb(get(DP_A), 0); put(DP_A, get(DP_A) + 1); }
    const nt = get(DP_A);
    const xt = Math.floor((nt + C + 1 + n + C - 1) / C) * C;
    if (xt + 4 * C > DICT_END) gerr(-8);
    put(nt, get(get(CURRENT_A)));       // the link: the newest word of CURRENT
    setb(nt + C, n);
    for (let j = 0; j < n; j++) setb(nt + C + 1 + j, getb(a + j));
    for (let j = nt + C + 1 + n; j < xt; j++) setb(j, 0);
    put(DP_A, xt);
    put(LATEST_A, nt);
    if (name === ':' && !bootColon) { bootColon = nt; gnt.add(nt); }
    if (name === ';' && !bootSemi) { bootSemi = nt; gnt.add(nt); }
  }

  function gnumber(s) {                 // the value of the word s, or null if it is no number
    if (s.length === 3 && s[0] === "'" && s[2] === "'") return s.charCodeAt(1);
    let base = get(BASE_A), neg = false, p = s;
    if (p.length > 1 && p[0] === '-' && '$#%'.includes(p[1])) { neg = true; p = p.slice(1); }
    if (p[0] === '$') { base = 16; p = p.slice(1); }
    else if (p[0] === '#') { base = 10; p = p.slice(1); }
    else if (p[0] === '%') { base = 2; p = p.slice(1); }
    if (p[0] === '-') { neg = !neg; p = p.slice(1); }
    if (!p || base < 2 || base > 36) return null;
    let v = 0;
    for (const ch of p) {
      const c = ch.charCodeAt(0);
      let d;
      if (c >= 48 && c <= 57) d = c - 48;          // 0-9
      else if (c >= 65 && c <= 90) d = c - 55;     // A-Z
      else if (c >= 97 && c <= 122) d = c - 87;    // a-z
      else return null;
      if (d >= base) return null;
      if (v <= MASK) v = v * base + d;             // above MASK the number is too big anyway
    }
    if (v > MASK || (neg && v > SIGN)) gerr(-24, s);
    return neg ? cell(-v) : v;
  }

  function gexec(xt) {                  // execute xt; through CATCH once core.fs has it
    if (XT_CATCH) {
      push(xt);
      runXt(XT_CATCH);
      const code = pop();
      if (code) gerr(sgn(code));
    } else runXt(xt);
  }

  function gline(line) {                // interpret one line of core.fs
    if (line.length > 256) gfail('line too long (' + line.length + ' bytes, at most 256)');
    for (let j = 0; j < line.length; j++) setb(TIB + j, line[j]);
    put(NTIB_A, line.length);
    put(TOIN_A, 0);
    for (;;) {
      pname();
      if (!pn) return;
      const word = textOf(memstr(pa, pn));
      if (gskip) {                      // "?:" skips a definition
        if (word === '\\') put(TOIN_A, get(NTIB_A));
        else if (word === '(') pparse(41);
        else if (word === ';') { gskip = false; gskipf = true; }
        continue;
      }
      if (gskipf) {                     // and the flags after it
        if (['IMMEDIATE', 'UNCHECKED', 'COMPILE-ONLY'].includes(word.toUpperCase())) continue;
        gskipf = false;
      }
      if (word === '?:') {
        const save = get(TOIN_A);
        pname();
        if (!pn) gerr(-16);
        keymem(pa, pn);
        if (gfind()) { gskip = true; continue; }
        put(TOIN_A, save);
        keystr(':');
        gexec(ntxt(gfind()));
        continue;
      }
      keymem(pa, pn);
      const nt = gfind();
      if (nt) {
        const xt = ntxt(nt), fl = getb(nt + C);
        if (get(STATE_A) === 0 || fl & 128) {
          if (get(STATE_A) === 0 && fl & 64) gerr(-14, word);
          gexec(xt);
        } else comma(xt);
        continue;
      }
      if (word === '\\') { put(TOIN_A, get(NTIB_A)); continue; }   // comments until core.fs defines them
      if (word === '(') { pparse(41); continue; }
      const num = gnumber(word);
      if (num === null) gerr(-13, word);
      if (get(STATE_A)) { comma(XT_LIT); comma(num); } else push(num);
    }
  }

  function genesis() {                  // compile core.fs; then remove the host primitives
    put(HALT_XT, 127);
    put(HALT_ADDR, HALT_XT);
    put(DP_A, DICT_START + GUARD);
    put(VP_A, VP_START);
    put(BASE_A, 10);
    put(WL_FORTH + C, 0);
    put(WL_INTERNALS + C, WL_FORTH);
    put(VOCLINK_A, WL_INTERNALS);
    sp = S0; rp = R0;
    for (const [op, name, voc, flags] of PRIM) {   // a header for each primitive
      if (!withHeader(op)) continue;
      const xt = mkhead(name, voc === 'F' ? WL_FORTH : WL_INTERNALS, flags);
      put(xt, op);
      put(DP_A, xt + C);
      if (op === 5) XT_LIT = xt;
      if (op >= 100) gnt.add(get(LATEST_A));
    }
    const consts = [];                  // constants for the system variables, then 9 more
    let a = SYS;
    for (const [name, cells, voc] of SV) { consts.push([name, a, voc]); a += cells * C; }
    consts.push(['TIB', TIB, 'I'], ['PAD', PAD, 'I'], ['S0', S0, 'I'], ['R0', R0, 'I'],
      ['DATA-END', DATA_END, 'I'], ['WL-FORTH', WL_FORTH, 'I'],
      ['WL-INTERNALS', WL_INTERNALS, 'I'], ['CELL', C, 'I'], ['DICT-END', DICT_END, 'I']);
    for (const [name, value, voc] of consts) {
      const xt = mkhead(name, voc === 'F' ? WL_FORTH : WL_INTERNALS, 0);
      put(xt, 2);
      put(xt + C, value);
      put(DP_A, xt + 2 * C);
    }
    put(NORDER_A, 2);
    put(CONTEXT_A, WL_FORTH);
    put(CONTEXT_A + C, WL_INTERNALS);
    put(CURRENT_A, WL_FORTH);
    const lines = readlines(files[0]);
    if (!lines) { msg('cannot read ' + files[0]); bye(1); }
    for (gln = 1; gln <= lines.length; gln++) {
      gline(lines[gln - 1]);
      if (!XT_CATCH) {
        keystr('CATCH');
        const nt = gfind();
        if (nt) XT_CATCH = ntxt(nt);
      }
    }
    gln = lines.length;
    for (let wl = get(VOCLINK_A); wl; wl = get(wl + C)) {   // remove the host primitives and the first ":" ";"
      let prev = 0;
      for (let nt = get(wl), nx; nt; nt = nx) {
        nx = get(nt);
        if (gnt.has(nt)) put(prev || wl, nx);
        else prev = nt;
      }
    }
  }

  // ==== Images: Intel HEX ================================================
  // An image is a text file (doc/vm.md, section 10). The first line names
  // the cell size and the start words; then come Intel HEX records for the
  // system variables, the RAM data up to VP and the dictionary up to DP.

  const hex = (x, n) => x.toString(16).toUpperCase().padStart(n, '0');

  function hexseg(a, e, lines, state) { // the records for the bytes from a up to e (exclusive)
    while (a < e) {
      const hi = Math.floor(a / 65536);
      if (hi !== state.ulba) {          // extended linear address: the upper 16 bits
        state.ulba = hi;
        lines.push(':02000004' + hex(hi, 4) + hex((256 - (6 + (hi >> 8) + (hi & 255)) % 256) % 256, 2));
      }
      const n = Math.min(e - a, 16, 65536 - (a & 0xFFFF));
      let data = '', s = n + (a >> 8 & 255) + (a & 255);
      for (let j = 0; j < n; j++) { const b = getb(a + j); s += b; data += hex(b, 2); }
      lines.push(':' + hex(n, 2) + hex(a & 0xFFFF, 4) + '00' + data + hex((256 - s % 256) % 256, 2));
      a += n;
    }
  }

  function writeImage(name, boot) {     // true if it works
    const lines = ['; ucf image bits=' + BITS + ' boot=' + boot + ' quit=' + XT_QUIT +
                   ' throw=' + XT_THROW + ' prims=' + prims];
    const state = { ulba: 0 };          // a record for the upper bits only above $FFFF
    hexseg(SYS, DATA_START, lines, state);
    hexseg(DATA_START, get(VP_A), lines, state);
    hexseg(DICT_START, get(DP_A), lines, state);
    lines.push(':00000001FF');
    const h = host.openWrite(name, true);
    if (!h) return false;
    const ok = host.writeTo(h, bytesOf(lines.join('\n') + '\n'));
    host.closeFile(h);
    return ok;
  }

  let meta = '';                        // the first line of an image

  function hexread(name, compare) {     // the records into memory, or compare them with it
    const lines = readlines(name);
    if (!lines) { host.error('ucf.js: cannot read ' + name + '\n'); throw new Bye(1); }
    let hi = 0, diffs = 0, first = '';
    for (let k = 0; k < lines.length; k++) {
      const s = textOf(lines[k]);
      if (s[0] === ';') { if (s.includes(' boot=')) meta = s; continue; }
      if (s[0] !== ':') continue;
      const rec = [];
      for (let j = 1; j + 1 < s.length; j += 2) rec.push(parseInt(s.substr(j, 2), 16));
      const n = rec[0], a = rec[1] << 8 | rec[2], t = rec[3];
      if (rec.reduce((x, y) => x + y, 0) % 256) {
        host.error('ucf.js: ' + name + ':' + (k + 1) + ': wrong checksum\n');
        throw new Bye(1);
      }
      if (t === 0) {
        for (let j = 0; j < n; j++) {
          const b = rec[4 + j];
          if (!compare) setb(hi + a + j, b);
          else {
            const r = getb(hi + a + j);
            if (r !== b) {
              if (!diffs) first = '$' + (hi + a + j).toString(16).toUpperCase() + ' (here ' + r + ', there ' + b + ')';
              diffs++;
            }
          }
        }
      } else if (t === 1) break;
      else if (t === 4) hi = (rec[4] << 8 | rec[5]) * 65536;
    }
    return [diffs, first];
  }

  function metaval(field) {             // the number after " field=" in the first line
    for (const part of meta.split(' ')) if (part.startsWith(field + '=')) return parseInt(part.slice(field.length + 1), 10);
    return 0;
  }

  function loadImage(name) {            // memory, start words, primitives
    meta = '';
    hexread(name, false);
    if (!meta) { host.error('ucf.js: ' + name + ' is not an image of uCForth\n'); throw new Bye(1); }
    XT_BOOT = metaval('boot'); XT_QUIT = metaval('quit'); XT_THROW = metaval('throw');
    for (const part of meta.split(' ')) {
      if (!part.startsWith('prims=')) continue;
      for (const r of part.slice(6).split(',')) {   // each range lo-hi: this VM must have it
        const [lo, hi] = r.split('-').map(Number);
        for (let op = lo; op <= (hi === undefined ? lo : hi); op++) {
          if (!have(op)) { host.error('ucf.js: ' + name + ' needs primitive ' + op + '\n'); throw new Bye(1); }
        }
      }
    }
  }

  let checkfail = 0;

  function checkImage(what, name) {     // compare the memory with a reference
    if (!host.readFile(name)) { msg('check ' + what + ': cannot read ' + name); checkfail = 1; return; }
    const [diffs, first] = hexread(name, true);
    if (diffs) { msg('check ' + what + ': ' + diffs + ' bytes differ from ' + name + ', the first at ' + first); checkfail = 1; }
    else msg('check ' + what + ': the same as ' + name);
  }

  function saveImage() {                // SAVE-IMAGE ( -- ior )
    const f = host.env('UCF_IMAGE') || 'ucf.hex';
    if (writeImage(f, XT_BOOT || XT_QUIT)) { msg('saved: ' + f); push(0); }
    else push(-37);
  }

  // ==== The HAL (doc/vm.md, section 8) and the host primitives ==========

  let T0 = 0;                           // the clock at the start

  function hal(op) {                    // true: the VM must wait (MS, US)
    let a, t;
    switch (op) {
      case 80: push(host.now() - T0); break;                            // MILLIS
      case 81: waitUntil = host.now() + pop(); return true;             // MS
      case 82: waitUntil = host.now() + pop() / 1000; return true;      // US
      case 83: a = pop(); t = pop() & 255; setb(a, getb(a) | t); break; // BIS!
      case 84: a = pop(); t = pop() & 255; setb(a, getb(a) & ~t); break;   // BIC!
      case 85: saveImage(); break;                                      // SAVE-IMAGE
      case 86: break;                                                   // ERASE-IMAGE
      case 87: reboot(); break;                                         // REBOOT
      case 88: bye(0); break;                                           // BYE
      case 89: fopen(false); break;                                     // OPEN-FILE
      case 90: fopen(true); break;                                      // CREATE-FILE
      case 91: fclose(); break;                                         // CLOSE-FILE
      case 92: fread(); break;                                          // READ-FILE
      case 93: fwrite(); break;                                         // WRITE-FILE
      case 98: freadline(); break;                                      // READ-LINE
      case 99: bye(pop() & 255); break;                                 // (BYE)
      case 100: pname(); push(pa); push(pn); break;                     // PARSE-NAME
      case 101: pparse(pop() & 255); push(pa); push(pn); break;         // PARSE
      case 102: gheader(); break;                                       // HEADER
      case 103: t = get(LATEST_A); if (t) put(get(CURRENT_A), t); break;   // REVEAL
      case 104: a = get(LATEST_A) + C; setb(a, getb(a) | 128); break;   // IMMEDIATE
      case 105: comma(pop()); break;                                    // ,
      case 106: t = get(DP_A); setb(t, pop()); put(DP_A, t + 1); break; // C,
      case 107: put(STATE_A, 0); break;                                 // [
      case 108: put(STATE_A, MASK); break;                              // ]
      default: throw_(-21);                                             // an unknown primitive
    }
    return false;
  }

  // ==== Start ============================================================

  function wordsInForth() {
    let n = 0;
    for (let nt = get(WL_FORTH); nt; nt = get(nt)) n++;
    return n;
  }

  function coreEnd() {                  // Genesis is complete: the seed, and its check
    if (dumpCoreFile && !writeImage(dumpCoreFile, XT_QUIT)) msg('cannot write ' + dumpCoreFile);
    if (check) checkImage('core' + BITS, pfx + 'seed/core' + BITS + '.hex');
  }

  function buildEnd() {                 // base.fs is complete (doc/genesis.md, section 6)
    const here = get(DP_A);
    put(BASEEND_A, here);
    put(FROZENEND_A, here);
    put(NORDER_A, 1);
    put(CONTEXT_A, WL_FORTH);
    put(CURRENT_A, WL_FORTH);
    put(STATE_A, 0);
    buildAt = -2;
    XT_BOOT = sysxt('BOOT');
    if (dumpFile && !writeImage(dumpFile, XT_BOOT)) msg('cannot write ' + dumpFile);
    if (check) {
      ob = [];                          // the prompt of QUIT is not wanted here
      checkImage('base' + BITS, pfx + 'seed/base' + BITS + '.hex');
      bye(checkfail);
    }
    host.write(bytesOf('uCForth - JavaScript bootstrap VM - ' + BITS + ' bit' + (pure ? ' - pure' : '') +
      ' - ' + wordsInForth() + ' words in FORTH - built in ' + (host.now() - T0) + ' ms\n'));
  }

  // run: slices of the VM until it waits or ends. The host calls it again.
  function run() {
    if (running || stopped) return;
    running = true;
    try {
      for (;;) {
        if (waitUntil) {                // MS: wait until the time has come
          const rest = waitUntil - host.now();
          if (rest > 0 && !brk) { showOutput(); running = false; host.later(run, Math.min(rest, 100)); return; }
          waitUntil = 0;
        }
        const r = vm(SLICE);
        if (r === DONE) bye(0);         // QUIT or BOOT came back: the end
        if (r === INPUT) { running = false; return; }   // input() calls run again
        if (r === MORE) { running = false; host.later(run, 0); return; }   // let the host breathe
      }
    } catch (e) {
      running = false;
      stopped = true;
      if (e instanceof Bye) host.exit(e.code);
      else throw e;
    }
  }

  function start(argv) {
    try {
      options(argv);
      memoryMap();
      primitives();
      T0 = host.now();
      if (imageFile) {
        loadImage(imageFile);
        files.push(...args);
        findWords();
        host.write(bytesOf('uCForth - JavaScript bootstrap VM - ' + BITS + ' bit - ' + wordsInForth() +
          ' words in FORTH - image ' + imageFile + '\n'));
        sp = S0; rp = R0;
        put(STATE_A, 0);
        put(HANDLER_A, 0);
        runLater(XT_BOOT || XT_QUIT);
      } else {
        files.push(coreFile, baseFile, ...args);
        genesis();
        XT_THROW = sysxt('THROW');
        XT_QUIT = sysxt('QUIT');
        if (!XT_QUIT) gfail(files[0] + ' does not define QUIT');
        findWords();
        coreEnd();
        fi = 1;
        buildAt = 1;
        runLater(XT_QUIT);
      }
    } catch (e) {
      stopped = true;
      if (e instanceof Bye) { host.exit(e.code); return; }
      throw e;
    }
    run();
  }

  function input(line) {                // a line from the keyboard or a pipe, without its LF
    keyLines.push(line);
    run();
  }

  function end() {                      // the keyboard or the pipe has no more lines
    inputEnd = true;
    run();
  }

  function interrupt() {                // Ctrl-C
    if (pendingRx && host.tty && !keyLines.length) {   // at the prompt: a new line, the prompt again
      host.write(Uint8Array.from([10].concat(prompt)));
      return;
    }
    brk = true;
    run();
  }

  return { start: start, input: input, end: end, interrupt: interrupt };
}

// ==== Node.js ================================================================
// The host for Node.js: files, the console, Ctrl-C.

if (typeof process !== 'undefined' && process.versions && process.versions.node && require.main === module) {
  const fs = require('fs');
  let vm = null;
  const host = {
    tty: Boolean(process.stdin.isTTY),
    write: (b) => process.stdout.write(Buffer.from(b)),
    error: (s) => process.stderr.write(s),
    readFile: (name) => { try { return new Uint8Array(fs.readFileSync(name)); } catch (e) { return null; } },
    openWrite: (name, fresh) => {
      try {
        if (fresh) { try { fs.unlinkSync(name); } catch (e) { /* no old file */ } }
        return fs.openSync(name, fresh ? 'w' : 'a');
      } catch (e) { return null; }
    },
    writeTo: (h, b) => { try { fs.writeSync(h, Buffer.from(b)); return true; } catch (e) { return false; } },
    closeFile: (h) => { try { fs.closeSync(h); } catch (e) { /* closed */ } },
    env: (name) => process.env[name] || '',
    now: () => Math.floor(Number(process.hrtime.bigint() / 1000000n)),
    later: (fn, ms) => (ms ? setTimeout(fn, ms) : setImmediate(fn)),
    exit: (code) => process.stdout.write('', () => process.exit(code))   // after the output
  };
  vm = makeVM(host);
  let rest = Buffer.alloc(0);           // the start of a line that has no LF yet
  process.stdin.on('data', (data) => {
    rest = Buffer.concat([rest, data]);
    let i;
    while ((i = rest.indexOf(10)) >= 0) {
      vm.input(new Uint8Array(rest.subarray(0, i)));
      rest = rest.subarray(i + 1);
    }
  });
  process.stdin.on('end', () => {
    if (rest.length) vm.input(new Uint8Array(rest));
    vm.end();
  });
  process.on('SIGINT', () => vm.interrupt());
  vm.start(process.argv.slice(2));
} else if (typeof window !== 'undefined') {
  window.makeVM = makeVM;               // for ucf.html
}
