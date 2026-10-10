# Forth basics

This manual is for readers who do not know Forth yet. It shows how
Forth works, how to read Forth source, and the common words. Most of it
is true for each Forth system. A paragraph that starts with
**uCForth:** tells where uCForth goes its own way.

Try each example. Start a bootstrap VM (`doc/start.md`, step 1):

    $ LC_ALL=C awk -f bootstrap/awk/ucf.awk
    uCForth - awk bootstrap VM - 16 bit - 163 words in FORTH - built in 1 s
    ok>

You type the text after `ok>` and press the return key. The lines below
it are the answer. The examples under one heading build on each other;
under the next heading uCForth starts new. All examples have 16-bit
cells.

## Contents

1. What Forth is
2. The stack
3. Words and the dictionary
4. Vocabularies and the search order
5. How to read Forth
6. The common words
7. What is special in uCForth
8. How Forth compiles, and why it can compile itself
9. Where next

## 1 What Forth is

Forth is a programming language and a development system in one: an
interpreter, a compiler and tools for tests, all in a few kilobytes.
It has three parts:

- The **data stack** holds the numbers that the words work with.
- The **dictionary** holds the **words**. A word is a name with an
  action. Other progamming languages call it a *function*.
- The **interpreter** reads a line of text and does it word by word,
  at once.

A program is a set of new words, and you make each new word from words
that are already there. A new word is equal to a built-in word: Forth
has no border between the language and the program. You test each word
at the prompt as soon as it is there.

    ok> 3 4 + .
    7
    ok> : SQUARE ( n -- n*n )  DUP * ;
    ok> 12 SQUARE .
    144

The interpreter takes the text between two spaces as one word. The
first line has four words:

| Word | What the interpreter does |
| --- | --- |
| `3` | It is not in the dictionary, but it is a number: 3 goes on the stack. |
| `4` | The same: 4 goes on the stack, above the 3. |
| `+` | It is in the dictionary. It takes two numbers from the stack and puts their sum there. |
| `.` | It is in the dictionary. It takes one number from the stack and shows it. |

The second line makes the new word `SQUARE`. The third line uses it in
the same way as `+`.

So this is the rule for each word of a line:

1. Search the word in the dictionary. If it is there, execute it.
2. Else read it as a number, and put the number on the stack.
3. Else stop with an error:

       ok> 1 2 FOO
         error: undefined word: FOO

After an error the stacks are empty, and the rest of the line is lost.

**uCForth:** The prompt `ok>` comes before your line. Most Forth
systems write `ok` after the line. The search does not look at upper
and lower case: `dup` finds `DUP`. (**uCForth:**: This can be changed.)

## 2 The stack

The data stack holds numbers, one on top of the other. A word takes
what it needs from the top and puts its result on the top. So an
operation comes after its operands. A line needs no parentheses and no
rule such as "multiply before adding":

| Usual form | Forth |
| --- | --- |
| (2 + 3) * 4 | `2 3 + 4 *` |
| 2 + 3 * 4 | `2 3 4 * +` |

`.S` shows the stack and does not change it: first the number of items,
then the items. The top is on the right.

    ok> 1 2 3 .S
    <3> 1 2 3
    ok> + .S
    <2> 1 5
    ok> . .
    5 1

**The stack comment.** Each word has a defined effect on the stack. The
source shows it as a comment: `( before -- after )`, the top on the
right. `+ ( n1 n2 -- n3 )` means: `+` takes two numbers and gives one.
A letter tells what an item is:

| Letter | Meaning |
| --- | --- |
| `n` | a number with a sign |
| `u` | a number without a sign |
| `x` | a cell with any contents |
| `f` | a flag: true (-1, all bits are 1) or false (0) |
| `c` | a character, or a byte |
| `a` | an address |
| `a u` | a text: its address and its length |
| `xt` | an execution token: the address that stands for a word (section 7.1) |
| `"name"` | the word takes the next word of the input line |

Stack comments are text for humans, not part of the program code, nor required.

**Cells.** Each item on the stack is one *cell*: 16 bits, or 32 bits
with the option `-32` of a bootstrap VM. A number, an address and a
flag all have this size. With 16 bits a number goes from -32768 to
32767, or without a sign from 0 to 65535.

**The return stack.** Forth has a second stack. It holds the place
where Forth continues when a word ends. A counted loop keeps its index
there, and a word can put an item there for a short time (`>R`, `R>`
in section 6.2).

**Too few items.** uCForth checks the data stack after each word of a
line. If a word took more than the stack had, the line stops with an
error:

    ok> DROP
      error: data stack underflow

## 3 Words and the dictionary

`:` starts the definition of a new word. The next word of the line is
its name. Then come the words that the new word does, and `;` ends the
definition. Between `:` and `;` Forth does not execute the words: it
compiles them into the new word (section 8). A definition can have more
than one line; the prompt is then `c>`.

    ok> : SQUARE ( n -- n*n )  DUP * ;
    ok> : CUBE ( n -- n*n*n )
    c>   DUP SQUARE * ;
    ok> 3 CUBE .
    27

**Names.** A name has 1 to 31 characters. Each character except the
space can be in a name: `1+`, `.S`, `>R` and `C@` are usual names. So
the spaces are important: `1+` is one word, `1 +` is a number and a
word.

**The dictionary** is the list of all words. The newest word comes
first, and a search starts there. So a new word with an old name hides
the old word; **uCForth** tells you so. A word that uses the old word keeps
it:

    ok> : SQUARE ( n -- n*n )  ." new " DUP * ;
      redefined SQUARE
    ok> 3 SQUARE .
    new 9
    ok> 3 CUBE .
    27

`WORDS` shows the names, the newest first. `SEE` shows what a word is
made of, with the address of each part:

    ok> SEE CUBE

    CUBE colon
      3A0E DUP
      3A10 SQUARE
      3A12 *
      3A14 EXIT

A word in the dictionary has three parts: a *header* with the name and
a link to the older word, a *code field* that tells what kind of word
it is, and a *body*. For a colon word such as `CUBE` the body is the
list of its words; `EXIT` ends it. `doc/vm.md`, section 6, has the
details.

**Small words.** A good Forth word is short: one line, one task, a
clear name. A long word is hard to test, because you must keep its
whole stack in your head. Cut it into parts and test each part at the
prompt.

## 4 Vocabularies and the search order

The dictionary is more than one list. A *vocabulary* is a list of words
with a name. The *search order* is the row of vocabularies in which the
interpreter searches. A word in a vocabulary outside the search order
is there, but the interpreter does not find it. This has three uses:

- A name in one vocabulary does not hide the same name in another.
- You see only the words that you need now.
- A package keeps its helpers to itself (section 7.3).

`ORDER` shows the search order, the first vocabulary on the left. After
`current:` comes the vocabulary that gets the new words. `VOCS` shows
all vocabularies:

    ok> ORDER
    FORTH   current: FORTH
    ok> VOCS
    INTERNALS FORTH

**uCForth** starts with two vocabularies. `FORTH` has the words for
programs: this manual shows all of them. `INTERNALS` has the parts of
the interpreter and of the compiler; `ALSO INTERNALS` puts them into
the search order.

| Word | Stack | Effect |
| --- | --- | --- |
| `VOCABULARY` | `( "name" -- )` | make a new, empty vocabulary with this name |
| `FORTH` | `( -- )` | put FORTH in the first place of the search order, instead of the vocabulary that is there. The name of each vocabulary does this |
| `INTERNALS` | `( -- )` | the same with INTERNALS |
| `ALSO` | `( -- )` | make one more place: copy the first vocabulary. So `ALSO name` adds a vocabulary |
| `PREVIOUS` | `( -- )` | take the first vocabulary away |
| `ONLY` | `( -- )` | only FORTH stays in the search order |
| `DEFINITIONS` | `( -- )` | new words go into the first vocabulary of the search order |
| `ORDER` | `( -- )` | show the search order, and the vocabulary for new words |
| `VOCS` | `( -- )` | show all vocabularies, the newest first |
| `WORDS` | `( -- )` | show the words of the first vocabulary of the search order |

    ok> VOCABULARY TOOLS
    ok> ALSO TOOLS DEFINITIONS
    ok> : HELLO ( -- )  ." hello from TOOLS" ;
    ok> ORDER
    TOOLS FORTH   current: TOOLS
    ok> ONLY FORTH DEFINITIONS
    ok> HELLO
      error: undefined word: HELLO
    ok> ALSO TOOLS  HELLO
    hello from TOOLS
    ok> WORDS

    HELLO

**uCForth:** The search order has 16 places. Each program starts with
`ONLY FORTH DEFINITIONS`. `VOCABULARY` is from the older standards up
to Forth-83; `ALSO`, `ONLY`, `PREVIOUS`, `DEFINITIONS` and `ORDER` are
in Forth-2012 too.

## 5 How to read Forth

Forth source looks strange at first, because it has almost no syntax.
These rules help:

1. **Read from left to right, word by word.** There is nothing else:
   no parentheses for arguments, no precedence, no end of statement.
2. **Follow the stack.** Start with the stack comment of the word.
   Write the stack after each word on paper, until you see it in your
   head.
3. **A space is the only border.** `R>` is one word, and `."` is one
   word with a text behind it.
4. **Read the names.** Forth names follow habits (the table below).
5. **Look up what you do not know.** `SEE name` shows a word. The
   source of all words is in `forth/core.fs` and `forth/base.fs`.

An example from `forth/core.fs`:

    : MAX     ( n1 n2 -- n )        2DUP < IF SWAP THEN DROP ;

`MAX` takes two numbers and gives the greater one. This is the stack
after each word, for `3 7 MAX`:

| Word | Stack after it | Note |
| --- | --- | --- |
| | 3 7 | the start |
| `2DUP` | 3 7 3 7 | |
| `<` | 3 7 -1 | 3 is less than 7: true |
| `IF` | 3 7 | `IF` takes the flag. It is true: go on |
| `SWAP` | 7 3 | |
| `THEN` | 7 3 | the end of the part after `IF` |
| `DROP` | 7 | |

With `7 3 MAX` the flag is false: `IF` jumps to `THEN`, and `DROP`
takes the 3 away. Each step is a test at the prompt:

    ok> 3 7 2DUP < .S
    <3> 3 7 -1
    ok> DROP SWAP DROP .
    7
    ok> 3 7 MAX .  7 3 MAX .
    7 7

**Names.** A sign in a name often tells what the word does:

| Sign | Meaning | Examples |
| --- | --- | --- |
| `@` | fetch: read from memory | `@` `C@` `R@` |
| `!` | store: write to memory | `!` `C!` `+!` |
| `C` | one byte (a character), not a cell | `C@` `C!` `C,` |
| `.` | show | `.` `U.` `.S` `."` |
| `,` | put into the dictionary (compile) | `,` `C,` |
| `?` at the start | maybe: only under a condition | `?DUP` `?DO` |
| `?` at the end | a question: the answer is a flag | `KEY?` |
| `'` | the xt of a word; a variable that holds an xt | `'` `[']` `'SETUP` |
| `>` | to, into: move or convert | `>R` `R>` `NAME>XT` |
| `U` | without a sign | `U.` `U<` `UM*` |
| `2` | two cells, or times 2 | `2DUP` `2DROP` `2*` |
| `0` | compare with zero | `0=` `0<` |
| `(name)` | the part of a compiler word that runs later | `(DO)` `(LOOP)` |
| `[name]` | it runs while Forth compiles | `[']` `[DEFINED]` |
| `"` | a text follows, up to the next `"` | `."` `S"` `ABORT"` |
| `:` at the end | it defines a word: a name follows | `BUFFER:` |
| `#` | a number, or a count | `#S` `#TIB` |

**Comments.** `\` starts a comment up to the end of the line. `(`
starts a comment up to `)`. Both are words: a space must follow.

**The form of the source in uCForth.** A definition has one line where
this is possible. The stack comment follows the name, and a comment at
the end of the line tells what the word does. Two spaces divide the
steps of a definition. Names are in upper case. Three more things come
after a `;`:

| After `;` | Meaning |
| --- | --- |
| `IMMEDIATE` | the word runs while Forth compiles (section 8) |
| `COMPILE-ONLY` | the word is for the inside of a definition only |
| `UNCHECKED` | the word changes the return stack on purpose |

`?:` in place of `:` marks a word that a machine can supply as a
primitive (section 6.1). `>INTERNALS` and `>FORTH` in `core.fs` and
`base.fs` tell where the next words go; a package has `PUBLIC` and
`PRIVATE` for this (section 7.3).

## 6 The common words

The tables of this section and of section 4 have all 163 words of the
vocabulary FORTH. The column "Kind" tells where a word comes from
(section 6.1).

### 6.1 Primitives

A *primitive* is a word that the machine does itself: a few lines in
the language of the bootstrap VM, or some machine instructions in a
kernel. All other words are Forth: lists of other words. uCForth needs
only 32 primitives (`doc/vm.md`, section 7):

| Group | Primitives |
| --- | --- |
| memory | `@ ! C@ C!` |
| data stack | `DUP DROP SWAP OVER` |
| return stack | `>R R> R@` |
| logic | `AND OR XOR 0<` |
| arithmetic | `UM+` (add, and give the carry) |
| control | `EXIT EXECUTE`, and `LIT BRANCH ?BRANCH` for the compiler |
| stack pointers | `SP@ SP! RP@ RP!` |
| console | `?RX TX! !IO` |
| kinds of words | four numbers for the code field: colon word, `CREATE`, constant, `DOES>` |

`UM+`, `LIT`, `BRANCH`, `?BRANCH` and the words for the stack pointers
and the console are in the vocabulary INTERNALS: a program does not
need them. The source builds all other words from these 32, step by
step. Three lines of `forth/core.fs`:

    ?: +       ( n1 n2 -- n3 )           UM+ DROP ;
    ?: NEGATE  ( n -- -n )               INVERT 1 + ;
    ?: ROT     ( x1 x2 x3 -- x2 x3 x1 )  >R SWAP R> SWAP ;

Such a system is slow. So a VM can supply 34 more words as primitives:
`?:` defines a word only if the VM does not have it already. The Forth
version stays in the source; it tells exactly what the word does.
`SEE` shows what a word is on your VM:

    ok> SEE DUP

    DUP primitive 13
    ok> SEE +

    + primitive 32
    ok> SEE MAX

    MAX colon
      1CDA 2DUP
      1CDC <
      1CDE ?BRANCH 1CE4
      1CE2 SWAP
      1CE4 DROP
      1CE6 EXIT

The column "Kind" in the tables:

| Kind | Meaning |
| --- | --- |
| P | a primitive that each machine has |
| O | an optional primitive: Forth in `core.fs` or `base.fs`, and a VM can supply it |
| H | a word of the HAL: the machine does it, each one in its own way |
| F | Forth in `core.fs` or `base.fs` |

### 6.2 The stack

| Word | Stack | Kind | Effect |
| --- | --- | --- | --- |
| `DUP` | `( x -- x x )` | P | copy the top item |
| `DROP` | `( x -- )` | P | take the top item away |
| `SWAP` | `( x1 x2 -- x2 x1 )` | P | exchange the two top items |
| `OVER` | `( x1 x2 -- x1 x2 x1 )` | P | copy the second item to the top |
| `ROT` | `( x1 x2 x3 -- x2 x3 x1 )` | O | bring the third item to the top |
| `NIP` | `( x1 x2 -- x2 )` | O | take the second item away |
| `TUCK` | `( x1 x2 -- x2 x1 x2 )` | F | copy the top item below the second |
| `?DUP` | `( x -- 0 \| x x )` | O | copy x, if it is not 0 |
| `2DUP` | `( x1 x2 -- x1 x2 x1 x2 )` | O | copy the two top items |
| `2DROP` | `( x1 x2 -- )` | O | take the two top items away |
| `DEPTH` | `( -- n )` | F | the number of items on the stack |
| `>R` | `( x -- )` R: `( -- x )` | P | move x to the return stack |
| `R>` | `( -- x )` R: `( x -- )` | P | move x back from the return stack |
| `R@` | `( -- x )` R: `( x -- x )` | P | copy the top item of the return stack |

`>R`, `R>` and `R@` work only inside a definition. The return stack
also holds the place where Forth continues after the word. So a word
must take away each item that it puts there, before it ends:

    ok> 1 2 3 ROT .S
    <3> 2 3 1
    ok> 2DROP DROP
    ok> : SCALE ( n1 n2 k -- n1*k n2*k )  >R  R@ *  SWAP R> *  SWAP ;
    ok> 3 4 10 SCALE .S
    <2> 30 40

A word that works with more than three items at once is hard to read.
Cut it into parts, or put a value into a variable.

### 6.3 Arithmetic

All numbers are whole numbers of one cell. A result that is too big
wraps around, without an error: with 16 bits, `32767 1+` gives -32768.

| Word | Stack | Kind | Effect |
| --- | --- | --- | --- |
| `+` | `( n1 n2 -- n3 )` | O | add |
| `-` | `( n1 n2 -- n3 )` | O | n1 minus n2 |
| `*` | `( n1 n2 -- n3 )` | O | multiply |
| `/` | `( n1 n2 -- n3 )` | F | divide n1 by n2 |
| `MOD` | `( n1 n2 -- n3 )` | F | the remainder of n1 / n2 |
| `/MOD` | `( n1 n2 -- rem quot )` | F | the remainder and the quotient |
| `*/` | `( n1 n2 n3 -- n4 )` | F | n1 * n2 / n3; the product has two cells, so it does not wrap around |
| `NEGATE` | `( n -- -n )` | O | change the sign |
| `ABS` | `( n -- u )` | F | the value without the sign |
| `MIN` | `( n1 n2 -- n )` | F | the smaller number |
| `MAX` | `( n1 n2 -- n )` | F | the greater number |
| `1+` | `( n -- n+1 )` | O | add 1 |
| `1-` | `( n -- n-1 )` | O | subtract 1 |
| `2*` | `( x -- x' )` | O | shift left by one bit: times 2 |
| `2/` | `( n -- n' )` | O | shift right by one bit; the sign stays |
| `UM*` | `( u1 u2 -- ud )` | O | multiply without signs; the product has two cells, the upper cell on top |
| `UM/MOD` | `( ud u -- urem uquot )` | O | divide a number of two cells by u, without signs |

    ok> 7 2 /MOD . .
    3 1
    ok> -7 2 / .  -7 2 MOD .
    -3 -1
    ok> 512 5000 1023 */ .
    2502

`*/` is the way to work without fractions: the last line changes the
value 512 of a converter with 10 bits (0 to 1023) into millivolts
(0 to 5000).

**uCForth:** A division rounds toward zero, and the remainder has the
sign of n1, as in C. Forth-83 rounds down; Forth-2012 lets each system
choose. There are no numbers of two cells at the prompt, and no
floating point numbers.

### 6.4 Comparison and logic

A comparison gives a flag: -1 for true, 0 for false. A word that takes
a flag sees each value other than 0 as true. `AND`, `OR` and `XOR` work
on each bit; with the flags -1 and 0 they also work as the logic
operations.

| Word | Stack | Kind | Effect |
| --- | --- | --- | --- |
| `=` | `( x1 x2 -- f )` | O | true if x1 is equal to x2 |
| `<>` | `( x1 x2 -- f )` | F | true if x1 is not equal to x2 |
| `<` | `( n1 n2 -- f )` | O | true if n1 is less than n2 |
| `>` | `( n1 n2 -- f )` | F | true if n1 is greater than n2 |
| `U<` | `( u1 u2 -- f )` | O | the same as `<`, for numbers without a sign |
| `U>` | `( u1 u2 -- f )` | F | the same as `>`, for numbers without a sign |
| `0=` | `( x -- f )` | O | true if x is 0. It also turns a flag around |
| `0<` | `( n -- f )` | P | true if n is negative |
| `0<>` | `( x -- f )` | F | true if x is not 0 |
| `WITHIN` | `( n lo hi -- f )` | F | true if n is lo or more, and less than hi |
| `AND` | `( x1 x2 -- x3 )` | P | a bit is 1 if it is 1 in x1 and in x2 |
| `OR` | `( x1 x2 -- x3 )` | P | a bit is 1 if it is 1 in x1 or in x2 |
| `XOR` | `( x1 x2 -- x3 )` | P | a bit is 1 if it is 1 in only one of x1 and x2 |
| `INVERT` | `( x -- x' )` | O | change each bit |
| `LSHIFT` | `( x u -- x' )` | O | shift x left by u bits |
| `RSHIFT` | `( x u -- x' )` | O | shift x right by u bits; zeros come in |
| `TRUE` | `( -- -1 )` | F | the flag true |
| `FALSE` | `( -- 0 )` | F | the flag false |

    ok> 3 4 < .  3 4 = .  5 1 10 WITHIN .
    -1 0 -1
    ok> %1100 %1010 AND .  %1100 %1010 OR .  %1100 %1010 XOR .
    8 14 6
    ok> 1 4 LSHIFT .  -1 1 U< .  -1 1 < .
    16 0 -1

The last line shows why `U<` exists: the cell with all bits 1 is -1
with a sign, and 65535 without.

### 6.5 Memory, variables, constants, buffers

The memory is a row of bytes, each with an address. `@` ("fetch") reads
the cell at an address, `!` ("store") writes it. A cell has 2 bytes
(or 4), the low byte first, and its address is a multiple of 2 (or 4).

| Word | Stack | Kind | Effect |
| --- | --- | --- | --- |
| `@` | `( a -- x )` | P | read the cell at a |
| `!` | `( x a -- )` | P | write x into the cell at a |
| `C@` | `( a -- c )` | P | read the byte at a |
| `C!` | `( c a -- )` | P | write the byte c to a |
| `+!` | `( n a -- )` | O | add n to the cell at a |
| `MOVE` | `( a1 a2 u -- )` | O | copy u bytes from a1 to a2 |
| `FILL` | `( a u c -- )` | O | fill u bytes from a on with the byte c |
| `CELLS` | `( n -- n' )` | F | the size of n cells in bytes |
| `CELL+` | `( a -- a' )` | F | the address of the next cell |
| `HERE` | `( -- a )` | F | the next free address of the dictionary |
| `ALLOT` | `( n -- )` | F | take n bytes of the dictionary |
| `UNUSED` | `( -- u )` | F | the free bytes of the dictionary |
| `,` | `( x -- )` | F | put the cell x into the dictionary at `HERE` |
| `C,` | `( c -- )` | F | put the byte c into the dictionary at `HERE` |

You do not work with numbers as addresses. A *defining word* makes a
new word that gives the address, or the value:

| Word | Stack | Kind | Effect |
| --- | --- | --- | --- |
| `CONSTANT` | `( x "name" -- )` | F | name gives the value x |
| `VARIABLE` | `( "name" -- )` | F | a cell for data, 0 at the start; name gives its address |
| `VALUE` | `( x "name" -- )` | F | a cell for data with the start value x; name gives the value |
| `TO` | `( x "name" -- )` | F | write x into the `VALUE` name |
| `BUFFER:` | `( u "name" -- )` | F | u bytes for data; name gives their address |
| `CREATE` | `( "name" -- )` | F | name gives the address of the data that follow in the dictionary; `,`, `C,` and `ALLOT` put them there |

    ok> VARIABLE TOTAL
    ok> 5 TOTAL !  3 TOTAL +!  TOTAL @ .
    8
    ok> 10 CONSTANT TEN
    ok> 100 VALUE LIMIT
    ok> LIMIT TEN + .  150 TO LIMIT  LIMIT .
    110 150
    ok> CREATE PRIMES  2 , 3 , 5 , 7 , 11 ,
    ok> PRIMES 3 CELLS + @ .
    7
    ok> 16 BUFFER: NAME
    ok> S" Forth" NAME SWAP MOVE  NAME 5 TYPE
    Forth

With `CELLS` and `CELL+` a program is right for 16-bit and for 32-bit
cells. Do not write `2 +` for the next cell.

**uCForth:** There are two places for data, and the difference is
important when a program goes into a ROM or a flash:

- `VARIABLE`, `VALUE` and `BUFFER:` take their memory in the *RAM
  data*, outside the dictionary. These data can always change.
- `CREATE`, `,`, `C,` and `ALLOT` make a table in the dictionary, next
  to the words. After `FREEZE` (section 6.13) the dictionary is
  read-only. So a `CREATE` table is for values that stay: a list of
  days, a text, a font.

Most other Forth systems keep the cell of a `VARIABLE` in the
dictionary.

### 6.6 Control

A word does its list from left to right. These words change the way.
They work only inside a definition.

| Form | What it does |
| --- | --- |
| `f IF a THEN` | do a if f is true |
| `f IF a ELSE b THEN` | do a if f is true, else do b |
| `BEGIN a f UNTIL` | do a again and again, until f is true |
| `BEGIN a f WHILE b REPEAT` | do a; if f is false, leave; else do b and start again |
| `BEGIN a AGAIN` | do a without end |
| `limit start DO a LOOP` | do a with the index from start to limit - 1 |
| `limit start DO a n +LOOP` | the same, but the index goes in steps of n |
| `x CASE x1 OF a ENDOF x2 OF b ENDOF c ENDCASE` | do a if x is x1, b if x is x2, else c |

Note the order: the flag comes first, then `IF`. `THEN` does not mean
"in this case": it ends the structure. Read it as "and then go on".

| Word | Stack | Kind | Effect |
| --- | --- | --- | --- |
| `IF` | `( f -- )` | F | if f is false, continue after `ELSE` or `THEN` |
| `ELSE` | `( -- )` | F | the end of the true part: continue after `THEN` |
| `THEN` | `( -- )` | F | the end of `IF` |
| `BEGIN` | `( -- )` | F | the start of a loop |
| `UNTIL` | `( f -- )` | F | if f is false, go back to `BEGIN` |
| `AGAIN` | `( -- )` | F | go back to `BEGIN` |
| `WHILE` | `( f -- )` | F | if f is false, continue after `REPEAT` |
| `REPEAT` | `( -- )` | F | go back to `BEGIN` |
| `DO` | `( limit start -- )` | F | the start of a counted loop |
| `?DO` | `( limit start -- )` | F | the same; if limit and start are equal, skip the loop |
| `LOOP` | `( -- )` | F | add 1 to the index; go back to `DO` until the index is the limit |
| `+LOOP` | `( n -- )` | F | add n to the index; go back until the index crosses the limit |
| `I` | `( -- n )` | O | the index of the loop |
| `J` | `( -- n )` | O | the index of the next loop outside |
| `LEAVE` | `( -- )` | F | leave the loop at once |
| `UNLOOP` | `( -- )` | O | take the loop values from the return stack: before an `EXIT` in a loop |
| `EXIT` | `( -- )` | P | end the word here |
| `RECURSE` | `( -- )` | F | call the word that is in work |
| `EXECUTE` | `( xt -- )` | P | do the word with this xt |
| `CASE` | `( -- )` | F | the start of a choice; the value is on the stack |
| `OF` | `( x1 x2 -- \| x1 )` | F | if the two are equal, take both and do the part up to `ENDOF`; else keep x1 |
| `ENDOF` | `( -- )` | F | the end of a part: continue after `ENDCASE` |
| `ENDCASE` | `( x -- )` | F | the end of the choice; take the value away |

    ok> : .PARITY ( n -- )  1 AND IF ." odd" ELSE ." even" THEN ;
    ok> 7 .PARITY
    odd
    ok> : COUNTDOWN ( n -- )  BEGIN DUP . 1- DUP 0= UNTIL DROP ;
    ok> 3 COUNTDOWN
    3 2 1
    ok> : BITS ( u -- n )  0 SWAP BEGIN DUP WHILE DUP 1 AND ROT + SWAP 1 RSHIFT REPEAT DROP ;
    ok> %1011 BITS .
    3
    ok> : STARS ( n -- )  0 ?DO '*' EMIT LOOP ;
    ok> 5 STARS
    *****
    ok> : DOWN ( -- )  0 10 DO I . -2 +LOOP ;
    ok> DOWN
    10 8 6 4 2 0
    ok> : COLOR ( n -- )
    c>   CASE
    c>     0 OF ." red" ENDOF
    c>     1 OF ." yellow" ENDOF
    c>     ." unknown"
    c>   ENDCASE ;
    ok> 1 COLOR  SPACE  5 COLOR
    yellow unknown

A word is visible only after its `;`. So a word calls itself with
`RECURSE`, not with its name:

    ok> : FAC ( n -- n! )  DUP 2 < IF DROP 1 EXIT THEN  DUP 1- RECURSE * ;
    ok> 5 FAC .
    120

`'` ("tick") gives the xt of a word, and `EXECUTE` does the word with
this xt. So a word can take another word as a parameter:

    ok> : HI ( -- )  ." hi " ;
    ok> : TWICE ( xt -- )  DUP EXECUTE EXECUTE ;
    ok> ' HI TWICE
    hi hi

**uCForth:**

- A control word at the prompt is an error: `compile-only word`. Make a
  definition for each loop.
- The compiler does not check that each `IF` has its `THEN`. After a
  definition, `.S` must show the stack as before.
- `DO` with a limit equal to the start runs 65536 times (with 32 bits:
  2^32 times). Take `?DO` when this case can come.
- A counted loop keeps three cells on the return stack. `I` is right
  only if the word put nothing above them.
- There is no `FOR` ... `NEXT`.

### 6.7 Text

A text is two items on the stack: the address of its first character
and its length (`a u`). A *counted string* is a text in memory with its
length in the first byte; `COUNT` makes `a u` from it.

| Word | Stack | Kind | Effect |
| --- | --- | --- | --- |
| `EMIT` | `( c -- )` | F | show the character c |
| `TYPE` | `( a u -- )` | F | show the text a u |
| `CR` | `( -- )` | F | start a new line |
| `SPACE` | `( -- )` | F | show one space |
| `SPACES` | `( n -- )` | F | show n spaces |
| `."` | `( "text" -- )` | F | show the text up to the next `"`. In a definition: show it when the word runs |
| `S"` | `( "text" -- a u )` | F | give the text up to the next `"` |
| `.(` | `( "text" -- )` | F | show the text up to `)` at once, also inside a definition |
| `COUNT` | `( a -- a+1 c )` | O | read the byte at a, and give the next address too |
| `KEY` | `( -- c )` | F | wait for a key, and give its character |
| `KEY?` | `( -- f )` | F | true if a key waits |
| `ACCEPT` | `( a u -- u' )` | F | read a line of up to u characters to a; u' is its length |
| `(` | `( "text" -- )` | F | a comment up to `)` |
| `\` | `( -- )` | F | a comment up to the end of the line |

    ok> : HELLO ( -- )  ." Hello, " S" world" TYPE CR ;
    ok> HELLO
    Hello, world
    ok> 65 EMIT  3 SPACES  'B' EMIT
    A   B
    ok> CREATE MSG  3 C, 'a' C, 'b' C, 'c' C,
    ok> MSG COUNT TYPE
    abc

**uCForth:**

- `S"` at the prompt has one buffer of 80 characters: the next `S"`
  writes over the text. Inside a definition each text is in the
  dictionary.
- `'A'` gives the code of a character. There is no `CHAR` and no `BL`.
- In a bootstrap VM the keyboard gives whole lines: `KEY` gets its
  character after the return key. On a board each key comes at once.

### 6.8 Numbers

`BASE` holds the number base for input and output, 10 at the start. A
sign before a number gives the base for this one number:

| Input | Value | Meaning |
| --- | --- | --- |
| `42` `-7` | 42, -7 | digits in the base of `BASE` |
| `$FF` | 255 | hexadecimal |
| `#99` | 99 | decimal, with each `BASE` |
| `%1010` | 10 | binary |
| `'A'` | 65 | the code of a character |
| `-$10` | -16 | a minus before the sign |

| Word | Stack | Kind | Effect |
| --- | --- | --- | --- |
| `BASE` | `( -- a )` | - | a system variable: the address of the number base |
| `HEX` | `( -- )` | F | the number base is 16 |
| `DECIMAL` | `( -- )` | F | the number base is 10 |
| `.` | `( n -- )` | F | show n with its sign, then a space |
| `U.` | `( u -- )` | F | show u without a sign, then a space |
| `.R` | `( n w -- )` | F | show n at the right side of a field of w characters |
| `<#` | `( -- )` | F | start a number text |
| `#` | `( ud -- ud' )` | F | make one digit, the lowest one first |
| `#S` | `( ud -- 0 0 )` | F | make all digits that are left |
| `HOLD` | `( c -- )` | F | put the character c before the digits so far |
| `SIGN` | `( n -- )` | F | put a minus there, if n is negative |
| `#>` | `( ud -- a u )` | F | end the number text and give it |

    ok> 255 HEX .  DECIMAL
    FF
    ok> -1 .  -1 U.
    -1 65535
    ok> 40000 U.  40000 .
    40000 -25536
    ok> 42 6 .R  7 6 .R
        42     7
    ok> : .VOLT ( mV -- )  0 <# # # # '.' HOLD #S #> TYPE ."  V" ;
    ok> 1234 .VOLT
    1.234 V

`<#` to `#>` build a text from the right to the left, with any signs
between the digits. They work on a number of two cells, so `.VOLT`
puts a 0 on top of its number first.

A number must fit into a cell: with 16 bits from -32768 to 65535. Else
the line stops with `error: number out of range`. The bits of 40000
and of -25536 are the same; `.` and `U.` read them in two ways.

### 6.9 Errors

`THROW` stops the work with an error number. `CATCH` runs a word and
catches the error. Without a `CATCH` the error goes up to the prompt:
uCForth shows a message, makes both stacks empty, and waits for the
next line.

| Word | Stack | Kind | Effect |
| --- | --- | --- | --- |
| `CATCH` | `( xt -- 0 \| n )` | F | do the word xt. Give 0, or the number n of a `THROW` inside it |
| `THROW` | `( n -- )` | F | if n is not 0: go back to the last `CATCH`, with n |
| `ABORT` | `( -- )` | F | `THROW` with -1; no message |
| `ABORT"` | `( f "text" -- )` | F | if f is true: `THROW` with -2; the message is the text |
| `QUIT` | `( -- )` | F | stop the line, and wait for the next one; the data stack stays |

    ok> : CHECK ( n -- n )  DUP 100 > IF 1 THROW THEN ;
    ok> : TRY ( n -- )  ['] CHECK CATCH IF DROP ." too big" ELSE . THEN ;
    ok> 50 TRY  500 TRY
    50 too big
    ok> : PERCENT ( n -- n )  DUP 100 > ABORT" more than 100" ;
    ok> 150 PERCENT
      error: more than 100
    ok> 1 0 /
      error: division by zero

The system has negative numbers for its own errors: -4 data stack
underflow, -8 dictionary overflow, -10 division by zero, -13 undefined
word, -14 compile-only word, -24 number out of range. Take positive
numbers for your own errors.

### 6.10 Definitions and the compiler

Section 8 tells how these words work together.

| Word | Stack | Kind | Effect |
| --- | --- | --- | --- |
| `:` | `( "name" -- )` | F | start the definition of name |
| `;` | `( -- )` | F | end the definition; the word is visible now |
| `IMMEDIATE` | `( -- )` | F | the newest word runs while Forth compiles |
| `[` | `( -- )` | F | inside a definition: execute the next words now |
| `]` | `( -- )` | F | go on with the compilation |
| `LITERAL` | `( x -- )` | F | compile x as a number into the definition |
| `'` | `( "name" -- xt )` | F | the xt of name |
| `[']` | `( "name" -- )` | F | inside a definition: compile the xt of name as a number |
| `POSTPONE` | `( "name" -- )` | F | inside a compiler word: when this word runs later, name works as if it stood in the source at that place |
| `DOES>` | `( -- )` | F | give the words of a defining word an action of their own |

`CREATE` and `DOES>` make a new defining word. The part before `DOES>`
builds a new word; the part after `DOES>` is what each such word does,
with the address of its data on the stack:

    ok> : TABLE ( "name" -- )  CREATE  DOES> ( i -- x )  SWAP CELLS + @ ;
    ok> TABLE SQUARES  0 , 1 , 4 , 9 , 16 ,
    ok> 3 SQUARES .
    9

### 6.11 Tools

| Word | Stack | Kind | Effect |
| --- | --- | --- | --- |
| `.S` | `( -- )` | F | show the data stack |
| `SEE` | `( "name" -- )` | F | show what name is made of |
| `DUMP` | `( a u -- )` | F | show the memory from a on, in lines of 16 bytes |
| `[DEFINED]` | `( "name" -- f )` | F | true if the search order has name |

`WORDS`, `ORDER` and `VOCS` are in section 4.

    ok> 16 BUFFER: NAME
    ok> NAME 16 0 FILL  S" Forth" NAME SWAP MOVE
    ok> NAME 16 DUMP

    0224: 46 6F 72 74 68 00 00 00 00 00 00 00 00 00 00 00 Forth...........
    ok> [DEFINED] DUP .  [DEFINED] NOTHERE .
    -1 0

### 6.12 Time and registers

These words are the HAL: each machine does them with its own hardware.

| Word | Stack | Kind | Effect |
| --- | --- | --- | --- |
| `MILLIS` | `( -- u )` | H | the milliseconds since the start; the count wraps around |
| `MS` | `( u -- )` | H | wait u milliseconds |
| `US` | `( u -- )` | H | wait u microseconds |
| `BIS!` | `( mask a -- )` | H | set the bits of mask in the register at a |
| `BIC!` | `( mask a -- )` | H | clear the bits of mask in the register at a |

A driver in uCForth is Forth: it writes the registers of a chip with
`!`, `C!`, `BIS!` and `BIC!` (`doc/vm.md`, section 13.5). The
difference of two values of `MILLIS` is right also after the count
wrapped around:

    ok> VARIABLE LAST
    ok> : DUE? ( ms -- f )  MILLIS LAST @ -  SWAP U< 0=  DUP IF MILLIS LAST ! THEN ;

`500 DUE?` is true if 500 ms are over since it was true the last time.

### 6.13 The application: run, freeze, forget

**uCForth:** An application has two words, as a sketch for an Arduino has `setup`
and `loop`: one sets things up, the other runs again and again. Two
variables hold their xts.

| Word | Stack | Kind | Effect |
| --- | --- | --- | --- |
| `'SETUP` | `( -- a )` | F | a variable: the xt that `RUN` does once |
| `'LOOP` | `( -- a )` | F | a variable: the xt that `RUN` does again and again |
| `RUN` | `( -- )` | F | start the application |
| `TURNKEY` | `( -- )` | F | the next start of the system runs the application by itself |
| `FREEZE` | `( -- )` | F | save the system as it is now: all words and the data |
| `EMPTY` | `( -- )` | F | start again at the prompt; the words after the last `FREEZE` are gone |
| `FACTORY` | `( -- )` | F | delete what `FREEZE` saved, and start again |
| `COLD` | `( -- )` | F | start again, as after power on |
| `MARKER` | `( "name" -- )` | F | name forgets itself and each newer word |
| `HIDE` | `( "name" -- )` | F | the search does not find name any more |
| `BYE` | `( -- )` | H | end uCForth |

    ok> VARIABLE TICKS
    ok> : SETUP ( -- )  0 TICKS !  ." start" CR ;
    ok> : STEP ( -- )  1 TICKS +!  ." tick " TICKS @ . CR  1000 MS ;
    ok> ' SETUP 'SETUP !  ' STEP 'LOOP !
    ok> RUN
    start
    tick 1
    tick 2
    ^C
      error: user interrupt
    ok>

Ctrl-C stops the application. This example is from the C bootstrap VM
(`build/ucf-c`, `bootstrap/c/README.md`): it has a clock, and it
catches Ctrl-C. In the bash and the awk bootstrap VM `MS` does not
wait, and Ctrl-C ends the VM.

`FREEZE` saves the system: a bootstrap VM writes the file `ucf.hex`, a
board writes its flash. With `TURNKEY` before `FREEZE`, the next start
runs `RUN` after one second; Ctrl-C or the ESC key in this second gives
the prompt.

    ok> FREEZE
      saved: ucf.hex
    ok> BYE
    $ build/ucf-c -image ucf.hex
    uCForth - C bootstrap VM - 16 bit - 166 words in FORTH - image ucf.hex
    ok> ' STEP EXECUTE
    tick 3

Note the value of `TICKS`: the image holds the data too.

`MARKER` helps while you write a program. The first line of a file
makes a marker. Before you load the file again, the marker takes the
old words away:

    ok> MARKER -WORK
    ok> : TEST ( -- )  ." test" ;
    ok> -WORK
    ok> TEST
      error: undefined word: TEST

**uCForth:** There is no `FORGET`. The part of the dictionary below the
last `FREEZE` is read-only: a marker in this part stops with an error.
A bootstrap VM does not load its image again: there `EMPTY` and `COLD`
only start the interpreter again, and all words stay.

## 7 What is special in uCForth

### 7.1 The xt and the primitive number

When Forth compiles `: CUBE DUP SQUARE * ;`, it puts one cell for each
word into the body of `CUBE`. The cell holds the *xt* of the word: the
address of its code field. The code field holds a small number:

    ok> : SQUARE ( n -- n*n )  DUP * ;
    ok> ' DUP @ .  ' SQUARE @ .
    13 0
    ok> 5 ' SQUARE EXECUTE .
    25

13 is the number of the primitive `DUP`. 0 tells that `SQUARE` is a
colon word: its body is a list of xts, and the machine runs them one
after the other (`doc/vm.md`, section 5).

**uCForth:** Most Forth systems put an address of machine code into the
code field, or they compile machine code. uCForth has only numbers and
addresses of its own memory there. So the same system runs on each
processor, and also in bash or awk. A system that is frozen on one
computer starts on another one: the other machine must only supply the
primitives that the system uses, with the same numbers (`doc/vm.md`,
section 13).

### 7.2 Files

The base of uCForth has no words for files: a board has no files. On
a computer with files, the package FILES adds them (`host/files.fs`).
Put it on the command line:

    $ LC_ALL=C awk -f bootstrap/awk/ucf.awk -- host/files.fs
    uCForth - awk bootstrap VM - 16 bit - 163 words in FORTH - built in 1 s
    ok> INCLUDE examples/hello.fs
    Hello from uCForth
    ok> ORDER
    FILES FORTH   current: FORTH

`INCLUDE name` reads a file line by line, as if you typed it. A file
can load other files. `S" name" INCLUDED` does the same with the name
on the stack. A board gets its source over the serial line: `#include`
of the tool `ucf` sends a file line by line (`host/README.md`).

### 7.3 Packages

A *package* is one file with words for one task: a driver, an
assembler, a tool. It has two vocabularies: `name` for the words that
it gives to you, and `name.I` for its helpers (I: internals). After the
load only `name` is in the search order (`lib/README.md`). Here the
package TEXT loads from the command line, after FILES:

    $ LC_ALL=C awk -f bootstrap/awk/ucf.awk -- host/files.fs lib/text.fs
    uCForth - awk bootstrap VM - 16 bit - 163 words in FORTH - built in 1 s
    ok> ORDER
    TEXT FILES FORTH   current: FORTH
    ok> VOCS
    TEXT.I TEXT PACKAGES.I FILES.I FILES INTERNALS FORTH
    ok> WORDS

    FAIL N>S +PLACE PLACE SPLIT -TRAILING -LEADING S= /STRING 2SWAP

The frame of a package file:

    PACKAGE CLOCK
    VARIABLE TICKS                  \ a helper, in CLOCK.I
    PUBLIC
    : TICK ( -- )  1 TICKS +! ;     \ for the user, in CLOCK
    : TIME ( -- n )  TICKS @ ;
    END-PACKAGE

Each program starts with only FORTH in the search order, also when
packages are in it: `ALSO CLOCK` gives the words of the package.
Packages do not go into the vocabulary FORTH. So the base stays small,
and a program has only the packages that it loads.

### 7.4 uCForth and other Forth systems

| Topic | uCForth |
| --- | --- |
| upper and lower case | the search does not look at it |
| prompt | `ok>` before the line; `c>` inside a definition |
| numbers | `$FF` `#99` `%1010` `'A'`; no numbers of two cells, no floating point |
| division | rounds toward zero |
| flags | -1 and 0 |
| data of `VARIABLE`, `VALUE`, `BUFFER:` | in the RAM data, not in the dictionary |
| tables made with `CREATE` | in the dictionary: read-only after `FREEZE` |
| control words, `>R`, `R>` | only inside a definition |
| counted loop | `DO` ... `LOOP`; no `FOR` ... `NEXT` |
| forget words | `MARKER`; no `FORGET` |
| files | the package FILES on a computer; none on a board |
| vocabularies | `ONLY`, `ALSO`; 16 places; packages with two vocabularies |
| errors | `CATCH` and `THROW`; a message `error:` at the prompt |
| start of an application | `'SETUP`, `'LOOP`, `RUN`, `TURNKEY`, `FREEZE` |
| a word in memory | a list of xts; the code field holds a primitive number |
| not there | `PICK` `ROLL` `2SWAP` `2OVER` `CHAR` `BL` `WORD` `FIND` `EVALUATE` `>NUMBER` `:NONAME` `[IF]`, blocks |

Packages add some of these words: `2SWAP` is in the package TEXT.

## 8 How Forth compiles, and why it can compile itself

### 8.1 Two states

The interpreter has two states. The variable `STATE` (in INTERNALS)
holds the state, and the prompt shows it.

| State | Prompt | A word of the line | A number |
| --- | --- | --- | --- |
| interpret | `ok>` | execute it | put it on the stack |
| compile | `c>` | put its xt into the new definition | put `LIT` and the number into the definition: the new word gives the number when it runs |

`:` makes the header of a new word and sets the state to compile. So
the next words go into the body of the new word, one xt for each. This
is the whole compiler: it puts cells at the end of the dictionary. The
word that does this is `,` ("comma").

### 8.2 IMMEDIATE

How does a definition end, if the compiler only collects words? `;` is
an *immediate* word. The compiler does not compile an immediate word:
it executes it at once. `;` puts the xt of `EXIT` into the definition
and sets the state back to interpret.

Each word can be immediate. `IMMEDIATE` after the `;` marks the newest
word:

    ok> : NOW ( -- )  ." compiling " ;  IMMEDIATE
    ok> : T ( -- )  NOW ;
    compiling
    ok> T
    ok> SEE T

    T colon
      3A12 EXIT

`NOW` ran while the compiler worked on `T`. `T` itself is empty.

### 8.3 The control words are Forth

`IF` is nothing special for the compiler. It is an immediate word, and
it is one line of `forth/core.fs`:

    : IF      ( -- a )       LIT ?BRANCH ,  HERE 0 , ;                 IMMEDIATE COMPILE-ONLY
    : THEN    ( a -- )       HERE SWAP ! ;                              IMMEDIATE COMPILE-ONLY

When the compiler finds `IF`, `IF` runs and does three things:

1. `LIT ?BRANCH ,` puts the xt of `?BRANCH` into the new definition.
   `?BRANCH` is the primitive that jumps if a flag is false.
2. `HERE` puts the address of the next free cell on the stack.
3. `0 ,` puts an empty cell there: the place for the address of the
   jump. Nobody knows this address yet.

Later the compiler finds `THEN`. `THEN` runs too: it takes the address
of the empty cell from the stack and writes `HERE` into it. Now the
jump is complete:

    ok> : TEST ( f -- )  IF ." yes" THEN ;
    ok> SEE TEST

    TEST colon
      39FC ?BRANCH 3A08
      3A00 (S") "yes"
      3A06 TYPE
      3A08 EXIT

So the data stack is the memory of the compiler while it compiles.
`BEGIN`, `UNTIL`, `WHILE`, `DO`, `CASE` and `."` work in the same way.

### 8.4 Your own compiler words

Because the control words are normal definitions, you can add your own.
`POSTPONE` makes a word that compiles other words:

    ok> : UNLESS ( f -- )  POSTPONE 0=  POSTPONE IF ;  IMMEDIATE
    ok> : .ZERO ( n -- )  UNLESS ." zero" THEN ;
    ok> 0 .ZERO
    zero

`UNLESS` runs while the compiler works on `.ZERO`. It compiles `0=`
and then does what `IF` does. `[` and `]` change the state inside a
definition: the words between them run at once. `LITERAL` compiles the
number that they leave:

    ok> : HOUR ( -- n )  [ 60 60 * ] LITERAL ;
    ok> SEE HOUR

    HOUR colon
      3A2E LIT 3600
      3A32 EXIT

### 8.5 Forth compiles itself

The interpreter and the compiler are words too. This is the whole loop,
in the form of a table (`INTERPRET` in `forth/core.fs` has 13 lines):

| The word of the line | State interpret | State compile |
| --- | --- | --- |
| is in the dictionary | execute it | execute it, if it is immediate; else compile its xt with `,` |
| is a number | put it on the stack | compile `LIT` and the number |
| else | `error: undefined word` | the same |

And `:` and `;` are one line each:

    : ;  ( -- )  LIT EXIT ,  DISCARD @ IF 0 DISCARD ! ELSE REVEAL THEN  0 STATE ! ;  IMMEDIATE COMPILE-ONLY
    : :  ( "name" -- )  PARSE-NAME HEADER  0 ,  ] ;

So a Forth system can build itself from a very small start, in its own
language:

1. `forth/core.fs` starts with two lines that make a first `:` and a
   first `;` by hand, from a few words such as `HEADER` and `,`.
2. With these it defines `IF`, `THEN`, the loops, the search in the
   dictionary, the numbers, and at the end the interpreter loop itself
   (`QUIT`), each word from the words before it.
3. From there on the new interpreter reads all other source:
   `forth/base.fs`, the packages, the assemblers, and the tools that
   build uCForth for a chip.

Only one thing is missing at the start: something must read the first
lines of `core.fs`, before an interpreter in Forth exists. A bootstrap
VM has a small loop for this in its own language. It is called Genesis
(`doc/genesis.md`). When `core.fs` is complete, Genesis is not needed
any more. In the end uCForth even builds its own start again, without
the Genesis of a bootstrap VM (`tools/genesis.fs`).

## 9 Where next

| For | Read |
| --- | --- |
| the first steps with files, tests, a program and a board | `doc/start.md` |
| all words of FORTH and INTERNALS in one list | `doc/words.md` |
| the source of all words of this manual | `forth/core.fs`, `forth/base.fs` |
| the machine under the words | `doc/vm.md` |
| Genesis | `doc/genesis.md` |
| packages | `lib/README.md` |
| where the ideas come from | `doc/origins.md` |
| Forth, step by step, in a book | Leo Brodie, *Starting Forth* (1981) and *Thinking Forth* (1984) |
