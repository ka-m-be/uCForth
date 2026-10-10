# Words: the vocabularies FORTH and INTERNALS

This is the reference for the base of uCForth: the 163 words of the
vocabulary FORTH and the 122 words of the vocabulary INTERNALS, as
`forth/core.fs` and `forth/base.fs` make them. `doc/forth.md` explains
the words of FORTH in groups, with examples.

A word of INTERNALS is a part of the interpreter, of the compiler or of
a tool. `ALSO INTERNALS` puts these words into the search order.

The words come in the order of their ASCII codes: first the signs from
`!` to `/`, then the digits, then `:` to `@`, the letters, and at the
end `[`, `\` and `]`.

## The columns

| Column | Contents |
| --- | --- |
| Word | the name |
| Kind | what the word is, then its flags |
| Place | the vocabulary, F for FORTH or I for INTERNALS, then the place of the definition |
| Stack | the effect on the data stack: `( before -- after )`, the top on the right; `R:` is the return stack |
| Meaning | what the word does, in short |

| Kind | The word is |
| --- | --- |
| `prim n` | a mandatory primitive with the number n: each machine has it |
| `opt n` | an optional primitive with the number n: the source has a Forth version (`?:`), and a machine can supply the word |
| `hal n` | a primitive of the HAL with the number n |
| `colon` | a colon word |
| `const` | a constant |
| `var` | a variable: a constant with the address of its cell in the RAM data |
| `sysvar` | a system variable: a constant with its fixed address |
| `table` | made with `CREATE`: its data are in the dictionary |
| `voc` | a vocabulary |

| Flag | Meaning |
| --- | --- |
| `I` | IMMEDIATE: the compiler executes the word and does not compile it |
| `C` | COMPILE-ONLY: only inside a definition; at the prompt the word is an error |
| `U` | UNCHECKED: the word changes the return stack on purpose |

| Place | The definition is in |
| --- | --- |
| `core n` | `forth/core.fs`, section n |
| `base n` | `forth/base.fs`, section n |
| `vm n` | the machine: a primitive, or a constant that Genesis makes; `doc/vm.md`, section n, describes the word |

The letters in the stack effects:

| Letter | Meaning |
| --- | --- |
| `x` | a cell with any contents |
| `n`, `u` | a number with a sign, without a sign |
| `d`, `ud` | a number of two cells, the upper cell on top |
| `f` | a flag: true (-1) or false (0) |
| `c` | a character, or a byte |
| `a` | an address |
| `a u` | a text: its address and its length |
| `xt` | an execution token: the address of the code field of a word |
| `nt` | a name token: the address of the header of a word |
| `wl` | a wordlist: the address of its record |
| `ior` | 0, or the number of an error |
| `"name"`, `"text"` | the word takes a name, or a text, from the input line |

## The words

| Word | Kind | Place | Stack | Meaning |
| --- | --- | --- | --- | --- |
| `!` | prim 10 | F vm 7 | `( x a -- )` | write x into the cell at a |
| `!IO` | prim 31 | I vm 7 | `( -- )` | make the console ready |
| `#` | colon | F core 13 | `( ud -- ud' )` | number text: make one digit, the lowest one first |
| `#>` | colon | F core 13 | `( ud -- a u )` | end the number text and give it |
| `#ORDER` | sysvar | I vm 4 | `( -- a )` | the number of vocabularies in the search order, 1 to 16 |
| `#S` | colon | F core 13 | `( ud -- 0 0 )` | number text: make all digits that are left |
| `#TIB` | sysvar | I vm 4 | `( -- a )` | the length of the input line |
| `'` | colon | F core 11 | `( "name" -- xt )` | the xt of name |
| `'EMIT` | var | I core 8 | `( -- a )` | the xt that `EMIT` executes; `TX!` at the start |
| `'KEY?` | var | I core 12 | `( -- a )` | the xt that `KEY?` executes, with the stack of `?RX`; `?RX` at the start |
| `'LOOP` | var | F base 6 | `( -- a )` | the xt that `RUN` does again and again; `NOOP` at the start |
| `'SETUP` | var | F base 6 | `( -- a )` | the xt that `RUN` does once; `NOOP` at the start |
| `(` | colon I | F core 9 | `( "text" -- )` | a comment up to `)` |
| `(+LOOP)` | opt 62 C | I base 2 | `( n -- )` | the part of `+LOOP` that runs later; the address of the loop start follows in the body |
| `(?DO)` | opt 60 C | I base 2 | `( limit start -- )` | the part of `?DO` that runs later; the address after the loop follows in the body |
| `(ABORT")` | colon | I core 11 | `( f a u -- )` | the part of `ABORT"` that runs later |
| `(DO)` | opt 59 C | I base 2 | `( limit start -- )` | the part of `DO` that runs later: it puts the address after the loop, the limit and the index on the return stack |
| `(DOES>)` | colon U | I base 1 | `( -- )` | the part of `DOES>` that runs later: the newest word gets the code after `DOES>` |
| `(LOOP)` | opt 61 C | I base 2 | `( -- )` | the part of `LOOP` that runs later; the address of the loop start follows in the body |
| `(S")` | colon | I core 9 | `( -- a u )` | the part of `S"` and `."` that runs later; the text follows in the body as a counted string |
| `*` | opt 46 | F core 5 | `( n1 n2 -- n3 )` | multiply |
| `*/` | colon | F base 3 | `( n1 n2 n3 -- n4 )` | n1 * n2 / n3; the product in between has two cells |
| `+` | opt 32 | F core 2 | `( n1 n2 -- n3 )` | add |
| `+!` | opt 54 | F core 2 | `( n a -- )` | add n to the cell at a |
| `+LOOP` | colon IC | F base 2 | `( n -- )` | add n to the index; go back to `DO` until the index crosses the limit. Compile: `( a1 a2 -- )` |
| `,` | colon | F core 3 | `( x -- )` | put the cell x into the dictionary at `HERE` |
| `-` | opt 33 | F core 2 | `( n1 n2 -- n3 )` | n1 minus n2 |
| `.` | colon | F core 13 | `( n -- )` | show n with its sign, then a space |
| `."` | colon I | F core 9 | `( "text" -- )` | show the text up to the next `"`; in a definition: when the word runs |
| `.(` | colon I | F core 9 | `( "text" -- )` | show the text up to `)` at once, also inside a definition |
| `.ERROR` | colon | I core 15 | `( n -- )` | show the message for the `THROW` number n; -1 and -56 show nothing |
| `.H2` | colon | I base 5 | `( c -- )` | show a byte as two hex digits |
| `.HEX` | colon | I base 5 | `( u -- )` | show a cell in hex with all its digits: 4 or 8 |
| `.ID` | colon | I base 4 | `( nt -- )` | show the name of a header |
| `.NAME` | colon | I base 5 | `( xt -- )` | show the name of a word; `?` if no header has this xt |
| `.R` | colon | F core 13 | `( n w -- )` | show n at the right side of a field of w characters |
| `.S` | colon | F base 5 | `( -- )` | show the data stack; it does not change |
| `.WL` | colon | I base 4 | `( wl -- )` | show the name of a wordlist and a space; `?` if it has no name |
| `/` | colon | F base 3 | `( n1 n2 -- quot )` | divide n1 by n2; the quotient rounds toward zero |
| `/MOD` | colon | F base 3 | `( n1 n2 -- rem quot )` | the remainder and the quotient of n1 / n2 |
| `0<` | prim 27 | F vm 7 | `( n -- f )` | true if n is negative |
| `0<>` | colon | F core 5 | `( x -- f )` | true if x is not 0 |
| `0=` | opt 43 | F core 5 | `( x -- f )` | true if x is 0; it also turns a flag around |
| `1+` | opt 34 | F core 2 | `( n -- n+1 )` | add 1 |
| `1-` | opt 35 | F core 2 | `( n -- n-1 )` | subtract 1 |
| `2*` | opt 38 | F core 2 | `( x -- 2x )` | shift left by one bit: times 2 |
| `2/` | opt 39 | F core 5 | `( n -- n' )` | shift right by one bit; the sign stays |
| `2DROP` | opt 53 | F core 2 | `( x1 x2 -- )` | take the two top items away |
| `2DUP` | opt 52 | F core 2 | `( x1 x2 -- x1 x2 x1 x2 )` | copy the two top items |
| `:` | colon | F core 16 | `( "name" -- )` | start the definition of name |
| `;` | colon IC | F core 16 | `( -- )` | end the definition; the word is visible now |
| `<` | opt 44 | F core 5 | `( n1 n2 -- f )` | true if n1 is less than n2 |
| `<#` | colon | F core 13 | `( -- )` | start a number text |
| `<>` | colon | F core 5 | `( x1 x2 -- f )` | true if x1 is not equal to x2 |
| `=` | opt 42 | F core 5 | `( x1 x2 -- f )` | true if x1 is equal to x2 |
| `>` | colon | F core 5 | `( n1 n2 -- f )` | true if n1 is greater than n2 |
| `>BODY` | colon | I base 1 | `( xt -- a )` | the address of the data of a `CREATE` word |
| `>FORTH` | colon | I core 2 | `( -- )` | new words go to FORTH |
| `>IN` | sysvar | I vm 4 | `( -- a )` | the parse position in the input line |
| `>INTERNALS` | colon | I core 2 | `( -- )` | new words go to INTERNALS |
| `>NAME` | colon | I base 5 | `( xt -- nt \| 0 )` | the header of a word: a search through all wordlists |
| `>R` | prim 17 C | F vm 7 | `( x -- )` R: `( -- x )` | move x to the return stack |
| `?:` | colon | I core 16 | `( "name" -- )` | start a definition as `:` does; if name is there already, `;` drops the new word |
| `?BRANCH` | prim 7 C | I vm 7 | `( f -- )` | if f is 0, continue at the address in the next cell of the body |
| `?DO` | colon IC | F base 2 | `( limit start -- )` | start a counted loop; skip it if limit and start are equal. Compile: `( -- a1 a2 )` |
| `?DUP` | opt 51 | F core 5 | `( x -- 0 \| x x )` | copy x, if it is not 0 |
| `?RX` | prim 29 | I vm 7 | `( -- c true \| false )` | a character from the console, if there is one; it does not wait |
| `?STACK` | colon | I core 17 | `( -- )` | `THROW` with -4 if the data stack is below empty |
| `@` | prim 9 | F vm 7 | `( a -- x )` | read the cell at a |
| `ABORT` | colon | F core 6 | `( -- )` | `THROW` with -1; no message |
| `ABORT"` | colon IC | F core 11 | `( f "text" -- )` | if f is true: `THROW` with -2; the message is the text |
| `ABS` | colon | F core 5 | `( n -- u )` | the value of n without its sign |
| `ACCEPT` | colon | F core 12 | `( a u -- u' )` | read a line of up to u characters to a; u' is its length |
| `AGAIN` | colon IC | F core 4 | `( -- )` | go back to `BEGIN`. Compile: `( a -- )` |
| `ALIGN` | colon | I core 7 | `( -- )` | put bytes 0 into the dictionary until `HERE` is at a cell boundary |
| `ALIGNED` | colon | I core 7 | `( a -- a' )` | the next cell boundary from a on |
| `ALLOT` | colon | F core 6 | `( n -- )` | take n bytes of the dictionary; `THROW` -8 if it is full |
| `ALSO` | colon | F base 4 | `( -- )` | one more place in the search order: copy the first vocabulary |
| `AND` | prim 24 | F vm 7 | `( x1 x2 -- x3 )` | a bit is 1 if it is 1 in x1 and in x2 |
| `AUTORUN` | sysvar | I vm 4 | `( -- a )` | true: `BOOT` starts the application; `TURNKEY` sets it |
| `BASE` | sysvar | F vm 4 | `( -- a )` | the number base for input and output, 10 at the start |
| `BASE-END` | sysvar | I vm 4 | `( -- a )` | the end of the base vocabulary in the dictionary |
| `BEGIN` | colon IC | F core 4 | `( -- )` | the start of a loop. Compile: `( -- a )` |
| `BIC!` | hal 84 | F vm 8 | `( mask a -- )` | clear the bits of mask in the byte at a (on a 32-bit chip: in the cell) |
| `BIS!` | hal 83 | F vm 8 | `( mask a -- )` | set the bits of mask in the byte at a (on a 32-bit chip: in the cell) |
| `BOOT` | colon | I base 6 | `( -- )` | the start word: `ONLY FORTH DEFINITIONS`; with `AUTORUN`, `RUN` after 1 s (ESC or Ctrl-C in this second gives the prompt); else `QUIT` |
| `BRANCH` | prim 6 C | I vm 7 | `( -- )` | continue at the address in the next cell of the body |
| `BRANCHY?` | colon | I base 5 | `( xt -- f )` | true for a word that has an address in the next cell of a body (for `SEE`) |
| `BUFFER:` | colon | F core 11 | `( u "name" -- )` | u bytes in the RAM data; name gives their address |
| `BYE` | hal 88 | F vm 8 | `( -- )` | end uCForth |
| `C!` | prim 12 | F vm 7 | `( c a -- )` | write the byte c to a |
| `C,` | colon | F core 3 | `( c -- )` | put the byte c into the dictionary at `HERE` |
| `C@` | prim 11 | F vm 7 | `( a -- c )` | read the byte at a |
| `CASE` | colon IC | F base 2 | `( -- )` | the start of a choice; its value is on the stack. Compile: `( -- 0 )` |
| `CASE-INSENSITIVE` | colon | I base 6 | `( -- )` | the search does not look at upper and lower case; this is the start value |
| `CASE-SENSE` | sysvar | I vm 4 | `( -- a )` | 0: the search does not look at upper and lower case |
| `CASE-SENSITIVE` | colon | I base 6 | `( -- )` | the search looks at upper and lower case |
| `CATCH` | colon | F core 6 | `( xt -- 0 \| n )` | do the word xt; give 0, or the number n of a `THROW` inside it |
| `CELL` | const | I vm 2 | `( -- n )` | the size of a cell in bytes: 2 or 4 |
| `CELL+` | colon | F core 2 | `( a -- a' )` | the address of the next cell |
| `CELLS` | colon | F core 5 | `( n -- n' )` | the size of n cells in bytes |
| `COLD` | colon | F base 6 | `( -- )` | start again, as after power on: `TRUE REBOOT` |
| `COMPILE-ONLY` | colon | I core 3 | `( -- )` | mark the newest word: only inside a definition |
| `COMPILE-ONLY?` | colon | I core 7 | `( nt -- f )` | true if the word is COMPILE-ONLY |
| `CONSTANT` | colon | F core 11 | `( x "name" -- )` | name gives the value x |
| `CONTEXT` | sysvar | I vm 4 | `( -- a )` | the search order: 16 cells with wordlists; the search starts at cell `#ORDER` - 1 |
| `COUNT` | opt 57 | F core 2 | `( a -- a+1 c )` | read the byte at a, and give the next address too |
| `CR` | colon | F core 8 | `( -- )` | start a new line |
| `CREATE` | colon | F core 11 | `( "name" -- )` | name gives the address of the data that follow in the dictionary |
| `CURRENT` | sysvar | I vm 4 | `( -- a )` | the wordlist for new words |
| `DABS` | colon | I base 3 | `( d -- ud )` | the value of a number of two cells without its sign |
| `DATA-ALLOT` | colon | I core 11 | `( n -- a )` | take n bytes in the RAM data; `THROW` -8 if it is full |
| `DATA-END` | const | I vm 3 | `( -- a )` | the end of the RAM data |
| `DECIMAL` | colon | F core 13 | `( -- )` | the number base is 10 |
| `DEFER` | colon | I base 1 | `( "name" -- )` | make a word whose action can change; its xt is in the RAM data, `NOOP` at the start |
| `DEFINITIONS` | colon | F base 4 | `( -- )` | new words go into the first vocabulary of the search order |
| `DEPTH` | colon | F base 3 | `( -- n )` | the number of items on the data stack |
| `DICT-END` | const | I vm 3 | `( -- a )` | the end of the dictionary |
| `DIGIT` | colon | I core 13 | `( u -- c )` | the character for the digit u: 0 to 9, then A to Z |
| `DIGIT?` | colon | I core 14 | `( c base -- u f )` | the value of the character c as a digit; f is true if it is a digit of this base |
| `DIGITS` | colon | I core 14 | `( a u base -- n 1 \| 0 0 \| 0 -1 )` | convert the digits a u: 1 with the number, 0 for a character that is no digit, -1 for a number that is too big |
| `DISCARD` | var | I core 8 | `( -- a )` | not 0: `;` drops the new word; `?:` sets it |
| `DNEGATE` | colon | I base 3 | `( d -- d' )` | change the sign of a number of two cells |
| `DO` | colon IC | F base 2 | `( limit start -- )` | start a counted loop. Compile: `( -- a1 a2 )` |
| `DOES>` | colon IC | F base 1 | `( -- )` | inside a defining word: its words do the code after `DOES>`, with the address of their data |
| `DP` | sysvar | I vm 4 | `( -- a )` | the next free byte of the dictionary; `HERE` is `DP @` |
| `DROP` | prim 14 | F vm 7 | `( x -- )` | take the top item away |
| `DUMP` | colon | F base 5 | `( a u -- )` | show the memory from a on, in lines of 16 bytes |
| `DUP` | prim 13 | F vm 7 | `( x -- x x )` | copy the top item |
| `ECHO` | var | I core 12 | `( -- a )` | not 0: `ACCEPT` shows each character that it reads |
| `ELSE` | colon IC | F core 4 | `( -- )` | the end of the true part: continue after `THEN`. Compile: `( a1 -- a2 )` |
| `EMIT` | colon | F core 8 | `( c -- )` | show the character c |
| `EMPTY` | colon | F base 6 | `( -- )` | start again at the prompt: `FALSE REBOOT` |
| `ENDCASE` | colon IC | F base 2 | `( x -- )` | the end of the choice; take the value away. Compile: `( 0 a.. -- )` |
| `ENDOF` | colon IC | F base 2 | `( -- )` | the end of a part: continue after `ENDCASE`. Compile: `( a -- a' )` |
| `ERASE-IMAGE` | hal 86 | I vm 8 | `( -- )` | delete the saved image |
| `ERRMSG` | sysvar | I vm 4 | `( -- a )` | two cells: the address and the length of the text for an error message |
| `EXECUTE` | prim 8 | F vm 7 | `( xt -- )` | do the word with this xt |
| `EXIT` | prim 4 C | F vm 7 | `( -- )` | end the word here |
| `FACTORY` | colon | F base 6 | `( -- )` | delete the saved image, and start again at the prompt |
| `FALSE` | const | F core 11 | `( -- 0 )` | the flag false |
| `FILL` | opt 56 | F core 5 | `( a u c -- )` | fill u bytes from a on with the byte c |
| `FIND-NAME` | opt 58 | I core 7 | `( a u -- nt \| 0 )` | search the name a u in the search order |
| `FLAG!` | colon | I core 3 | `( mask -- )` | set the bits of mask in the count byte of the newest header |
| `FORGET-UNFINISHED` | colon | I core 10 | `( -- )` | give back the space of a header that is not visible |
| `FORTH` | voc | F base 4 | `( -- )` | put the vocabulary FORTH first in the search order, in place of the one that is there |
| `FREEZE` | colon | F base 6 | `( -- )` | save the system as it is now; the dictionary below `HERE` is read-only from then on |
| `FROZEN-END` | sysvar | I vm 4 | `( -- a )` | the end of the frozen part: below it the dictionary is read-only |
| `HANDLER` | sysvar | I vm 4 | `( -- a )` | the return stack pointer of the newest `CATCH`; 0: none |
| `HEADER` | colon | I core 10 | `( a u -- )` | make a new header with the name a u in `CURRENT`; it is not visible yet |
| `HERE` | colon | F core 2 | `( -- a )` | the next free address of the dictionary |
| `HEX` | colon | F core 13 | `( -- )` | the number base is 16 |
| `HIDE` | colon | F base 6 | `( "name" -- )` | the search does not find name any more |
| `HLD` | var | I core 13 | `( -- a )` | number text: the address of its first character so far |
| `HOLD` | colon | F core 13 | `( c -- )` | number text: put the character c before the digits so far |
| `HOLD-END` | colon | I core 13 | `( -- a )` | the end of the area for a number text: `PAD` + 256 |
| `I` | opt 63 C | F base 2 | `( -- n )` | the index of the loop |
| `IF` | colon IC | F core 4 | `( f -- )` | if f is false, continue after `ELSE` or `THEN`. Compile: `( -- a )` |
| `IMMEDIATE` | colon | F core 3 | `( -- )` | mark the newest word: it runs while Forth compiles |
| `IMMEDIATE?` | colon | I core 7 | `( nt -- f )` | true if the word is IMMEDIATE |
| `INTERNALS` | voc | F base 4 | `( -- )` | put the vocabulary INTERNALS first in the search order, in place of the one that is there |
| `INTERPRET` | colon | I core 17 | `( -- )` | execute or compile each word of the input line |
| `INVERT` | opt 37 | F core 2 | `( x -- x' )` | change each bit |
| `IS` | colon I | I base 1 | `( xt "name" -- )` | set the action of the `DEFER` word name |
| `J` | opt 64 C | F base 2 | `( -- n )` | the index of the next loop outside |
| `KEY` | colon | F core 12 | `( -- c )` | wait for a key, and give its character |
| `KEY?` | colon | F core 12 | `( -- f )` | true if a key waits |
| `KEYBUF` | var | I core 12 | `( -- a )` | one character that `KEY?` has read; -1: none |
| `LAST-CR` | var | I core 12 | `( -- a )` | true: the last line ended with CR, so `ACCEPT` skips a LF that follows |
| `LATEST` | sysvar | I vm 4 | `( -- a )` | the nt of the newest header, also before it is visible |
| `LEAVE` | colon CU | F base 2 | `( -- )` | leave the loop at once |
| `LIT` | prim 5 C | I vm 7 | `( -- x )` | push the next cell of the body |
| `LITERAL` | colon IC | F core 11 | `( x -- )` | compile x as a number into the definition |
| `LOOP` | colon IC | F base 2 | `( -- )` | add 1 to the index; go back to `DO` until the index is the limit. Compile: `( a1 a2 -- )` |
| `LSHIFT` | opt 40 | F core 5 | `( x u -- x' )` | shift x left by u bits |
| `M*` | colon | I base 3 | `( n1 n2 -- d )` | multiply; the product has two cells |
| `MARKER` | colon | F base 6 | `( "name" -- )` | name forgets itself and each newer word, with their data and vocabularies |
| `MAX` | colon | F core 5 | `( n1 n2 -- n )` | the greater number |
| `MESSAGE` | colon | I core 15 | `( n -- a u )` | the text for the `THROW` number n; u is 0 if there is none |
| `MESSAGES` | table | I core 15 | `( -- a )` | the table of the messages: a `THROW` number and a counted string each, 0 at the end |
| `MILLIS` | hal 80 | F vm 8 | `( -- u )` | the milliseconds since the start; the count wraps around |
| `MIN` | colon | F core 5 | `( n1 n2 -- n )` | the smaller number |
| `MOD` | colon | F base 3 | `( n1 n2 -- rem )` | the remainder of n1 / n2; it has the sign of n1 |
| `MOVE` | opt 55 | F core 5 | `( a1 a2 u -- )` | copy u bytes from a1 to a2; the areas can overlap |
| `MS` | hal 81 | F vm 8 | `( u -- )` | wait u milliseconds |
| `NAME-THROW` | colon | I core 7 | `( a u n -- )` | put the text a u into `ERRMSG`, then `THROW` with n |
| `NAME>XT` | colon | I core 7 | `( nt -- xt )` | the xt of a header |
| `NAMED` | colon | I base 6 | `( "name" -- nt )` | the header of name; `THROW` with -13 if the search order does not have it |
| `NEGATE` | opt 36 | F core 2 | `( n -- -n )` | change the sign |
| `NIP` | opt 50 | F core 2 | `( x1 x2 -- x2 )` | take the second item away |
| `NOOP` | colon | I core 2 | `( -- )` | do nothing: the start value of a vector |
| `NUMBER?` | colon | I core 14 | `( a u -- n true \| false )` | convert the text a u to a number: `BASE`, the signs `$ # %`, `'c'`, a minus; `THROW` with -24 if it does not fit a cell |
| `OF` | colon IC | F base 2 | `( x1 x2 -- \| x1 )` | if the two are equal, take both and do the part up to `ENDOF`; else keep x1. Compile: `( -- a )` |
| `ONLY` | colon | F base 4 | `( -- )` | only FORTH stays in the search order |
| `OR` | prim 25 | F vm 7 | `( x1 x2 -- x3 )` | a bit is 1 if it is 1 in x1 or in x2 |
| `ORDER` | colon | F base 4 | `( -- )` | show the search order, and the vocabulary for new words |
| `OUT` | var | I core 8 | `( -- a )` | the column of the output: `EMIT` counts it, a new line sets it to 0 |
| `OVER` | prim 16 | F vm 7 | `( x1 x2 -- x1 x2 x1 )` | copy the second item to the top |
| `PAD` | const | I vm 3 | `( -- a )` | a scratch area of 256 bytes: the number text, and `S"` at the prompt |
| `PARSE` | colon | I core 9 | `( c -- a u )` | take the text of the input line up to the character c |
| `PARSE-NAME` | colon | I core 9 | `( -- a u )` | take the next word of the input line; u is 0 at its end |
| `POSTPONE` | colon IC | F core 11 | `( "name" -- )` | inside a compiler word: when this word runs later, name works as if it stood in the source at that place |
| `PREFIX` | colon | I core 14 | `( c -- base \| 0 )` | the number base for the sign `$`, `#` or `%`; else 0 |
| `PREVIOUS` | colon | F base 4 | `( -- )` | take the first vocabulary out of the search order |
| `PROMPT` | colon | I core 17 | `( -- )` | show `ok>` or `c>` at the start of a line |
| `QUIT` | colon | F core 17 | `( -- )` | the interpreter: empty the return stack, then do lines from the console; the data stack stays |
| `R0` | const | I vm 3 | `( -- a )` | the return stack pointer of the empty return stack |
| `R>` | prim 18 C | F vm 7 | `( -- x )` R: `( x -- )` | move x back from the return stack |
| `R@` | prim 19 C | F vm 7 | `( -- x )` R: `( x -- x )` | copy the top item of the return stack |
| `REBOOT` | hal 87 | I vm 8 | `( f -- )` | start again with empty stacks: `BOOT` if f is true, else `QUIT` |
| `RECURSE` | colon IC | F core 11 | `( -- )` | call the word that is in work |
| `REFILL` | colon | I core 17 | `( -- f )` | read the next line from the console into `TIB`; f is always true |
| `REPEAT` | colon IC | F core 4 | `( -- )` | go back to `BEGIN`. Compile: `( a2 a -- )` |
| `REVEAL` | colon | I core 10 | `( -- )` | make the newest header visible |
| `REVEALED?` | colon | I core 10 | `( -- f )` | true if the newest header is visible |
| `ROT` | opt 49 | F core 2 | `( x1 x2 x3 -- x2 x3 x1 )` | bring the third item to the top |
| `RP!` | prim 23 | I vm 7 | `( a -- )` | set the return stack pointer |
| `RP@` | prim 22 | I vm 7 | `( -- a )` | the return stack pointer |
| `RSHIFT` | opt 41 | F core 5 | `( x u -- x' )` | shift x right by u bits; zeros come in |
| `RUN` | colon | F base 6 | `( -- )` | the application: do `'SETUP` once, then `'LOOP` again and again |
| `S"` | colon I | F core 9 | `( "text" -- a u )` | give the text up to the next `"`; at the prompt it is in one buffer of 80 characters |
| `S,` | colon | I core 9 | `( a u -- )` | put the text a u into the dictionary as a counted string, up to a cell boundary |
| `S0` | const | I vm 3 | `( -- a )` | the data stack pointer of the empty data stack |
| `S>D` | colon | I base 3 | `( n -- d )` | make a number of two cells from n |
| `SAME?` | colon | I core 7 | `( a1 a2 u -- f )` | true if two names of u characters are the same; the rule is in `CASE-SENSE` |
| `SAVE-IMAGE` | hal 85 | I vm 8 | `( -- ior )` | save the image; ior is 0, or an error number |
| `SEE` | colon | F base 5 | `( "name" -- )` | show what name is made of |
| `SEE-BODY` | colon | I base 5 | `( a -- )` | show the body at a, up to its last `EXIT` |
| `SIGN` | colon | F core 13 | `( n -- )` | number text: put a minus there, if n is negative |
| `SM/REM` | colon | I base 3 | `( d n -- rem quot )` | divide a number of two cells by n; the quotient rounds toward zero |
| `SOURCE` | colon | I core 9 | `( -- a u )` | the input line: `TIB` and its length |
| `SP!` | prim 21 | I vm 7 | `( a -- )` | set the data stack pointer |
| `SP@` | prim 20 | I vm 7 | `( -- a )` | the data stack pointer, as it was before this word |
| `SPACE` | colon | F core 8 | `( -- )` | show one space |
| `SPACES` | colon | F core 8 | `( n -- )` | show n spaces |
| `STATE` | sysvar | I vm 4 | `( -- a )` | 0: interpret; true: compile |
| `SWAP` | prim 15 | F vm 7 | `( x1 x2 -- x2 x1 )` | exchange the two top items |
| `THEN` | colon IC | F core 4 | `( -- )` | the end of `IF`. Compile: `( a -- )` |
| `THROW` | colon | F core 6 | `( n -- )` | if n is not 0: go back to the newest `CATCH`, with n |
| `TIB` | const | I vm 3 | `( -- a )` | the input line, 256 bytes |
| `TO` | colon I | F base 1 | `( x "name" -- )` | write x into the `VALUE` name |
| `TOP` | colon | I base 4 | `( -- a )` | the cell of `CONTEXT` that holds the first vocabulary of the search order |
| `TRUE` | const | F core 11 | `( -- -1 )` | the flag true |
| `TUCK` | colon | F core 5 | `( x1 x2 -- x2 x1 x2 )` | copy the top item below the second |
| `TURNKEY` | colon | F base 6 | `( -- )` | set `AUTORUN`: after the next `FREEZE`, the system starts `RUN` by itself |
| `TX!` | prim 30 | I vm 7 | `( c -- )` | send the character c to the console |
| `TYPE` | colon | F core 8 | `( a u -- )` | show the text a u |
| `U.` | colon | F core 13 | `( u -- )` | show u without a sign, then a space |
| `U<` | opt 45 | F core 5 | `( u1 u2 -- f )` | true if u1 is less than u2, without signs |
| `U>` | colon | F core 5 | `( u1 u2 -- f )` | true if u1 is greater than u2, without signs |
| `UM*` | opt 47 | F core 5 | `( u1 u2 -- ud )` | multiply without signs; the product has two cells, the upper cell on top |
| `UM+` | prim 28 | I vm 7 | `( u1 u2 -- u3 c )` | add; c is the carry: 0 or 1 |
| `UM/MOD` | opt 48 | F core 5 | `( ud u -- urem uquot )` | divide a number of two cells by u, without signs |
| `UMAX` | colon | I base 5 | `( u1 u2 -- u )` | the greater number, without signs |
| `UNCHECKED` | colon | I core 3 | `( -- )` | mark the newest word: it changes the return stack on purpose |
| `UNDEFINED` | colon | I core 7 | `( a u -- )` | `THROW` with -13; the message shows the name a u |
| `UNLINK` | colon | I base 6 | `( nt c -- f )` | take the header nt out of the chain that starts in the cell c; true if it was there |
| `UNLOOP` | opt 65 C | F base 2 | `( -- )` | take the loop values from the return stack: before an `EXIT` in a loop |
| `UNTIL` | colon IC | F core 4 | `( f -- )` | if f is false, go back to `BEGIN`. Compile: `( a -- )` |
| `UNUSED` | colon | F core 6 | `( -- u )` | the free bytes of the dictionary: from `HERE` to `DICT-END` |
| `UPC` | colon | I core 7 | `( c -- c' )` | change a to z into A to Z |
| `US` | hal 82 | F vm 8 | `( u -- )` | wait u microseconds |
| `VALLOT` | colon | I core 11 | `( n -- a )` | take n bytes for data: in the extension of the RAM data, if there is one, else in the RAM data |
| `VALUE` | colon | F base 1 | `( x "name" -- )` | a cell in the RAM data with the start value x; name gives the value |
| `VARIABLE` | colon | F core 11 | `( "name" -- )` | a cell in the RAM data, 0 at the start; name gives its address |
| `VOC` | colon | I base 4 | `( wl "name" -- )` | give the wordlist wl a name: a vocabulary |
| `VOC-LINK` | sysvar | I vm 4 | `( -- a )` | the newest wordlist record; the records form a chain |
| `VOCABULARY` | colon | F base 4 | `( "name" -- )` | make a new, empty vocabulary with this name |
| `VOCS` | colon | F base 4 | `( -- )` | show all vocabularies, the newest first |
| `VP` | sysvar | I vm 4 | `( -- a )` | the next free byte of the RAM data |
| `WHILE` | colon IC | F core 4 | `( f -- )` | if f is false, continue after `REPEAT`. Compile: `( a -- a2 a )` |
| `WITHIN` | colon | F core 5 | `( n lo hi -- f )` | true if n is lo or more, and less than hi |
| `WL-FIND` | colon | I core 7 | `( a u wl -- a u nt \| a u 0 )` | search the name a u in one wordlist |
| `WL-FORTH` | const | I vm 3 | `( -- wl )` | the wordlist record of FORTH |
| `WL-INTERNALS` | const | I vm 3 | `( -- wl )` | the wordlist record of INTERNALS |
| `WORDLIST` | colon | I base 4 | `( -- wl )` | make a new, empty wordlist record, without a name |
| `WORDS` | colon | F base 4 | `( -- )` | show the words of the first vocabulary of the search order |
| `XDATA-END` | var | I core 11 | `( -- a )` | the end of the extension of the RAM data; 0: there is none |
| `XOR` | prim 26 | F vm 7 | `( x1 x2 -- x3 )` | a bit is 1 if it is 1 in only one of x1 and x2 |
| `XVP` | var | I core 11 | `( -- a )` | the next free byte of the extension of the RAM data |
| `[` | colon I | F core 3 | `( -- )` | inside a definition: execute the next words now |
| `[']` | colon IC | F core 11 | `( "name" -- )` | inside a definition: compile the xt of name as a number |
| `[DEFINED]` | colon I | F base 5 | `( "name" -- f )` | true if the search order has name |
| `\` | colon I | F core 4 | `( -- )` | a comment up to the end of the line |
| `]` | colon | F core 3 | `( -- )` | go on with the compilation |

## Notes

- **Compiler words.** For a word that works inside a definition (`IF`,
  `DO`, `LOOP` ...), the column Stack shows what happens when the new
  word runs. "Compile:" is the effect of the word itself at compile
  time: it keeps addresses on the data stack, as the stack comment in
  the source shows.
- **The flags of the loop words.** `(DO)`, `(?DO)`, `(LOOP)`, `(+LOOP)`,
  `I`, `J` and `UNLOOP` are COMPILE-ONLY as primitives. Their Forth
  versions (a machine with only the mandatory primitives) are not:
  there `(DO)`, `(?DO)`, `(LOOP)`, `(+LOOP)` and `UNLOOP` are UNCHECKED,
  and `I` and `J` have no flag.
- **Primitives without a name.** The numbers 0 to 3 are the kinds of
  code field (`DOCOL`, `DOVAR`, `DOCON`, `DODOES`), 66 is `DOCODE` and
  127 is `HALT`; they have no header (`doc/vm.md`, section 7). The
  host primitives 100 to 108 exist only while Genesis runs
  (`doc/genesis.md`).
- **More words.** A package adds its own vocabularies: files and
  `INCLUDE` are in FILES (`host/files.fs`), `PACKAGE` and its three
  words in `lib/package.fs`. The README of each folder has their lists.
