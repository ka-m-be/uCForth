\ build.fs - build uCForth for a web browser: build/ucf-web.html
\
\ Use (in the folder ucf), with a bootstrap VM with 16-bit or 32-bit
\ cells, for example:
\   build/ucf-c -32 host/files.fs [APP ...] targets/browser/build.fs </dev/null
\
\ 1. FREEZE: the running system is the image: base, the package FILES
\    and the application files before build.fs. It goes to ucf.hex.
\ 2. The tool writes build/ucf-web.html: the lines of the page of the
\    JavaScript bootstrap VM (bootstrap/js/ucf.html), with two things
\    more in it:
\    - after the line <script type=... id="image">: the lines of
\      ucf.hex, the image as text;
\    - in place of the line <script src="ucf.js"></script>: the lines of
\      bootstrap/js/ucf.js, the VM, in the page.
\    A page with an image starts from it at once (the README).
\ Each line of the page ends with LF, also where a file has CR LF.

FREEZE

ONLY FORTH DEFINITIONS  ALSO FILES  S" lib/ihex.fs" INCLUDED
ONLY FORTH  ALSO FILES  S" host/image.fs" INCLUDED
ONLY FORTH  ALSO FILES  ALSO IMAGE  ALSO INTERNALS
VOCABULARY BROWSER-BUILD  ALSO BROWSER-BUILD DEFINITIONS

CREATE LBUF 256 ALLOT                      \ a line of a file
CREATE LF 10 C,
VARIABLE PAGE-FID   VARIABLE #IMAGE   VARIABLE #VM

\ The size of the page: a number of two cells (16 bits are too few).
VARIABLE BYTES-LO   VARIABLE BYTES-HI
: COUNT-BYTES ( a u -- )   NIP  BYTES-LO @ UM+  BYTES-HI +!  BYTES-LO ! ;
' COUNT-BYTES IS ON-PUT

: LINE ( fid -- a u f )   \ the next line of the file fid; f false at the end
  >R  LBUF 256 R> READ-LINE THROW
  OVER 256 = ABORT" a line has more than 255 characters"
  LBUF ROT ROT ;
: PUT-LINE ( a u -- )   PUT  LF 1 PUT ;
: COPY ( a u -- n )   \ the lines of the file a u into the page; n: their number
  R/O OPEN-FILE THROW  0 >R
  BEGIN  DUP LINE WHILE  PUT-LINE  R> 1+ >R  REPEAT  2DROP
  CLOSE-FILE THROW  R> ;
: STARTS? ( a u a2 u2 -- f )   \ the text a u starts with the text a2 u2
  ROT OVER < IF  2DROP DROP FALSE EXIT  THEN  SAME? ;

: WRITE-PAGE ( -- )
  0 #IMAGE !  0 #VM !  0 BYTES-LO !  0 BYTES-HI !
  S" build/ucf-web.html" WRITE-START
  S" bootstrap/js/ucf.html" R/O OPEN-FILE THROW PAGE-FID !
  BEGIN  PAGE-FID @ LINE  WHILE
    2DUP S" <script src=" STARTS? IF          \ the VM goes into the page
      2DROP  S" <script>" PUT-LINE
      S" bootstrap/js/ucf.js" COPY #VM !
      S" </script>" PUT-LINE
    ELSE
      2DUP PUT-LINE                           \ the image goes into its element
      S" <script type=" STARTS? IF  S" ucf.hex" COPY #IMAGE !  THEN
    THEN
  REPEAT  2DROP
  PAGE-FID @ CLOSE-FILE THROW  WRITE-END
  #IMAGE @ 0= ABORT" bootstrap/js/ucf.html has no element for the image"
  #VM @ 0= ABORT" bootstrap/js/ucf.html does not load ucf.js" ;

: BUILD ( -- )
  WRITE-PAGE
  ."   written: build/ucf-web.html (" BYTES-LO @ BYTES-HI @ <# #S #> TYPE ."  bytes, "
  CELL 8 * 0 .R ." -bit cells)" CR ;

BUILD
