\ json.fs - tests of the package JSON (lib/json.fs): writing byte for byte, reading, the limits
\
\ Load (in the folder ucf), after the package FILES and the tester:
\   ... host/files.fs forth/test/tester.fs lib/test/json.fs
\ S" cannot hold a ": the texts here have ' for each ", and the word Q turns
\ them back, into a place of their own. S" at the prompt holds 80 characters:
\ a longer text is in a word. The buffers are in the dictionary: a VM with
\ 16-bit cells has little RAM data. With 16- and 32-bit cells.

ONLY FORTH DEFINITIONS  ALSO FILES
[DEFINED] T{ S" forth/test/tester.fs" ?INCLUDED
ONLY FORTH ALSO FILES  [DEFINED] TEXT S" lib/text.fs" ?INCLUDED
ONLY FORTH ALSO FILES  S" lib/json.fs" INCLUDED
ONLY FORTH DEFINITIONS  ALSO TEXT  ALSO JSON
DECIMAL

CREATE TEXTS 4096 ALLOT  VARIABLE TEXTS-N  0 TEXTS-N !  VARIABLE DEST
: Q ( a u -- a2 u )   \ the text a u, each ' as ", at a place of its own
  TEXTS TEXTS-N @ + DEST !
  DUP 0 ?DO  OVER I + C@  DUP 39 = IF DROP 34 THEN  DEST @ I + C!  LOOP
  NIP DEST @ SWAP  DUP TEXTS-N +! ;
: IS? ( a u ior a2 u2 -- f ior )   ROT >R STR= R> ;
CREATE OUT 300 ALLOT
-1 1 RSHIFT CONSTANT BIG                  \ the largest number of a cell

\ ---- writing: the messages of the bridge (orga/Archiv/008_Vorschlag_J_JSON.md), byte for byte
: W-RESULT ( -- a u ior )   OUT 300 JSON-INTO  7 S" id" JSON-NUMBER  0 S" code" JSON-NUMBER  JSON-DONE ;
T{ W-RESULT S" {'id':7,'code':0}" Q IS? -> TRUE 0 }T
: W-STATUS ( -- a u ior )
  OUT 300 JSON-INTO  S" 0.1" S" version" JSON-TEXT  S" 192.168.1.20" S" ip" JSON-TEXT
  -61 S" rssi" JSON-NUMBER  3600 S" uptime" JSON-NUMBER
  S" ir" JSON-OBJECT  12 S" heard" JSON-NUMBER  3 S" sent" JSON-NUMBER  0 S" failed" JSON-NUMBER  JSON-END
  JSON-DONE ;
: STATUS-TEXT ( -- a u )
  S" {'version':'0.1','ip':'192.168.1.20','rssi':-61,'uptime':3600,'ir':{'heard':12,'sent':3,'failed':0}}" ;
T{ W-STATUS STATUS-TEXT Q IS? -> TRUE 0 }T
CREATE TIMES4  9000 , 4500 , 560 , 1690 ,
CREATE BYTES4  $60 C, $38 C, $FF C, $00 C,
: W-EVENT ( -- a u ior )
  OUT 300 JSON-INTO  TIMES4 4 S" times" JSON-NUMBERS  S" rty3" S" protocol" JSON-TEXT
  BYTES4 4 S" bytes" JSON-HEX  JSON-DONE ;
T{ W-EVENT S" {'times':[9000,4500,560,1690],'protocol':'rty3','bytes':'6038FF00'}" Q IS? -> TRUE 0 }T
: W-KINDS ( -- a u ior )   \ flags, an empty array and object; JSON-DONE closes what is open
  OUT 300 JSON-INTO  TRUE S" on" JSON-FLAG  FALSE S" off" JSON-FLAG  TIMES4 0 S" none" JSON-NUMBERS
  S" o" JSON-OBJECT JSON-END  S" p" JSON-OBJECT 1 S" q" JSON-NUMBER  JSON-DONE ;
T{ W-KINDS S" {'on':true,'off':false,'none':[],'o':{},'p':{'q':1}}" Q IS? -> TRUE 0 }T
: W-EMPTY ( -- a u ior )   OUT 300 JSON-INTO JSON-DONE ;
T{ W-EMPTY S" {}" Q IS? -> TRUE 0 }T
CREATE ODD  34 C, 92 C, 10 C, 9 C, 1 C, 31 C, 65 C, 47 C,     \ " \ LF TAB 1 31 A /
: W-ESCAPES ( -- a u ior )   OUT 300 JSON-INTO  ODD 8 S" t" JSON-TEXT  JSON-DONE ;
T{ W-ESCAPES S" {'t':'\'\\\n\t\u0001\u001FA/'}" Q IS? -> TRUE 0 }T
: W-SMALL ( -- u ior )   OUT 10 JSON-INTO  12345 S" number" JSON-NUMBER  JSON-DONE ROT DROP ;
T{ W-SMALL -> 10 JSON-FULL }T
: W-DEEP ( n -- ior )   OUT 300 JSON-INTO  0 ?DO  S" a" JSON-OBJECT  LOOP  JSON-DONE NIP NIP ;
T{ 7 W-DEEP 8 W-DEEP -> 0 JSON-LIMIT }T
: W-BIG ( -- a u ior )   OUT 300 JSON-INTO  BIG S" max" JSON-NUMBER  BIG NEGATE S" min" JSON-NUMBER  JSON-DONE ;
T{ W-BIG NIP NIP -> 0 }T

\ ---- JSON-CHECK
: VALID? ( a u -- ior )   Q JSON-CHECK ;
T{ S" {}" VALID? S" []" VALID? S"  true " VALID? S" -0" VALID? S" 1.5e-3" VALID? -> 0 0 0 0 0 }T
T{ S" {'a':[1,2,{'b':null}],'c':'x\u0041','d':-12.5E+2,'e':false}" VALID? -> 0 }T
T{ S" [ 1 , [ ] , { } , 'é\/' ]" VALID? -> 0 }T
T{ S" {" VALID? S" {'a'}" VALID? S" {'a':}" VALID? S" {'a':1,}" VALID? S" [1,]" VALID? -> -471 -471 -471 -471 -471 }T
T{ S" 01" VALID? S" 'abc" VALID? S" tru" VALID? S" {'a':1}x" VALID? S" 1." VALID? -> -471 -471 -471 -471 -471 }T
T{ S" .5" VALID? S" -" VALID? S" [1 2]" VALID? S" '\x'" VALID? S" '\u12G4'" VALID? -> -471 -471 -471 -471 -471 }T
T{ S" " VALID? S" {1:2}" VALID? S" nulls" VALID? S" 1e" VALID? S" trux" VALID? -> -471 -471 -471 -471 -471 }T
: RAW-LF ( -- ior )   S" 'a b'" Q  OVER 2 + 10 SWAP C!  JSON-CHECK ;   \ a line feed inside a text
T{ RAW-LF -> -471 }T
\ between the parts each character below 33 is white space, as for -LEADING
: RAW-WS ( c -- ior )   >R  S" { 'a' : 1 }" Q  OVER 1+ R> SWAP C!  JSON-CHECK ;
T{ 12 RAW-WS 0 RAW-WS 1 RAW-WS 32 RAW-WS 33 RAW-WS -> 0 0 0 0 -471 }T
T{ S" [[[[[[[[1]]]]]]]]" VALID? S" [[[[[[[[[1]]]]]]]]]" VALID? -> 0 JSON-LIMIT }T
T{ S" {'a':{'a':{'a':{'a':{'a':{'a':{'a':{'a':1}}}}}}}}" VALID? -> 0 }T
CREATE LONG 4097 ALLOT
: LONG? ( u -- ior )   LONG 4097 32 FILL  '0' LONG C!  LONG SWAP JSON-CHECK ;
T{ 4096 LONG? 4097 LONG? -> 0 JSON-LIMIT }T

\ ---- JSON-FIELD and the values
VARIABLE M-A  VARIABLE M-U
S" { 'id' : 7 , 'times':[9000,4500], 'ir':{'sent':3}, 'name':'rty3', 'id':8 }" Q  M-U ! M-A !
: M ( -- a u )   M-A @ M-U @ ;
T{ M S" id" JSON-FIELD NIP NIP -> TRUE }T
T{ M S" id" JSON-FIELD DROP JSON>NUMBER -> 7 0 }T
T{ M S" ID" JSON-FIELD M S" nothing" JSON-FIELD -> FALSE FALSE }T
T{ M S" i" JSON-FIELD M S" idx" JSON-FIELD -> FALSE FALSE }T   \ a name is the whole name
T{ M S" times" JSON-FIELD DROP S" [9000,4500]" STR= -> TRUE }T
T{ M S" ir" JSON-FIELD DROP S" sent" JSON-FIELD DROP JSON>NUMBER -> 3 0 }T
T{ S" [1]" Q S" a" JSON-FIELD -> FALSE }T
T{ S" {'a':1" Q S" b" JSON-FIELD -> FALSE }T
T{ S" {}" Q S" a" JSON-FIELD -> FALSE }T
: N? ( a u -- n ior )   Q JSON>NUMBER ;
T{ S" 7" N? S"  -61 " N? S" 0" N? S" -0" N? -> 7 0 -61 0 0 0 0 0 }T
T{ S" 1.5" N? S" 1e3" N? S" 'a'" N? -> 0 JSON-LIMIT 0 JSON-LIMIT 0 JSON-TYPE }T
T{ S" true" N? S" [1]" N? S" 1x" N? -> 0 JSON-TYPE 0 JSON-TYPE 0 JSON-INVALID }T
: TOO-BIG ( -- n ior )   BIG N>S Q  2DUP + 1- DUP C@ 1+ SWAP C!  JSON>NUMBER ;   \ BIG ends with 7
T{ BIG N>S Q JSON>NUMBER TOO-BIG -> BIG 0 0 JSON-LIMIT }T
T{ W-BIG DROP S" max" JSON-FIELD DROP JSON>NUMBER -> BIG 0 }T
T{ W-BIG DROP S" min" JSON-FIELD DROP JSON>NUMBER -> BIG NEGATE 0 }T
: F? ( a u -- f ior )   Q JSON>FLAG ;
T{ S" true" F? S" false" F? S"  true  " F? -> TRUE 0 FALSE 0 TRUE 0 }T
T{ S" null" F? S" 1" F? S" truex" F? -> 0 JSON-TYPE 0 JSON-TYPE 0 JSON-INVALID }T
CREATE TEXT-OUT 40 ALLOT
: T? ( a u m -- b u ior )   >R Q TEXT-OUT R> JSON>TEXT ;
T{ S" 'abc'" 40 T? S" abc" IS? -> TRUE 0 }T
CREATE PLAIN  'a' C, 34 C, 'b' C, 92 C, 'c' C, 10 C, '/' C, 'A' C, 9 C,
T{ S" 'a\'b\\c\n\/\u0041\t'" 40 T? PLAIN 9 IS? -> TRUE 0 }T
T{ S" '\u00e9'" 40 T? NIP NIP S" 'abcdef'" 3 T? NIP NIP -> JSON-LIMIT JSON-LIMIT }T
T{ S" 7" 40 T? NIP NIP S" 'abc" 40 T? NIP NIP -> JSON-TYPE JSON-INVALID }T
T{ S" 'aé'" 40 T? ROT DROP -> 3 0 }T
CREATE CELLS-OUT 8 CELLS ALLOT
: NS? ( a u m -- b n ior )   >R Q CELLS-OUT R> JSON>NUMBERS ;
T{ S" [9000,4500,560]" 8 NS? ROT DROP -> 3 0 }T
T{ CELLS-OUT @ CELLS-OUT CELL+ @ CELLS-OUT 2 CELLS + @ -> 9000 4500 560 }T
T{ S" [ ]" 8 NS? ROT DROP S" [1,2,3]" 2 NS? ROT DROP -> 0 0 0 JSON-LIMIT }T
T{ S" [1,'a']" 8 NS? ROT DROP S" [1.5]" 8 NS? ROT DROP -> 0 JSON-TYPE 0 JSON-LIMIT }T
T{ S" 7" 8 NS? ROT DROP S" [1,]" 8 NS? ROT DROP -> 0 JSON-TYPE 0 JSON-INVALID }T
CREATE BYTES-OUT 8 ALLOT
: HX? ( a u m -- b n ior )   >R Q BYTES-OUT R> JSON>HEX ;
T{ S" '6038ff00'" 8 HX? ROT DROP -> 4 0 }T
T{ BYTES-OUT 4 BYTES4 4 STR= -> TRUE }T
T{ S" '603'" 8 HX? ROT DROP S" '60x8'" 8 HX? ROT DROP -> 0 JSON-TYPE 0 JSON-TYPE }T
T{ S" '6038'" 1 HX? ROT DROP S" 6038" 8 HX? ROT DROP -> 0 JSON-LIMIT 0 JSON-TYPE }T

\ ---- a message of the writer, read back
: EVENT ( -- a u )   W-EVENT DROP ;
T{ EVENT JSON-CHECK -> 0 }T
T{ EVENT S" times" JSON-FIELD DROP CELLS-OUT 8 JSON>NUMBERS ROT DROP -> 4 0 }T
T{ CELLS-OUT @ CELLS-OUT 3 CELLS + @ -> 9000 1690 }T
T{ EVENT S" bytes" JSON-FIELD DROP BYTES-OUT 8 JSON>HEX ROT DROP BYTES-OUT 4 BYTES4 4 STR= -> 4 0 TRUE }T
T{ EVENT S" protocol" JSON-FIELD DROP TEXT-OUT 40 JSON>TEXT S" rty3" IS? -> TRUE 0 }T
T{ W-STATUS DROP JSON-CHECK W-KINDS DROP JSON-CHECK W-ESCAPES DROP JSON-CHECK -> 0 0 0 }T

S" json" .RESULTS
