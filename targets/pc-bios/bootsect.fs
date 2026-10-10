\ bootsect.fs - the boot sector of uCForth for a PC with a BIOS
\
\ The BIOS loads the first sector of the disk (512 bytes) to 0000:7C00
\ and runs it; DL holds the number of the disk (0: floppy disk A:, $80:
\ the first hard disk; a USB stick is one of them). This sector loads
\ the code (CODE-SECTORS sectors from sector 1) to CODESEG:0000 and the
\ memory of the VM (128 sectors from VM-LBA) to VMSEG:0000, then jumps
\ to the code: DL the disk, CX its sectors per track, BX its heads, AX
\ 1 if the BIOS reads the disk by the number of the sector (LBA).
\
\ The sector is also the boot sector of a FAT12 file system: the disk is
\ a normal floppy disk of 1.44 MB, and uCForth is in its reserved
\ sectors, before the first FAT. DOS, Windows, macOS and Linux see an
\ empty disk with the name UCFORTH. The BIOS parameter block (BPB) from
\ byte 11 on describes the disk.
\
\ Two ways to read: a floppy disk by cylinder, head and sector (CHS),
\ with the form of the BPB. A hard disk (and a USB stick that the BIOS
\ shows as one) by the number of the sector, when the BIOS has the
\ extensions of INT 13h (since about 1998: functions 41h and 42h); else
\ by CHS, with the form that the BIOS tells (function 08h).
\
\ Loaded by build.fs after $7C00 ORG, with the constants CODE-SECTORS,
\ VM-LBA, RESERVED, CODESEG and VMSEG. Only instructions of the 8086.
\ Each jump inside the sector is relative, so the BIOS may start it at
\ 0000:7C00 or at 07C0:0000. The variables lie at 0000:0600, in memory
\ that is free at the start.

$0600 CONSTANT B-LBA          \ the next sector to read, from 0
$0602 CONSTANT B-LEFT         \ the sectors to go
$0604 CONSTANT B-N            \ the sectors of this call
$0606 CONSTANT B-AX           \ AX for this call: the function, the sectors
$0608 CONSTANT B-EXT          \ 1: read by LBA (function 42h)
$0610 CONSTANT B-DAP          \ the block for function 42h: 16 bytes
FORWARD B-READ   FORWARD B-ERROR   FORWARD B-MSG

$EB TC, $3C TC, $90 TC,       \ JMP SHORT over the BPB to $7C3E (B-START), NOP
S" UCFORTH " TS,              \ the system that made the disk: 8 characters
\ The BPB of a floppy disk with 1.44 MB:
512 T,                        \ bytes per sector
1 TC,                         \ sectors per cluster
RESERVED T,                   \ reserved sectors: this one, the code, the VM
2 TC,                         \ FATs
224 T,                        \ entries of the root folder
2880 T,                       \ sectors: 80 cylinders, 2 heads, 18 sectors
$F0 TC,                       \ the media: 3.5 inch, 1.44 MB
9 T,                          \ sectors per FAT
LABEL B-SPT     18 T,         \ sectors per track
LABEL B-HEADS   2 T,          \ heads
0 T, 0 T,                     \ hidden sectors
0 T, 0 T,                     \ sectors with 32 bits: 0, the number above counts
LABEL B-DRIVE   0 TC,         \ the disk (the code puts DL here)
0 TC,
$29 TC,                       \ three more fields follow:
$4655 T, $4355 T,             \ the serial number of the volume
S" UCFORTH    " TS,           \ its name: 11 characters
S" FAT12   " TS,              \ the file system: 8 characters

LABEL B-START    \ at $7C3E
  CLI,  AX AX XOR,  DS AX MOV,  ES AX MOV,  SS AX MOV,  SP $7C00 MOVI,  STI,  CLD,
  B-DRIVE [] DL MOV,  B-EXT [] AX MOV,
  DL $80 TESTI,  NZ IF,                                   \ a hard disk:
    AH 8 MOVI,  $13 INT,  NC IF,                          \ the form that the BIOS tells
      CL $3F ANDI,  CH CH XOR,  B-SPT [] CX MOV,          \ CL bits 0-5: the sectors per track
      DL DH MOV,  DH DH XOR,  DX INC,  B-HEADS [] DX MOV, \ DH: the last head
    THEN,
    AH $41 MOVI,  BX $55AA MOVI,  DL B-DRIVE [] MOV,  $13 INT,   \ the extensions?
    NC IF,  BX $AA55 CMPI,  Z IF,  CL 1 TESTI,  NZ IF,  B-EXT [] 1 MOVI,  THEN,  THEN,  THEN,
  THEN,
  AX CODESEG MOVI,  ES AX MOV,  BX BX XOR,
  B-LBA [] 1 MOVI,  B-LEFT [] CODE-SECTORS MOVI,  B-READ CALL,   \ the code
  AX VMSEG MOVI,  ES AX MOV,  BX BX XOR,
  B-LBA [] VM-LBA MOVI,  B-LEFT [] 128 MOVI,  B-READ CALL,       \ the memory of the VM
  DL B-DRIVE [] MOV,  CX B-SPT [] MOV,  BX B-HEADS [] MOV,  AX B-EXT [] MOV,
  SI CODESEG MOVI,  SI PUSH,  SI SI XOR,  SI PUSH,  RETF,        \ JMP FAR CODESEG:0000

LABEL B-READ     \ B-LEFT sectors from the sector B-LBA (from 0) to ES:BX; BX goes on
LABEL L1
  B-LEFT [] 0 CMPI,  Z IF,  RET,  THEN,
  B-EXT [] 0 CMPI,  Z IF,                                 \ CHS: at most up to the end of a track
    AX B-LBA [] MOV,  DX DX XOR,  B-SPT [] DIV,           \ AX: the track, DX: the sector in it
    CX B-SPT [] MOV,  CX DX SUB,                          \ CX: the sectors up to the end of the track
    CX B-LEFT [] CMP,  A IF,  CX B-LEFT [] MOV,  THEN,
    B-N [] CX MOV,  CH 2 MOVI,  B-AX [] CX MOV,           \ AH 2: read, AL sectors
    DX INC,  CX DX MOV,                                   \ CL: the sector, from 1
    DX DX XOR,  B-HEADS [] DIV,                           \ AX: the cylinder, DL: the head
    CH AL MOV,  AH 1 ROR,  AH 1 ROR,  CL AH OR,           \ bits 8 and 9 of the cylinder: CL bits 6 and 7
    DH DL MOV,
  ELSE,                                                   \ LBA: at most 64 sectors
    AX 64 MOVI,  AX B-LEFT [] CMP,  A IF,  AX B-LEFT [] MOV,  THEN,
    B-N [] AX MOV,  B-AX [] $4200 MOVI,
  THEN,
  DL B-DRIVE [] MOV,
  DI 3 MOVI,                                              \ three tries
LABEL L2
  B-DAP [] $10 MOVI,  AX B-N [] MOV,  B-DAP 2 + [] AX MOV,   \ the block: its size, the sectors,
  B-DAP 4 + [] BX MOV,  B-DAP 6 + [] ES MOV,              \ the address,
  AX B-LBA [] MOV,  B-DAP 8 + [] AX MOV,                  \ the sector (64 bits)
  AX AX XOR,  B-DAP 10 + [] AX MOV,  B-DAP 12 + [] AX MOV,  B-DAP 14 + [] AX MOV,
  SI B-DAP MOVI,  AX B-AX [] MOV,  $13 INT,  L3 JNC,
  AX AX XOR,  $13 INT,                                    \ reset the disk, then again
  DI DEC,  L2 JNZ,
  B-ERROR JMP,
LABEL L3
  AX B-N [] MOV,  B-LBA [] AX ADD,  B-LEFT [] AX SUB,
  CL 9 MOVI,  AX CL SHL,  BX AX ADD,                      \ 512 bytes for each sector
  L1 JMP,

LABEL B-ERROR    \ the disk cannot be read: a message, a key, then the BIOS goes on
  SI B-MSG MOVI,
  BEGIN,  LODSB,  AL AL OR,  NZ WHILE,  AH $0E MOVI,  BX 7 MOVI,  $10 INT,  REPEAT,
  AH AH XOR,  $16 INT,
  $19 INT,
LABEL B-MSG      S" uCForth: disk error" TS,  13 TC,  10 TC,  0 TC,
