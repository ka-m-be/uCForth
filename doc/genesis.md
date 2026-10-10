# Genesis: how a bootstrap VM compiles core.fs

A VM with only primitives cannot read `forth/core.fs`: the interpreter
that reads Forth source is defined in `core.fs` itself. So a bootstrap
VM that starts from the source text has *Genesis*: a small outer
interpreter in its own language (`bootstrap/README.md`). Genesis reads
`core.fs` once. At the end of `core.fs`, the Forth interpreter `QUIT` is
complete, and Genesis stops. From then on, uCForth compiles itself.

This file tells what Genesis must do, so that the image is the same as
the reference image in `seed/`, byte for byte. The terms are in
`doc/vm.md`. The C bootstrap VM (`bootstrap/c/ucf.c`, section
"Genesis") is the reference.

## 1 Before the first line

1. Set the two HALT cells: the cell at HALT (`$01F0`, 32 bit `$03F0`)
   gets 127, the cell after it gets the HALT address.
2. Set `DP` = `S0` + 8 cells: the dictionary starts at `S0`, and its
   first 8 cells stay free (the guard, `doc/vm.md`, section 3). Set
   `VP` = the first free byte of DATA, `BASE` = 10.
3. Make the two wordlist records: `FORTH` at `WL-FORTH`, `INTERNALS` at
   `WL-INTERNALS`. The link of `INTERNALS` points to `FORTH`, the link of
   `FORTH` is 0, `VOC-LINK` points to `INTERNALS`.
4. Make a header for each primitive that the VM supplies, in the order of
   the numbers: 4 to 31, then 32 to 65 (not with `-pure`), then 80 to 88,
   then 100 to 108. The code field holds the number. Names, wordlists
   and flags are in the table below. A header is visible at once.
5. Make a constant (code field 2) for each system variable, in the order
   of `doc/vm.md` section 4, then for `TIB PAD S0 R0 DATA-END WL-FORTH
   WL-INTERNALS CELL DICT-END`. `BASE` goes into `FORTH`, all others into
   `INTERNALS`.
6. Set the search order to `FORTH INTERNALS` (`#ORDER` = 2; cell 0 of
   `CONTEXT` is `FORTH`, cell 1 is `INTERNALS`) and `CURRENT` = `FORTH`.

| Numbers | Wordlist | Compile-only |
| --- | --- | --- |
| `EXIT` (4), `EXECUTE @ ! C@ C! DUP DROP SWAP OVER` (8–16), `AND OR XOR 0<` (24–27) | FORTH | `EXIT` |
| `LIT BRANCH ?BRANCH` (5–7), `SP@ SP! RP@ RP!` (20–23), `UM+ ?RX TX! !IO` (28–31) | INTERNALS | `LIT BRANCH ?BRANCH` |
| `>R R> R@` (17–19) | FORTH | all three |
| 32–57: `+ - 1+ 1- NEGATE INVERT 2* 2/ LSHIFT RSHIFT = 0= < U< * UM* UM/MOD ROT NIP ?DUP 2DUP 2DROP +! MOVE FILL COUNT` | FORTH | – |
| `FIND-NAME` (58), `(DO) (?DO) (LOOP) (+LOOP)` (59–62) | INTERNALS | 59–62 |
| `I J UNLOOP` (63–65) | FORTH | all three |
| `MILLIS MS US BIS! BIC!` (80–84), `BYE` (88) | FORTH | – |
| `SAVE-IMAGE ERASE-IMAGE REBOOT` (85–87) | INTERNALS | – |
| `PARSE-NAME PARSE HEADER REVEAL` (100–103) | INTERNALS | – |
| `IMMEDIATE , C, [ ]` (104–108) | FORTH | – (`[` is IMMEDIATE) |

## 2 One line

Genesis reads `core.fs` line by line, as the text between two LF. The
text after the last LF is one more line, also when it is empty. A CR at
the end of a line goes away. A line has at most 256 bytes.

For each line:

1. Copy the line into TIB. `#TIB` = its length, `>IN` = 0.
2. Take the next word with the rules of `PARSE-NAME` (section 3). If
   there is none, the line is complete.
3. Handle the word as follows, then go back to step 2.

| Case | Action |
| --- | --- |
| Genesis skips a definition (after `?:`, below) | `\` skips the rest of the line, `(` skips up to `)`, `;` ends the skipping; other words do nothing |
| the words just after a skipped definition, as long as they are `IMMEDIATE`, `UNCHECKED` or `COMPILE-ONLY` (any case) | ignore them |
| the word is `?:` | take the next name. If the search order has it, skip the definition. Else set `>IN` back and execute `:` |
| the search order has the word | if `STATE` is 0 or the word is IMMEDIATE: execute it (a COMPILE-ONLY word with `STATE` 0 is error −14); else compile its xt with `,` |
| the word is `\` or `(` and not found | skip the rest of the line, or skip up to `)` |
| the word is a number (section 4) | if `STATE` is 0, push it; else compile `LIT` and the number |
| else | error −13 |

Before `core.fs` defines `CATCH`, Genesis executes a word directly. After
each line it looks for `CATCH`. From then on, it executes each word
through `CATCH`: push the xt, execute `CATCH`, pop the result. A result
other than 0 is an error.

To execute a word, Genesis puts the xt and the HALT address into two free
cells, sets IP to them and runs the VM until primitive 127 comes.

An error stops Genesis. It shows the error, the file and the line.
`core.fs` is then not complete, and the system cannot start.

## 3 The host primitives

These nine primitives exist only while Genesis runs. `core.fs` defines
Forth versions of all of them early; after that no word uses them.

| No. | Name | Stack | Effect |
| --- | --- | --- | --- |
| 100 | `PARSE-NAME` | `( -- a u )` | from `>IN`, skip bytes ≤ 32, then take bytes > 32. a u is the word in TIB. `>IN` moves past it and past one more byte, if there is one. |
| 101 | `PARSE` | `( c -- a u )` | take the text from `>IN` up to the byte c. `>IN` moves past c. |
| 102 | `HEADER` | `( a u -- )` | a new header in `CURRENT`, not visible yet (see below) |
| 103 | `REVEAL` | `( -- )` | make the header `LATEST` the newest word of `CURRENT` |
| 104 | `IMMEDIATE` | `( -- )` | set bit 7 in the count byte of `LATEST` |
| 105 | `,` | `( x -- )` | store x at `DP`; `DP` = `DP` + CELL |
| 106 | `C,` | `( c -- )` | store the byte at `DP`; `DP` = `DP` + 1 |
| 107 | `[` | `( -- )` | `STATE` = 0; IMMEDIATE |
| 108 | `]` | `( -- )` | `STATE` = true |

`HEADER` in detail:

- Length 0 is error −16, a length above 31 is error −19.
- If `CURRENT` has the name already, show `redefined NAME` (not for a
  host primitive and not for the first `:` and `;`).
- Store bytes 0 until `DP` is at a cell boundary.
- nt = `DP`; xt = nt + CELL + 1 + length, rounded up to a cell boundary.
  If xt + 4 cells is above `DICT-END`, error −8.
- Store the link (the newest nt of `CURRENT`), the count byte (only the
  length), the name and bytes 0 up to xt. `DP` = xt, `LATEST` = nt.
- The first header with the name `:` and the first with the name `;`
  are made by hand in line 1 and 2 of `core.fs`. Genesis notes them: it
  removes them at the end.

## 4 Numbers

| Form | Example | Value |
| --- | --- | --- |
| digits in `BASE` | `42`, `-1` | 42, −1 |
| `$` hex, `#` decimal, `%` binary | `$FF`, `#10`, `%101` | 255, 10, 5 |
| a minus before or after the prefix | `-$10`, `$-10` | −16 |
| a character in single quotes | `'A'` | 65 |

Letters are digits from 10 on, in upper or lower case. A digit must be
smaller than the base. A number that does not fit in a cell (below
−2^(bits−1) or above 2^bits − 1) is error −24. A negative number is
stored modulo 2^bits.

## 5 The end of core.fs

1. Remove from all wordlists the headers of the host primitives (100 to
   108) and the first `:` and `;`. The bytes stay in the dictionary; only
   the links change.
2. This is the **seed**: the image after `core.fs` (option
   `-dump-core`). Its start word is `QUIT`.
3. Execute `QUIT`. From now on, `QUIT` reads each line through `?RX`:
   first `base.fs`, then the other files, then the keyboard
   (`doc/vm.md` section 9). The rule for the lines of a file is the same
   as in section 2: the text after the last LF is one more line.

## 6 The end of base.fs

When `base.fs` is complete, the bootstrap VM sets:

- `BASE-END` = `FROZEN-END` = `DP`;
- the search order to `FORTH` only (`#ORDER` = 1), `CURRENT` = `FORTH`;
- `STATE` = 0.

This is the **base image** (option `-dump`); its start word is `BOOT`.
A VM without Genesis (`bootstrap/dos/`) does the same in its own code,
at the same moment: `QUIT` waits for the next line, after the empty
line that comes after the last LF of `base.fs`. One more line of Forth
after `base.fs` (`HERE DUP BASE-END ! FROZEN-END !  ONLY FORTH
DEFINITIONS`) gives a different image: `>IN`, `#TIB` and `HANDLER`
then hold the values of that line, inside `CATCH`.

## 7 The test

A bootstrap VM with Genesis is correct if its seed and its base image
are the same as `seed/core16.hex` and `seed/base16.hex` (32 bit:
`…32.hex`), byte for byte, built with only the mandatory primitives and
the HAL (`-pure`). Each bootstrap VM checks this with `-check`.
