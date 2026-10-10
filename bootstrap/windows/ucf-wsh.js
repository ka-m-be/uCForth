// ucf-wsh.js - the JScript bootstrap VM of uCForth, for Windows
//
// It is the virtual machine of uCForth in JScript, the script language
// of the Windows Script Host: each Windows has it, from Windows 98 to
// Windows 11. Its parts: the memory, the registers, the primitives and a
// small HAL (doc/vm.md). Its Genesis, a small outer interpreter, compiles
// forth/core.fs once (doc/genesis.md). From then on QUIT in the image
// reads each line, as on a board: first forth/base.fs, then the files of
// the command line, then the keyboard.
//
// The parts and their names are the same as in the C bootstrap VM
// (bootstrap/c/ucf.c, the reference); the README in this folder tells
// what is special in JScript.
//
// Use, in a command window, in the folder ucf:
//
//   cscript //nologo bootstrap\windows\ucf-wsh.js [OPTION ...] [FILE ...]
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
// in the environment variable UCF_IMAGE.
//
// The program is JScript 5.8 (ECMAScript 3): only "var", no typed
// arrays, no functions of newer JavaScript. So it runs with the cscript
// of each Windows. Only the part "The system" uses Windows.
//
// Limits:
// - ?RX gets whole lines: the console sends a line with the return key.
// - cscript cannot tell a keyboard from a pipe: a line from a pipe does
//   not show after the prompt.
// - Ctrl-C ends cscript: it does not stop only the word that runs.
// - No checks of the stacks. An address wraps around in the memory of
//   the VM (64 KB or 256 KB).
// - A cell is at an address that is a multiple of the cell size.
// - REBOOT starts BOOT or QUIT again, but does not load the image again.
//   ERASE-IMAGE does nothing.
// - OPEN-FILE with W/O or R/W writes at the end of the file. A written
//   file is complete on the disk after CLOSE-FILE, or at the end of the
//   program.
// - TX! does not send the byte 0.

// ==== The system ===============================================================
// Only these functions use Windows: the clock, a wait, the console, the
// files, the environment and the end of the program.
//
// cscript gives the console as WScript.StdIn, StdOut and StdErr. wscript,
// the host for a double click, has no console: the program then tells how
// to start it.
//
// JScript has strings, but no bytes. Here the bytes of a file are a
// string: each character is one byte, 0 to 255. To get the bytes of a
// file, the program needs two objects of Windows: ADODB.Stream reads and
// writes a file as bytes, and an element of Microsoft.XMLDOM with the
// data type "bin.hex" makes hex digits of bytes, and bytes of hex digits.

var stdin = null, stdout = null, stderr = null;
var fso = null;                         // Scripting.FileSystemObject: is a file there?
var HEX2 = [], CHR = {};                // a byte as two hex digits; two hex digits as a character

function sys_start() {                  // the console
  try {
    stdin = WScript.StdIn; stdout = WScript.StdOut; stderr = WScript.StdErr;
    stdout.Write('');
  } catch (e) {
    WScript.Echo('uCForth needs a command window. Start it there, in the folder ucf:\n\n' +
                 'cscript //nologo bootstrap\\windows\\ucf-wsh.js');
    WScript.Quit(2);
  }
  fso = new ActiveXObject('Scripting.FileSystemObject');
}

function tables() {                     // HEX2 and CHR, for the files and for the images
  var i, h, d = '0123456789ABCDEF';
  for (i = 0; i < 256; i++) {
    h = d.charAt(i >> 4) + d.charAt(i & 15);
    HEX2[i] = h;
    CHR[h] = String.fromCharCode(i);
    CHR[h.toLowerCase()] = String.fromCharCode(i);
  }
}

function now_ms() { return new Date().getTime(); }   // the clock in milliseconds

function wait_ms(ms) { if (ms > 0) WScript.Sleep(ms); }

function ascii(s) {                     // s with "?" for each character that is not ASCII
  var t = '', i, c;
  for (i = 0; i < s.length; i++) { c = s.charCodeAt(i); t += c < 128 ? s.charAt(i) : '?'; }
  return t;
}

function out_line(s) {                  // a line to the console
  try { stdout.WriteLine(s); } catch (e) { stdout.WriteLine(ascii(s)); }
}

function out_text(s) {                  // text without a line end: the prompt before the keyboard
  try { stdout.Write(s); } catch (e) { stdout.Write(ascii(s)); }
}

function err_line(s) { stderr.WriteLine(s); }   // a message of the program itself

function key_line() {                   // a line from the keyboard without its end; null: no more lines
  if (stdin.AtEndOfStream) return null;
  return stdin.ReadLine();
}

function file_there(name) { return fso.FileExists(name); }

function file_read(name) {              // the bytes of a file, or null
  var st, b, el, h, n, t = [], i;
  try {
    st = new ActiveXObject('ADODB.Stream');
    st.Type = 1;                        // 1: bytes, not text
    st.Open();
    st.LoadFromFile(name);
    n = st.Size;
    b = n ? st.Read() : null;
    st.Close();
  } catch (e) { return null; }
  if (!n) return '';
  el = new ActiveXObject('Microsoft.XMLDOM').createElement('b');
  el.dataType = 'bin.hex';
  el.nodeTypedValue = b;
  h = el.text.replace(/[^0-9A-Fa-f]/g, '');
  for (i = 0; i < h.length; i += 2) t.push(CHR[h.substr(i, 2)]);
  return t.join('');
}

function file_write(name, s) {          // a new file with the bytes s; false if it fails
  var st, el, t = [], i;
  try {
    st = new ActiveXObject('ADODB.Stream');
    st.Type = 1;
    st.Open();
    if (s.length) {
      for (i = 0; i < s.length; i++) t.push(HEX2[s.charCodeAt(i) & 255]);
      el = new ActiveXObject('Microsoft.XMLDOM').createElement('b');
      el.dataType = 'bin.hex';
      el.text = t.join('');
      st.Write(el.nodeTypedValue);
    }
    st.SaveToFile(name, 2);             // 2: an old file with this name goes
    st.Close();
  } catch (e) { return false; }
  return true;
}

function env_var(name) {                // an environment variable, or ''
  try { return String(new ActiveXObject('WScript.Shell').Environment('Process').Item(name)); }
  catch (e) { return ''; }
}

function leave(code) { WScript.Quit(code); }   // the end of the program

// ==== Options ==================================================================
// The default files lie in the project. The program looks for them in
// the current folder (ucf), then two folders above (ucf\bootstrap\windows).

var BITS = 16;                          // the cell size
var pure = false, check = false;        // -pure, -check
var coreFile, baseFile, imageFile = null, dumpFile = null, dumpCoreFile = null;
var pfx = '';                           // '' or '../../': where the project is
var args = [];                          // the files of the command line

function usage() {
  err_line('use: cscript //nologo ucf-wsh.js [-32] [-pure] [-core FILE] [-base FILE] [-image FILE]');
  err_line('                                 [-dump-core FILE] [-dump FILE] [-check] [FILE ...]');
  leave(2);
}

function options() {
  var argv = [], i, a, n, text;
  for (i = 0; i < WScript.Arguments.length; i++) argv.push(String(WScript.Arguments.Item(i)));
  if (!file_there('forth/core.fs') && file_there('../../forth/core.fs')) pfx = '../../';
  coreFile = pfx + 'forth/core.fs';
  baseFile = pfx + 'forth/base.fs';
  for (i = 0; i < argv.length; i++) {
    a = argv[i];
    if (a === '--') continue;
    if (a === '-32') BITS = 32;
    else if (a === '-pure') pure = true;
    else if (a === '-check') { check = true; pure = true; }
    else if (a === '-core' || a === '-base' || a === '-image' || a === '-dump-core' || a === '-dump') {
      if (i + 1 >= argv.length) usage();
      n = argv[++i];
      if (a === '-core') coreFile = n;
      else if (a === '-base') baseFile = n;
      else if (a === '-image') imageFile = n;
      else if (a === '-dump-core') dumpCoreFile = n;
      else dumpFile = n;
    } else if (a.charAt(0) === '-') usage();
    else args.push(a);
  }
  if (imageFile !== null) {             // the first line of an image names the cell size
    text = file_read(imageFile);
    if (text === null) { err_line('ucf-wsh.js: cannot read ' + imageFile); leave(1); }
    BITS = text.substring(0, 200).indexOf('bits=32') >= 0 ? 32 : 16;
  }
}

// ==== Memory map (doc/vm.md, section 3) ========================================
// The memory of the VM is an array of cells: M[a >> S] is the cell at
// the address a (S is 1 for cells of 2 bytes, 2 for cells of 4 bytes). A
// byte is a part of its cell, the low byte first (little endian).

var M = [];                             // the memory: 32768 cells (16 bit) or 65536 cells (32 bit)
var C;                                  // bytes in a cell: 2 or 4
var S;                                  // address to index: 1 or 2
var MASK;                               // the biggest cell: $FFFF or $FFFFFFFF
var SIGN;                               // the top bit of a cell
var AMASK;                              // the size of the memory - 1: addresses wrap around
var SYS, DATA_START, DATA_END, TIB, PAD, R0, S0;
var DICT_START, DICT_END, MEM_END;
var GUARD;                              // free bytes at DICT_START: a stack underflow lands there
var HALT_XT, HALT_ADDR;                 // the two HALT cells
var RUN_ADDR;                           // two cells at the top: run one word, then HALT
var THROW_ADDR;                         // two cells at the top: run THROW
var WL_FORTH, WL_INTERNALS, VP_START;

// The system variables (doc/vm.md, section 4), one cell each from SYS
var DP_A, VP_A, LATEST_A, STATE_A, BASE_A, TOIN_A, NTIB_A, CURRENT_A, NORDER_A;
var CONTEXT_A, VOCLINK_A, CASESENSE_A, BASEEND_A, FROZENEND_A, HANDLER_A;

// The names of the system variables for Genesis: name, cells, vocabulary
// (F = FORTH, I = INTERNALS). The addresses follow one after the other.
var SV = [['DP', 1, 'I'], ['VP', 1, 'I'], ['LATEST', 1, 'I'], ['STATE', 1, 'I'],
  ['BASE', 1, 'F'], ['>IN', 1, 'I'], ['#TIB', 1, 'I'], ['CURRENT', 1, 'I'],
  ['#ORDER', 1, 'I'], ['CONTEXT', 16, 'I'], ['VOC-LINK', 1, 'I'],
  ['CASE-SENSE', 1, 'I'], ['BASE-END', 1, 'I'], ['FROZEN-END', 1, 'I'],
  ['AUTORUN', 1, 'I'], ['ERRMSG', 2, 'I'], ['HANDLER', 1, 'I']];

function memoryMap() {
  var k = BITS === 16 ? 1 : 2, i, n;
  C = BITS / 8;
  S = k;
  MASK = BITS === 16 ? 0xFFFF : 0xFFFFFFFF;
  SIGN = BITS === 16 ? 0x8000 : 0x80000000;
  SYS = 256;
  DATA_START = 512 * k;
  DATA_END = k === 1 ? 4096 : 65536;
  TIB = DATA_END;
  PAD = TIB + 256;
  R0 = PAD + 256 + 256 * C;             // the return stack: 256 cells below R0
  S0 = R0 + 256 * C;                    // the data stack: 256 cells below S0
  DICT_START = S0;
  GUARD = 8 * C;                        // the first word comes 8 cells above S0
  MEM_END = k === 1 ? 65536 : 262144;
  DICT_END = MEM_END - 16;
  AMASK = MEM_END - 1;
  HALT_XT = DATA_START - 16;
  HALT_ADDR = HALT_XT + C;
  RUN_ADDR = MEM_END - 2 * C;
  THROW_ADDR = MEM_END - 4 * C;
  WL_FORTH = DATA_START;
  WL_INTERNALS = WL_FORTH + 3 * C;
  VP_START = WL_INTERNALS + 3 * C;
  n = MEM_END >> S;
  for (i = 0; i < n; i++) M[i] = 0;     // each cell 0
  DP_A = SYS; VP_A = SYS + C; LATEST_A = SYS + 2 * C; STATE_A = SYS + 3 * C;
  BASE_A = SYS + 4 * C; TOIN_A = SYS + 5 * C; NTIB_A = SYS + 6 * C;
  CURRENT_A = SYS + 7 * C; NORDER_A = SYS + 8 * C; CONTEXT_A = SYS + 9 * C;
  VOCLINK_A = SYS + 25 * C; CASESENSE_A = SYS + 26 * C; BASEEND_A = SYS + 27 * C;
  FROZENEND_A = SYS + 28 * C; HANDLER_A = SYS + 32 * C;
}

// ==== Primitives ===============================================================
// Number, name, vocabulary, flags (64 = compile-only, 128 = immediate).
// 0-3 are the code field types, 4-31 the other mandatory primitives,
// 32-65 the optional ones, 80-88 the HAL, 100-108 exist only for Genesis.

var PRIM = [[4, 'EXIT', 'F', 64], [5, 'LIT', 'I', 64], [6, 'BRANCH', 'I', 64],
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

var prims = '';                         // the primitives with a header, as "0-31,80-88"

function withHeader(op) {               // does Genesis make a header for op?
  return op < 32 || op >= 80 || !pure;
}

function have(op) {                     // can this VM run op? An image may use only these.
  return op <= 65 || (op >= 80 && op <= 93) || op === 98 || op === 99;
}

function primitives() {                 // prims: the numbers below 100 with a header, as ranges
  var ops = [0, 1, 2, 3], ranges = [], i, first, prev;
  for (i = 0; i < PRIM.length; i++)
    if (PRIM[i][0] < 100 && withHeader(PRIM[i][0])) ops.push(PRIM[i][0]);
  first = prev = ops[0];
  for (i = 1; i <= ops.length; i++) {
    if (i < ops.length && ops[i] === prev + 1) { prev = ops[i]; continue; }
    ranges.push(first === prev ? String(first) : first + '-' + prev);
    if (i < ops.length) first = prev = ops[i];
  }
  prims = ranges.join(',');
}

// ==== Helpers ==================================================================
// The access to the memory, the stacks, and the messages of this program.
//
// JScript has one kind of number, with 53 bits for integers. Its bit
// operations take 32 bits and give a number with a sign; ">>> 0" makes
// it a number without a sign again. So (x & MASK) >>> 0 is x as a cell,
// for both cell sizes.

function get(a) { return M[(a & AMASK) >> S]; }   // the cell at a

function put(a, x) { M[(a & AMASK) >> S] = (x & MASK) >>> 0; }   // store the cell x at a

function getb(a) {                      // the byte at a: a part of its cell
  return M[(a & AMASK) >> S] >>> ((a & C - 1) << 3) & 255;
}

function setb(a, b) {                   // store the byte b at a
  var i = (a & AMASK) >> S, n = (a & C - 1) << 3;
  M[i] = (M[i] & ~(255 << n) | (b & 255) << n) >>> 0;
}

var ip = 0, w = 0, sp = 0, rp = 0;      // the registers (doc/vm.md, section 5)

function push(x) { sp -= C; M[sp >> S] = (x & MASK) >>> 0; }

function pop() { var x = M[sp >> S]; sp += C; return x; }

function cell(x) { return (x & MASK) >>> 0; }   // a number as a cell

function sgn(x) { return x & SIGN ? x - MASK - 1 : x; }   // a cell as a signed number

function ntxt(nt) {                     // the xt of the header nt
  return Math.floor((nt + C + 1 + (getb(nt + C) & 31) + C - 1) / C) * C;
}

// The output: TX! collects the characters of a line in ob, and the line
// shows at LF. So the prompts of QUIT stay hidden while a file loads.
var ob = '';                            // the output line that does not show yet

function msg(s) {                       // a message of this program on its own line
  if (ob !== '') { out_line(ob); ob = ''; }
  out_line('  ' + s);
}

function bye(n) {
  if (ob !== '') { out_line(ob); ob = ''; }
  closeFiles();
  leave(n);
}

var THROW_TEXT = {
  '-3': 'data stack overflow', '-4': 'data stack underflow', '-8': 'dictionary overflow',
  '-10': 'division by zero', '-11': 'result out of range', '-13': 'undefined word',
  '-14': 'compile-only word', '-16': 'missing name', '-19': 'name too long',
  '-21': 'unsupported operation', '-24': 'number out of range'
};

function throwText(n) {
  var t = THROW_TEXT[String(n)];
  return t ? t : 'error ' + n;
}

function memstr(a, n) {                 // n bytes at a as a string
  var t = '', j;
  for (j = 0; j < n; j++) t += String.fromCharCode(getb(a + j));
  return t;
}

// ==== Search in the dictionary =================================================
// key holds the name; without CASE-SENSE in upper case.

var key = [];

function upper(c, cs) {                 // a to z as A to Z, without CASE-SENSE
  return !cs && c >= 97 && c <= 122 ? c - 32 : c;
}

function keymem(a, n) {                 // the key from memory
  var cs = get(CASESENSE_A) !== 0, j;
  key = [];
  for (j = 0; j < n; j++) key.push(upper(getb(a + j), cs));
}

function keystr(s) {                    // the key from a string
  var cs = get(CASESENSE_A) !== 0, j;
  key = [];
  for (j = 0; j < s.length; j++) key.push(upper(s.charCodeAt(j), cs));
}

function wlfind(wl) {                   // the nt of the key in the wordlist wl, or 0
  var cs = get(CASESENSE_A) !== 0, kn = key.length, nt = get(wl), a, j, c;
  while (nt) {
    if ((M[(nt + C) >> S] & 31) === kn) {   // the count byte is the low byte of its cell
      a = nt + C + 1;
      for (j = 0; j < kn; j++) {
        c = M[(a + j) >> S] >>> ((a + j & C - 1) << 3) & 255;
        if (!cs && c >= 97 && c <= 122) c -= 32;
        if (c !== key[j]) break;
      }
      if (j === kn) return nt;
    }
    nt = M[nt >> S];
  }
  return 0;
}

function gfind() {                      // the search order, the top first: the nt or 0
  var i, nt;
  if (key.length === 0 || key.length > 31) return 0;
  for (i = get(NORDER_A); i > 0; i--) {
    nt = wlfind(get(CONTEXT_A + (i - 1) * C));
    if (nt) return nt;
  }
  return 0;
}

function sysxt(s) {                     // the xt of the oldest word with exactly this name
  var n = s.length, k, nt, found, j;
  for (k = 0; k < 2; k++) {
    found = 0;
    nt = get(k ? WL_INTERNALS : WL_FORTH);
    while (nt) {
      if ((getb(nt + C) & 31) === n) {
        for (j = 0; j < n; j++) if (getb(nt + C + 1 + j) !== s.charCodeAt(j)) break;
        if (j === n) found = nt;
      }
      nt = get(nt);
    }
    if (found) return ntxt(found);
  }
  return 0;
}

// ==== The virtual machine ======================================================
// ip, w, sp and rp are the registers. SP and RP point to the top cell;
// the stacks grow down. A flag is MASK (true) or 0 (false).

var XT_THROW = 0, XT_QUIT = 0, XT_BOOT = 0;   // from the image
var XT_CATCH = 0, XT_LIT = 0;           // for Genesis
var OUT_A = 0;                          // the address of OUT, the column of the output

function throw_(n) {                    // THROW n in the image: the next word is THROW
  if (!XT_THROW) gfail(throwText(n) + ' (no THROW yet)');
  push(n);
  put(THROW_ADDR, XT_THROW);
  put(THROW_ADDR + C, HALT_XT);
  ip = THROW_ADDR;
}

function runLater(xt) {                 // the VM goes on with xt, then HALT
  put(RUN_ADDR, xt);
  put(RUN_ADDR + C, HALT_XT);
  ip = RUN_ADDR;
}

function runXt(xt) {                    // execute xt; return when it is complete
  runLater(xt);
  vm();
}

function umStar() {                     // UM* ( u1 u2 -- ud ): a product with two cells
  var x = get(sp + C), y = get(sp), lo, hi, xl, xh, yl, yh, ll, lh, hl, hh, mid;
  if (BITS === 16) {                    // 16 x 16 bits fit into a number
    lo = x * y;
    hi = lo >>> 16;
    lo = lo & 0xFFFF;
  } else {                              // a number has 53 bits: four products of 16 bits
    xl = x & 0xFFFF; xh = x >>> 16; yl = y & 0xFFFF; yh = y >>> 16;
    ll = xl * yl; lh = xl * yh; hl = xh * yl; hh = xh * yh;
    mid = (ll >>> 16) + (lh & 0xFFFF) + (hl & 0xFFFF);
    lo = ((mid & 0xFFFF) << 16 | ll & 0xFFFF) >>> 0;
    hi = hh + (lh >>> 16) + (hl >>> 16) + (mid >>> 16);
  }
  put(sp + C, lo);
  put(sp, hi);
}

function umSlashMod() {                 // UM/MOD ( ud u -- rem quot )
  var u = pop(), hi = get(sp), lo = get(sp + C), q = 0, r = hi, top, i;
  if (u === 0) { throw_(-10); return; }
  if (hi >= u) { throw_(-11); return; }  // the quotient would not fit into a cell
  if (hi === 0) {                       // one cell: the division of JScript is exact
    q = Math.floor(lo / u);
    r = lo - q * u;
  } else {
    for (i = 0; i < BITS; i++) {        // division as on paper, one bit at a time
      top = r & SIGN;
      r = ((r << 1 | lo >>> BITS - 1) & MASK) >>> 0;
      lo = (lo << 1 & MASK) >>> 0;
      q = (q << 1 & MASK) >>> 0;
      if (top || r >= u) { r = (r - u & MASK) >>> 0; q = (q | 1) >>> 0; }
    }
  }
  put(sp + C, r);
  put(sp, q);
}

function move() {                       // MOVE ( a1 a2 u -- ), also for areas that overlap
  var u = pop(), a2 = pop(), a1 = pop(), j;
  if (a1 < a2) { for (j = u; j > 0; j--) setb(a2 + j - 1, getb(a1 + j - 1)); }
  else { for (j = 0; j < u; j++) setb(a2 + j, getb(a1 + j)); }
}

function fill() {                       // FILL ( a u c -- )
  var c = pop() & 255, u = pop(), a = pop(), j;
  for (j = 0; j < u; j++) setb(a + j, c);
}

function findname() {                   // FIND-NAME ( a u -- nt | 0 )
  var u = pop(), a = get(sp);
  if (u === 0 || u > 31) put(sp, 0);
  else { keymem(a, u); put(sp, gfind()); }
}

function reboot() {                     // REBOOT ( f -- ): empty stacks, then BOOT or QUIT
  var f = get(sp);
  sp = S0; rp = R0;
  put(STATE_A, 0);
  put(HANDLER_A, 0);
  runLater(f && XT_BOOT ? XT_BOOT : XT_QUIT);
}

// JScript finds a local variable much faster than a global one. So the
// loop of the VM is a function that gets the memory, the constants and
// the registers as its arguments: inside, the names are the same as in
// the rest of the program, but they are local. NEXT and the primitives
// 0 to 65 work there, except the long ones (?RX, TX!, UM*, UM/MOD, MOVE,
// FILL, FIND-NAME). For these and for the HAL, run gives the registers
// back with regs, calls the function of the primitive and ends; vm then
// calls run again. The primitives stand in groups, because JScript looks
// at the cases of a switch one after the other.

function regs(i, x, s, r) { ip = i; w = x; sp = s; rp = r; }   // the registers of run, back to the program

// run: NEXT and the primitives. The result is false when primitive 127
// (HALT) came, else true: vm calls run again.
function run(M, C, S, MASK, SIGN, AMASK, ip, sp, rp) {
  var w, op, x, y, t, a;
  for (;;) {
    w = M[ip >> S]; ip += C;            // NEXT
    op = M[w >> S];
    while (op === 8) { w = M[sp >> S]; sp += C; op = M[w >> S]; }   // EXECUTE
    if (op < 8) {
      switch (op) {
        case 0: rp -= C; M[rp >> S] = ip; ip = w + C; break;                    // DOCOL
        case 1: sp -= C; M[sp >> S] = w + C + C; break;                         // DOVAR
        case 2: sp -= C; M[sp >> S] = M[(w + C) >> S]; break;                   // DOCON
        case 3: sp -= C; M[sp >> S] = w + C + C;                                // DODOES
          rp -= C; M[rp >> S] = ip; ip = M[(w + C) >> S]; break;
        case 4: ip = M[rp >> S]; rp += C; break;                                // EXIT
        case 5: sp -= C; M[sp >> S] = M[ip >> S]; ip += C; break;               // LIT
        case 6: ip = M[ip >> S]; break;                                         // BRANCH
        case 7: if (M[sp >> S]) ip += C; else ip = M[ip >> S];                  // ?BRANCH
          sp += C; break;
      }
    } else if (op < 20) {
      switch (op) {
        case 9: a = M[sp >> S]; M[sp >> S] = M[(a & AMASK) >> S]; break;        // @
        case 10: a = M[sp >> S]; M[(a & AMASK) >> S] = M[(sp + C) >> S];        // !
          sp += C + C; break;
        case 11: a = M[sp >> S];                                                // C@
          M[sp >> S] = M[(a & AMASK) >> S] >>> ((a & C - 1) << 3) & 255; break;
        case 12: a = M[sp >> S]; t = (a & AMASK) >> S; y = (a & C - 1) << 3;    // C!
          M[t] = (M[t] & ~(255 << y) | (M[(sp + C) >> S] & 255) << y) >>> 0;
          sp += C + C; break;
        case 13: sp -= C; M[sp >> S] = M[(sp + C) >> S]; break;                 // DUP
        case 14: sp += C; break;                                                // DROP
        case 15: x = M[sp >> S]; M[sp >> S] = M[(sp + C) >> S];                 // SWAP
          M[(sp + C) >> S] = x; break;
        case 16: sp -= C; M[sp >> S] = M[(sp + C + C) >> S]; break;             // OVER
        case 17: rp -= C; M[rp >> S] = M[sp >> S]; sp += C; break;              // >R
        case 18: sp -= C; M[sp >> S] = M[rp >> S]; rp += C; break;              // R>
        case 19: sp -= C; M[sp >> S] = M[rp >> S]; break;                       // R@
      }
    } else if (op < 29) {
      switch (op) {
        case 20: sp -= C; M[sp >> S] = sp + C; break;                           // SP@: the value of SP before
        case 21: sp = M[sp >> S]; break;                                        // SP!
        case 22: sp -= C; M[sp >> S] = rp; break;                               // RP@
        case 23: rp = M[sp >> S]; sp += C; break;                               // RP!
        case 24: x = M[sp >> S]; sp += C; M[sp >> S] = (M[sp >> S] & x) >>> 0; break;   // AND
        case 25: x = M[sp >> S]; sp += C; M[sp >> S] = (M[sp >> S] | x) >>> 0; break;   // OR
        case 26: x = M[sp >> S]; sp += C; M[sp >> S] = (M[sp >> S] ^ x) >>> 0; break;   // XOR
        case 27: M[sp >> S] = M[sp >> S] & SIGN ? MASK : 0; break;              // 0<
        case 28: t = M[sp >> S] + M[(sp + C) >> S];                             // UM+
          M[(sp + C) >> S] = (t & MASK) >>> 0; M[sp >> S] = t > MASK ? 1 : 0; break;
      }
    } else if (op >= 32 && op < 47) {
      switch (op) {
        case 32: x = M[sp >> S]; sp += C; M[sp >> S] = (M[sp >> S] + x & MASK) >>> 0; break;   // +
        case 33: x = M[sp >> S]; sp += C; M[sp >> S] = (M[sp >> S] - x & MASK) >>> 0; break;   // -
        case 34: M[sp >> S] = (M[sp >> S] + 1 & MASK) >>> 0; break;             // 1+
        case 35: M[sp >> S] = (M[sp >> S] - 1 & MASK) >>> 0; break;             // 1-
        case 36: M[sp >> S] = (-M[sp >> S] & MASK) >>> 0; break;                // NEGATE
        case 37: M[sp >> S] = (~M[sp >> S] & MASK) >>> 0; break;                // INVERT
        case 38: M[sp >> S] = (M[sp >> S] << 1 & MASK) >>> 0; break;            // 2*
        case 39: x = M[sp >> S]; M[sp >> S] = (x >>> 1 | x & SIGN) >>> 0; break;   // 2/
        case 40: t = M[sp >> S]; sp += C;                                       // LSHIFT
          M[sp >> S] = t >= BITS ? 0 : (M[sp >> S] << t & MASK) >>> 0; break;
        case 41: t = M[sp >> S]; sp += C;                                       // RSHIFT
          M[sp >> S] = t >= BITS ? 0 : M[sp >> S] >>> t; break;
        case 42: x = M[sp >> S]; sp += C; M[sp >> S] = M[sp >> S] === x ? MASK : 0; break;   // =
        case 43: M[sp >> S] = M[sp >> S] ? 0 : MASK; break;                     // 0=
        case 44: x = M[sp >> S]; sp += C;                                       // < (signed)
          M[sp >> S] = ((M[sp >> S] ^ SIGN) >>> 0) < ((x ^ SIGN) >>> 0) ? MASK : 0; break;
        case 45: x = M[sp >> S]; sp += C; M[sp >> S] = M[sp >> S] < x ? MASK : 0; break;   // U<
        case 46: x = M[sp >> S]; sp += C; y = M[sp >> S];                       // *: the low cell of the product
          M[sp >> S] = (((x >>> 16) * (y & 65535) + (x & 65535) * (y >>> 16) << 16)
                        + (x & 65535) * (y & 65535) & MASK) >>> 0; break;
      }
    } else if (op >= 49 && op < 66 && op !== 55 && op !== 56 && op !== 58) {
      switch (op) {
        case 49: x = M[(sp + C + C) >> S]; M[(sp + C + C) >> S] = M[(sp + C) >> S];   // ROT
          M[(sp + C) >> S] = M[sp >> S]; M[sp >> S] = x; break;
        case 50: x = M[sp >> S]; sp += C; M[sp >> S] = x; break;                // NIP
        case 51: x = M[sp >> S]; if (x) { sp -= C; M[sp >> S] = x; } break;     // ?DUP
        case 52: sp -= C + C; M[(sp + C) >> S] = M[(sp + 3 * C) >> S];          // 2DUP
          M[sp >> S] = M[(sp + C + C) >> S]; break;
        case 53: sp += C + C; break;                                            // 2DROP
        case 54: a = (M[sp >> S] & AMASK) >> S;                                 // +!
          M[a] = (M[a] + M[(sp + C) >> S] & MASK) >>> 0; sp += C + C; break;
        case 57: a = M[sp >> S]; M[sp >> S] = (a + 1 & MASK) >>> 0;             // COUNT
          sp -= C; M[sp >> S] = M[(a & AMASK) >> S] >>> ((a & C - 1) << 3) & 255; break;
        case 59: rp -= 3 * C;                                                   // (DO)
          M[(rp + C + C) >> S] = M[ip >> S]; M[(rp + C) >> S] = M[(sp + C) >> S];
          M[rp >> S] = M[sp >> S]; sp += C + C; ip += C; break;
        case 60: if (M[sp >> S] === M[(sp + C) >> S]) ip = M[ip >> S];          // (?DO)
          else {
            rp -= 3 * C;
            M[(rp + C + C) >> S] = M[ip >> S]; M[(rp + C) >> S] = M[(sp + C) >> S];
            M[rp >> S] = M[sp >> S]; ip += C;
          }
          sp += C + C; break;
        case 61: t = (M[rp >> S] + 1 & MASK) >>> 0;                             // (LOOP)
          if (t === M[(rp + C) >> S]) { rp += 3 * C; ip += C; }
          else { M[rp >> S] = t; ip = M[ip >> S]; }
          break;
        case 62: t = M[sp >> S]; sp += C;                                       // (+LOOP)
          x = (M[rp >> S] - M[(rp + C) >> S] & MASK) >>> 0;   // index - limit
          y = (x + t & MASK) >>> 0;                           // the same after the step
          M[rp >> S] = (M[rp >> S] + t & MASK) >>> 0;
          if ((x ^ y) & (x ^ t) & SIGN) { rp += 3 * C; ip += C; }   // it crossed limit-1 | limit
          else ip = M[ip >> S];
          break;
        case 63: sp -= C; M[sp >> S] = M[rp >> S]; break;                       // I
        case 64: sp -= C; M[sp >> S] = M[(rp + 3 * C) >> S]; break;             // J
        case 65: rp += 3 * C; break;                                            // UNLOOP
      }
    } else {
      regs(ip, w, sp, rp);
      if (op === 127) return false;     // HALT: back to JScript
      slow(op);
      return true;
    }
  }
}

function slow(op) {                     // the primitives that are functions
  switch (op) {
    case 29: rx(); break;               // ?RX
    case 30: tx(); break;               // TX!
    case 31: break;                     // !IO
    case 47: umStar(); break;           // UM*
    case 48: umSlashMod(); break;       // UM/MOD
    case 55: move(); break;             // MOVE
    case 56: fill(); break;             // FILL
    case 58: findname(); break;         // FIND-NAME
    default: hal(op);
  }
}

// vm: NEXT and the primitives, until primitive 127 (HALT) comes.
function vm() {
  while (run(M, C, S, MASK, SIGN, AMASK, ip, sp, rp)) { }
}

// ==== Terminal: input lines for ?RX, output for TX! ============================
// The input comes from the files of the command line, then from the
// keyboard. A line of a file hides the prompt of QUIT.

function readlines(name) {              // the lines of a file (doc/genesis.md, section 2), or null
  var text = file_read(name), lines, k, s;
  if (text === null) return null;
  lines = text.split('\n');             // the text after the last LF is a line too
  for (k = 0; k < lines.length; k++) {
    s = lines[k];
    if (s.length && s.charCodeAt(s.length - 1) === 13) lines[k] = s.substring(0, s.length - 1);
  }
  return lines;
}

var files = [];                         // core, base (not with -image), then the files of the command line
var fi = 0;                             // the next file to open
var curfile = null, curln = 0, cf = []; // the file that loads now, its line number, its lines
var buildAt = -1;                       // after the file with this number, the system is complete
var errseen = false;                    // a line with "error:" came
var inStr = '', inPos = 0;              // the current input line, with LF

function setline(s) {                   // the next line for ?RX
  if (s.length && s.charCodeAt(s.length - 1) === 13) s = s.substring(0, s.length - 1);
  inStr = s + '\n';
  inPos = 0;
}

function nextLine() {                   // the next line for ?RX
  var line, prompt;
  for (;;) {
    if (curfile !== null && errseen) {  // an error stops all files
      msg(curfile + ':' + curln + ': loading stopped');
      curfile = null;
      if (buildAt >= 0 && fi <= buildAt + 1) { fi = buildAt + 1; buildEnd(); }
      fi = files.length;
    }
    errseen = false;
    if (curfile !== null) {
      if (curln < cf.length) {
        curln++;
        ob = '';                        // the prompt of QUIT does not show
        setline(cf[curln - 1]);
        return;
      }
      curfile = null;
      if (fi === buildAt + 1) buildEnd();
      continue;
    }
    if (fi < files.length) {
      curfile = files[fi++];
      curln = 0;
      cf = readlines(curfile);
      if (cf === null) { msg('cannot read ' + curfile); curfile = null; fi = files.length; }
      continue;
    }
    prompt = ob;                        // the keyboard: the prompt, then the console shows the typed line
    ob = '';
    out_text(prompt);
    line = key_line();
    if (line === null) {                // no more lines: the end
      if (prompt !== '') out_line('');
      bye(0);
    }
    setline(line);
    return;
  }
}

var XT_REFILL = 0, REFILL_END = 0;      // the body of REFILL: QUIT waits for a line there

function refilling() {                  // true if a return address points into REFILL
  var r, x;
  for (r = rp; r < rp + 8 * C && r < R0; r += C) {
    x = get(r);
    if (x > XT_REFILL && x < REFILL_END) return true;
  }
  return false;
}

function rx() {                         // ?RX ( -- c true | false )
  if (inPos >= inStr.length) {
    if (!refilling() && (curfile !== null || fi < files.length)) {
      push(0);                          // a word reads keys while files load: no key (doc/vm.md 9)
      return;
    }
    nextLine();                         // QUIT waits in REFILL, or a word waits at the keyboard
  }
  push(inStr.charCodeAt(inPos++) & 255);
  push(MASK);
}

function bodyend(xt) {                  // the next header in memory after xt
  var best = MEM_END, wl = get(VOCLINK_A), nt;
  while (wl) {
    for (nt = get(wl); nt; nt = get(nt)) if (nt > xt && nt < best) best = nt;
    wl = get(wl + C);
  }
  return best;
}

function findWords() {                  // the words of the image that this program needs
  var xt;
  XT_REFILL = sysxt('REFILL');
  REFILL_END = bodyend(XT_REFILL);
  xt = sysxt('OUT');
  OUT_A = xt ? get(xt + C) : 0;
}

function tx() {                         // TX! ( c -- )
  var c = pop() & 255;
  if (c === 10) {
    if (ob.indexOf('error:') >= 0) errseen = true;
    out_line(ob);
    ob = '';
  } else if (c) ob += String.fromCharCode(c);
}

// ==== Files: HAL 89-93, 98, 99 (doc/vm.md, section 8) ==========================
// A file id (fid) is a number from 10 to 29. OPEN-FILE reads the whole
// file into memory; READ-LINE and READ-FILE take the bytes from there.
// WRITE-FILE collects the bytes; CLOSE-FILE writes the file: CREATE-FILE
// made it new, OPEN-FILE with W/O or R/W writes at its end.

var NFID = 20;
var fds = [];                           // each: null, or { name, fresh, buf, pos, out }

function ret2(x, y) { put(sp + C, x); put(sp, y); }

function ret3(x, y, z) { put(sp + 2 * C, x); put(sp + C, y); put(sp, z); }

function fid(x) {                       // the index of an open fid, or -1
  return x >= 10 && x < 10 + NFID && fds[x - 10] ? x - 10 : -1;
}

function fopen(create) {                // OPEN-FILE, CREATE-FILE ( a u fam -- fid ior )
  var fam = pop() & 3, name = memstr(get(sp + C), get(sp)), k, buf = '', out = null;
  for (k = 0; k < NFID && fds[k]; k++) { }
  if (k === NFID) { ret2(0, -37); return; }
  if (create) {
    if (!file_write(name, '')) { ret2(0, -37); return; }   // a new, empty file
    out = [];
  } else {
    if (fam !== 1) {
      buf = file_read(name);
      if (buf === null) { ret2(0, -38); return; }
    }
    if (fam !== 0) {
      if (!file_there(name) && !file_write(name, '')) { ret2(0, fam === 1 ? -38 : -37); return; }
      out = [];
    }
  }
  fds[k] = { name: name, fresh: create, buf: buf, pos: 0, out: out };
  ret2(k + 10, 0);
}

function fflush(k) {                    // write the collected bytes of the file k; true if it works
  var f = fds[k], old = '';
  if (f.out === null || (f.out.length === 0 && f.fresh)) return true;
  if (!f.fresh) {                       // at the end of the file as it is now
    old = file_read(f.name);
    if (old === null) old = '';
  }
  return file_write(f.name, old + f.out.join(''));
}

function fclose() {                     // CLOSE-FILE ( fid -- ior )
  var k = fid(get(sp)), ok;
  if (k < 0) { put(sp, -37); return; }
  ok = fflush(k);
  fds[k] = null;
  put(sp, ok ? 0 : -37);
}

function closeFiles() {                 // the end of the program: write the files that are still open
  var k;
  for (k = 0; k < NFID; k++) if (fds[k]) { fflush(k); fds[k] = null; }
}

function freadline() {                  // READ-LINE ( a u fid -- u2 f ior )
  var k = fid(get(sp)), u = get(sp + C), a = get(sp + 2 * C), f, e, n, j;
  if (k < 0) { ret3(0, 0, -37); return; }
  f = fds[k];
  if (f.pos >= f.buf.length) { ret3(0, 0, 0); return; }   // the end of the file
  e = f.buf.indexOf('\n', f.pos);
  if (e < 0) e = f.buf.length;
  n = e - f.pos;
  if (n && f.buf.charCodeAt(e - 1) === 13) n--;   // CR LF ends a line too
  if (n > u) { n = u; e = f.pos + u - 1; }   // the buffer is full: the rest of the line comes next
  for (j = 0; j < n; j++) setb(a + j, f.buf.charCodeAt(f.pos + j));
  f.pos = e + 1;
  ret3(n, MASK, 0);
}

function fread() {                      // READ-FILE ( a u fid -- u2 ior )
  var k = fid(pop()), u = get(sp), a = get(sp + C), f, n, j;
  if (k < 0) { ret2(0, -37); return; }
  f = fds[k];
  n = f.buf.length - f.pos;
  if (n > u) n = u;
  if (n < 0) n = 0;
  for (j = 0; j < n; j++) setb(a + j, f.buf.charCodeAt(f.pos + j));
  f.pos += n;
  ret2(n, 0);
}

function fwrite() {                     // WRITE-FILE ( a u fid -- ior )
  var k = fid(pop()), u = pop(), a = get(sp);
  if (k < 0 || fds[k].out === null) { put(sp, -37); return; }
  fds[k].out.push(memstr(a, u));
  put(sp, 0);
}

// ==== Genesis: JScript compiles core.fs (doc/genesis.md) =======================
// The outer interpreter: find the word, then execute or compile it; else
// convert a number. PARSE-NAME, PARSE, HEADER, REVEAL, IMMEDIATE, ",",
// "C,", "[" and "]" are host primitives until core.fs defines Forth
// versions. Genesis handles "?:", "\" and "(" itself until core.fs has them.

var gln = 0;                            // the line of core.fs
var gnt = {};                           // the headers that Genesis removes at the end
var bootColon = 0, bootSemi = 0;        // the first ":" and ";" (line 1 and 2 of core.fs)
var gskip = false, gskipf = false;      // "?:" skips a definition, then its flags
var pa = 0, pn = 0;                     // the word that PARSE-NAME or PARSE found

function gfail(s) {
  var core = files.length ? files[0] : coreFile;
  msg('error: ' + s);
  msg(core + ':' + gln + ': loading stopped');
  msg(core + ' is not complete; the system cannot start');
  bye(1);
}

function gerr(n, word) {                // a THROW code while Genesis runs
  gfail(word ? throwText(n) + ': ' + word : throwText(n));
}

function pname() {                      // PARSE-NAME on TIB: pa pn; >IN moves
  var i = get(TOIN_A), n = get(NTIB_A), st;
  while (i < n && getb(TIB + i) <= 32) i++;
  st = i;
  while (i < n && getb(TIB + i) > 32) i++;
  pn = i - st;
  pa = TIB + st;
  if (i < n) i++;
  put(TOIN_A, i);
}

function pparse(c) {                    // PARSE up to the character c: pa pn
  var i = get(TOIN_A), n = get(NTIB_A), st = i;
  while (i < n && getb(TIB + i) !== c) i++;
  pn = i - st;
  pa = TIB + st;
  if (i < n) i++;
  put(TOIN_A, i);
}

function comma(x) {                     // , ( x -- )
  var t = get(DP_A);
  put(t - t % C, x);
  put(DP_A, t + C);
}

function mkhead(name, wl, flags) {      // a visible header; the result is the xt
  var nt = get(DP_A), j;
  put(nt, get(wl));
  setb(nt + C, name.length + flags);
  for (j = 0; j < name.length; j++) setb(nt + C + 1 + j, name.charCodeAt(j));
  put(wl, nt);
  put(LATEST_A, nt);
  put(DP_A, ntxt(nt));
  return get(DP_A);
}

function gheader() {                    // HEADER ( a u -- ): a new header in CURRENT, not visible yet
  var n = pop(), a = pop(), name, r, nt, xt, j;
  if (n === 0) gerr(-16, '');
  if (n > 31) gerr(-19, '');
  name = memstr(a, n);
  keymem(a, n);
  r = wlfind(get(CURRENT_A));
  if (r && gnt[r] !== 1) msg('redefined ' + name);
  while (get(DP_A) % C) { setb(get(DP_A), 0); put(DP_A, get(DP_A) + 1); }
  nt = get(DP_A);
  xt = Math.floor((nt + C + 1 + n + C - 1) / C) * C;
  if (xt + 4 * C > DICT_END) gerr(-8, '');
  put(nt, get(get(CURRENT_A)));         // the link: the newest word of CURRENT
  setb(nt + C, n);
  for (j = 0; j < n; j++) setb(nt + C + 1 + j, getb(a + j));
  for (j = nt + C + 1 + n; j < xt; j++) setb(j, 0);
  put(DP_A, xt);
  put(LATEST_A, nt);
  if (name === ':' && !bootColon) { bootColon = nt; gnt[nt] = 1; }
  if (name === ';' && !bootSemi) { bootSemi = nt; gnt[nt] = 1; }
}

function gnumber(s) {                   // the value of the word s, or null if it is no number
  var base = get(BASE_A), neg = false, p = s, v = 0, j, c, d;
  if (s.length === 3 && s.charAt(0) === "'" && s.charAt(2) === "'") return s.charCodeAt(1);
  if (p.length > 1 && p.charAt(0) === '-' && '$#%'.indexOf(p.charAt(1)) >= 0) { neg = true; p = p.substring(1); }
  if (p.charAt(0) === '$') { base = 16; p = p.substring(1); }
  else if (p.charAt(0) === '#') { base = 10; p = p.substring(1); }
  else if (p.charAt(0) === '%') { base = 2; p = p.substring(1); }
  if (p.charAt(0) === '-') { neg = !neg; p = p.substring(1); }
  if (p === '' || base < 2 || base > 36) return null;
  for (j = 0; j < p.length; j++) {
    c = p.charCodeAt(j);
    if (c >= 48 && c <= 57) d = c - 48;            // 0-9
    else if (c >= 65 && c <= 90) d = c - 55;       // A-Z
    else if (c >= 97 && c <= 122) d = c - 87;      // a-z
    else return null;
    if (d >= base) return null;
    if (v <= MASK) v = v * base + d;               // above MASK the number is too big anyway
  }
  if (v > MASK || (neg && v > SIGN)) gerr(-24, s);
  return neg ? cell(-v) : v;
}

function gexec(xt) {                    // execute xt; through CATCH once core.fs has it
  var code;
  if (XT_CATCH) {
    push(xt);
    runXt(XT_CATCH);
    code = pop();
    if (code) gerr(sgn(code), '');
  } else runXt(xt);
}

function gline(line) {                  // interpret one line of core.fs
  var j, word, up, save, nt, xt, fl, num;
  if (line.length > 256) gfail('line too long (' + line.length + ' bytes, at most 256)');
  for (j = 0; j < line.length; j++) setb(TIB + j, line.charCodeAt(j));
  put(NTIB_A, line.length);
  put(TOIN_A, 0);
  for (;;) {
    pname();
    if (!pn) return;
    word = memstr(pa, pn);
    if (gskip) {                        // "?:" skips a definition
      if (word === '\\') put(TOIN_A, get(NTIB_A));
      else if (word === '(') pparse(41);
      else if (word === ';') { gskip = false; gskipf = true; }
      continue;
    }
    if (gskipf) {                       // and the flags after it
      up = word.toUpperCase();
      if (up === 'IMMEDIATE' || up === 'UNCHECKED' || up === 'COMPILE-ONLY') continue;
      gskipf = false;
    }
    if (word === '?:') {
      save = get(TOIN_A);
      pname();
      if (!pn) gerr(-16, '');
      keymem(pa, pn);
      if (gfind()) { gskip = true; continue; }
      put(TOIN_A, save);
      keystr(':');
      gexec(ntxt(gfind()));
      continue;
    }
    keymem(pa, pn);
    nt = gfind();
    if (nt) {
      xt = ntxt(nt);
      fl = getb(nt + C);
      if (get(STATE_A) === 0 || fl & 128) {
        if (get(STATE_A) === 0 && fl & 64) gerr(-14, word);
        gexec(xt);
      } else comma(xt);
      continue;
    }
    if (word === '\\') { put(TOIN_A, get(NTIB_A)); continue; }   // comments until core.fs defines them
    if (word === '(') { pparse(41); continue; }
    num = gnumber(word);
    if (num === null) gerr(-13, word);
    if (get(STATE_A)) { comma(XT_LIT); comma(num); } else push(num);
  }
}

function genesis() {                    // compile core.fs; then remove the host primitives
  var i, p, xt, a, consts = [], lines, nt, wl, prev, nx;
  put(HALT_XT, 127);
  put(HALT_ADDR, HALT_XT);
  put(DP_A, DICT_START + GUARD);
  put(VP_A, VP_START);
  put(BASE_A, 10);
  put(WL_FORTH + C, 0);
  put(WL_INTERNALS + C, WL_FORTH);
  put(VOCLINK_A, WL_INTERNALS);
  sp = S0; rp = R0;
  for (i = 0; i < PRIM.length; i++) {   // a header for each primitive
    p = PRIM[i];
    if (!withHeader(p[0])) continue;
    xt = mkhead(p[1], p[2] === 'F' ? WL_FORTH : WL_INTERNALS, p[3]);
    put(xt, p[0]);
    put(DP_A, xt + C);
    if (p[0] === 5) XT_LIT = xt;
    if (p[0] >= 100) gnt[get(LATEST_A)] = 1;
  }
  a = SYS;                              // constants for the system variables, then 9 more
  for (i = 0; i < SV.length; i++) { consts.push([SV[i][0], a, SV[i][2]]); a += SV[i][1] * C; }
  consts.push(['TIB', TIB, 'I'], ['PAD', PAD, 'I'], ['S0', S0, 'I'], ['R0', R0, 'I'],
    ['DATA-END', DATA_END, 'I'], ['WL-FORTH', WL_FORTH, 'I'],
    ['WL-INTERNALS', WL_INTERNALS, 'I'], ['CELL', C, 'I'], ['DICT-END', DICT_END, 'I']);
  for (i = 0; i < consts.length; i++) {
    xt = mkhead(consts[i][0], consts[i][2] === 'F' ? WL_FORTH : WL_INTERNALS, 0);
    put(xt, 2);
    put(xt + C, consts[i][1]);
    put(DP_A, xt + 2 * C);
  }
  put(NORDER_A, 2);
  put(CONTEXT_A, WL_FORTH);
  put(CONTEXT_A + C, WL_INTERNALS);
  put(CURRENT_A, WL_FORTH);
  lines = readlines(files[0]);
  if (lines === null) { msg('cannot read ' + files[0]); bye(1); }
  for (gln = 1; gln <= lines.length; gln++) {
    gline(lines[gln - 1]);
    if (!XT_CATCH) {
      keystr('CATCH');
      nt = gfind();
      if (nt) XT_CATCH = ntxt(nt);
    }
  }
  gln = lines.length;
  for (wl = get(VOCLINK_A); wl; wl = get(wl + C)) {   // remove the host primitives and the first ":" ";"
    prev = 0;
    for (nt = get(wl); nt; nt = nx) {
      nx = get(nt);
      if (gnt[nt] === 1) put(prev ? prev : wl, nx);
      else prev = nt;
    }
  }
}

// ==== Images: Intel HEX ========================================================
// An image is a text file (doc/vm.md, section 10). The first line names
// the cell size and the start words; then come Intel HEX records for the
// system variables, the RAM data up to VP and the dictionary up to DP.

function hex4(x) { return HEX2[x >> 8 & 255] + HEX2[x & 255]; }   // 16 bits as four hex digits

var ulba = 0;                           // the upper 16 bits of the addresses that follow

function hexseg(a, e, lines) {          // the records for the bytes from a up to e (exclusive)
  var hi, n, data, s, j, b;
  while (a < e) {
    hi = Math.floor(a / 65536);
    if (hi !== ulba) {                  // extended linear address: the upper 16 bits
      ulba = hi;
      lines.push(':02000004' + hex4(hi) + HEX2[(256 - (6 + (hi >> 8) + (hi & 255)) % 256) % 256]);
    }
    n = Math.min(e - a, 16, 65536 - (a & 0xFFFF));
    data = '';
    s = n + (a >> 8 & 255) + (a & 255);
    for (j = 0; j < n; j++) { b = getb(a + j); s += b; data += HEX2[b]; }
    lines.push(':' + HEX2[n] + hex4(a & 0xFFFF) + '00' + data + HEX2[(256 - s % 256) % 256]);
    a += n;
  }
}

function writeImage(name, boot) {       // true if it works
  var lines = ['; ucf image bits=' + BITS + ' boot=' + boot + ' quit=' + XT_QUIT +
               ' throw=' + XT_THROW + ' prims=' + prims];
  ulba = 0;                             // a record for the upper bits only above $FFFF
  hexseg(SYS, DATA_START, lines);
  hexseg(DATA_START, get(VP_A), lines);
  hexseg(DICT_START, get(DP_A), lines);
  lines.push(':00000001FF');
  return file_write(name, lines.join('\n') + '\n');
}

var meta = '';                          // the first line of an image
var ndiff = 0, firstdiff = '';          // hexread with compare: the bytes that differ, the first of them

function hexread(name, compare) {       // the records into memory, or compare them with it
  var lines = readlines(name), hi = 0, k, s, rec, j, n, a, t, sum, b, r;
  if (lines === null) { err_line('ucf-wsh.js: cannot read ' + name); bye(1); }
  ndiff = 0; firstdiff = '';
  for (k = 0; k < lines.length; k++) {
    s = lines[k];
    if (s.charAt(0) === ';') { if (s.indexOf(' boot=') >= 0) meta = s; continue; }
    if (s.charAt(0) !== ':' || s.length < 11) continue;
    rec = [];
    sum = 0;
    for (j = 1; j + 1 < s.length; j += 2) { b = parseInt(s.substr(j, 2), 16); rec.push(b); sum += b; }
    n = rec[0]; a = rec[1] << 8 | rec[2]; t = rec[3];
    if (s.length < 11 + 2 * n) { err_line('ucf-wsh.js: ' + name + ':' + (k + 1) + ': short record'); bye(1); }
    if (sum % 256 !== 0) {              // also for a record that is no hex text
      err_line('ucf-wsh.js: ' + name + ':' + (k + 1) + ': wrong checksum');
      bye(1);
    }
    if (t === 0) {
      for (j = 0; j < n; j++) {
        b = rec[4 + j];
        if (!compare) setb(hi + a + j, b);
        else {
          r = getb(hi + a + j);
          if (r !== b) {
            if (!ndiff) firstdiff = '$' + (hi + a + j).toString(16).toUpperCase() + ' (here ' + r + ', there ' + b + ')';
            ndiff++;
          }
        }
      }
    } else if (t === 1) break;
    else if (t === 4) hi = (rec[4] << 8 | rec[5]) * 65536;
  }
}

function metaval(field) {               // the number after " field=" in the first line
  var parts = meta.split(' '), k;
  for (k = 0; k < parts.length; k++)
    if (parts[k].substring(0, field.length + 1) === field + '=') return parseInt(parts[k].substring(field.length + 1), 10);
  return 0;
}

function loadImage(name) {              // memory, start words, primitives
  var parts, k, ranges, j, r, lo, hi, op;
  meta = '';
  hexread(name, false);
  if (meta === '') { err_line('ucf-wsh.js: ' + name + ' is not an image of uCForth'); bye(1); }
  XT_BOOT = metaval('boot'); XT_QUIT = metaval('quit'); XT_THROW = metaval('throw');
  parts = meta.split(' ');
  for (k = 0; k < parts.length; k++) {
    if (parts[k].substring(0, 6) !== 'prims=') continue;
    ranges = parts[k].substring(6).split(',');
    for (j = 0; j < ranges.length; j++) {   // each range lo-hi: this VM must have it
      r = ranges[j].split('-');
      lo = parseInt(r[0], 10);
      hi = r.length > 1 ? parseInt(r[1], 10) : lo;
      for (op = lo; op <= hi; op++)
        if (!have(op)) { err_line('ucf-wsh.js: ' + name + ' needs primitive ' + op); bye(1); }
    }
  }
}

var checkfail = 0;

function checkImage(what, name) {       // compare the memory with a reference
  if (!file_there(name)) { msg('check ' + what + ': cannot read ' + name); checkfail = 1; return; }
  hexread(name, true);
  if (ndiff) {
    msg('check ' + what + ': ' + ndiff + ' bytes differ from ' + name + ', the first at ' + firstdiff);
    checkfail = 1;
  } else msg('check ' + what + ': the same as ' + name);
}

function saveImage() {                  // SAVE-IMAGE ( -- ior )
  var f = env_var('UCF_IMAGE');
  if (f === '') f = 'ucf.hex';
  if (writeImage(f, XT_BOOT ? XT_BOOT : XT_QUIT)) { msg('saved: ' + f); push(0); }
  else push(-37);
}

// ==== The HAL (doc/vm.md, section 8) and the host primitives ===================

var T0 = 0;                             // the clock at the start

function hal(op) {
  var a, t;
  switch (op) {
    case 80: push(now_ms() - T0); break;                                // MILLIS
    case 81: wait_ms(pop()); break;                                     // MS
    case 82: wait_ms(Math.floor(pop() / 1000)); break;                  // US
    case 83: a = pop(); t = pop() & 255; setb(a, getb(a) | t); break;   // BIS!
    case 84: a = pop(); t = pop() & 255; setb(a, getb(a) & ~t); break;  // BIC!
    case 85: saveImage(); break;                                        // SAVE-IMAGE
    case 86: break;                                                     // ERASE-IMAGE
    case 87: reboot(); break;                                           // REBOOT
    case 88: bye(0); break;                                             // BYE
    case 89: fopen(false); break;                                       // OPEN-FILE
    case 90: fopen(true); break;                                        // CREATE-FILE
    case 91: fclose(); break;                                           // CLOSE-FILE
    case 92: fread(); break;                                            // READ-FILE
    case 93: fwrite(); break;                                           // WRITE-FILE
    case 98: freadline(); break;                                        // READ-LINE
    case 99: bye(pop() & 255); break;                                   // (BYE)
    case 100: pname(); push(pa); push(pn); break;                       // PARSE-NAME
    case 101: pparse(pop() & 255); push(pa); push(pn); break;           // PARSE
    case 102: gheader(); break;                                         // HEADER
    case 103: t = get(LATEST_A); if (t) put(get(CURRENT_A), t); break;  // REVEAL
    case 104: a = get(LATEST_A) + C; setb(a, getb(a) | 128); break;     // IMMEDIATE
    case 105: comma(pop()); break;                                      // ,
    case 106: t = get(DP_A); setb(t, pop()); put(DP_A, t + 1); break;   // C,
    case 107: put(STATE_A, 0); break;                                   // [
    case 108: put(STATE_A, MASK); break;                                // ]
    default: throw_(-21);                                               // an unknown primitive
  }
}

// ==== Start ====================================================================

function wordsInForth() {
  var n = 0, nt;
  for (nt = get(WL_FORTH); nt; nt = get(nt)) n++;
  return n;
}

function coreEnd() {                    // Genesis is complete: the seed, and its check
  if (dumpCoreFile !== null && !writeImage(dumpCoreFile, XT_QUIT)) msg('cannot write ' + dumpCoreFile);
  if (check) checkImage('core' + BITS, pfx + 'seed/core' + BITS + '.hex');
}

function buildEnd() {                   // base.fs is complete (doc/genesis.md, section 6)
  var here = get(DP_A);
  put(BASEEND_A, here);
  put(FROZENEND_A, here);
  put(NORDER_A, 1);
  put(CONTEXT_A, WL_FORTH);
  put(CURRENT_A, WL_FORTH);
  put(STATE_A, 0);
  buildAt = -2;
  XT_BOOT = sysxt('BOOT');
  if (dumpFile !== null && !writeImage(dumpFile, XT_BOOT)) msg('cannot write ' + dumpFile);
  if (check) {
    ob = '';                            // the prompt of QUIT is not wanted here
    checkImage('base' + BITS, pfx + 'seed/base' + BITS + '.hex');
    bye(checkfail);
  }
  out_line('uCForth - JScript bootstrap VM - ' + BITS + ' bit' + (pure ? ' - pure' : '') +
           ' - ' + wordsInForth() + ' words in FORTH - built in ' + (now_ms() - T0) + ' ms');
}

function main() {
  var i;
  sys_start();
  tables();
  options();
  memoryMap();
  primitives();
  T0 = now_ms();
  for (i = 0; i < NFID; i++) fds[i] = null;
  if (imageFile !== null) {
    loadImage(imageFile);
    for (i = 0; i < args.length; i++) files.push(args[i]);
    findWords();
    out_line('uCForth - JScript bootstrap VM - ' + BITS + ' bit - ' + wordsInForth() +
             ' words in FORTH - image ' + imageFile);
    sp = S0; rp = R0;
    put(STATE_A, 0);
    put(HANDLER_A, 0);
    runXt(XT_BOOT ? XT_BOOT : XT_QUIT);
  } else {
    files.push(coreFile);
    files.push(baseFile);
    for (i = 0; i < args.length; i++) files.push(args[i]);
    genesis();
    XT_THROW = sysxt('THROW');
    XT_QUIT = sysxt('QUIT');
    if (!XT_QUIT) gfail(files[0] + ' does not define QUIT');
    findWords();
    coreEnd();
    fi = 1;
    buildAt = 1;
    runXt(XT_QUIT);
  }
  bye(0);
}

main();
