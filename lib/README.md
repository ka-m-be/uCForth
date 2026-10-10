# lib: packages for every system

A package is a vocabulary in one file. The packages in this folder run
on every system, from the host to the smallest board. The packages for
one platform, or for one topic, have a folder of their own (next table). A test is in the
folder `test/` next to its package, with the same file name. File names
have at most 8 characters and `.fs`, folder names at most 8 characters
(MS-DOS).

| File | Contents |
| --- | --- |
| `ihex.fs` | package IHEX: write Intel HEX records (`IHEX-START`, `IHEX-DATA`, `IHEX-END`), each line to the vector `IHEX-LINE`; read one record with `IHEX>` |
| `vm.fs` | package VM: the virtual machine of doc/vm.md in Forth. It runs a second uCForth (the target) in a window of the dictionary; the target has the cell size of the host. `TRUN` runs a target word, `OP` adds a primitive. |
| `package.fs` | `PACKAGE PUBLIC PRIVATE END-PACKAGE`: the frame of a package (in FORTH; FILES loads it) |
| `sha256.fs` | package SHA256: the hash function SHA-256 (`SHA-START`, `SHA-ADD`, `SHA-END`, `SHA-DIGEST`); 32-bit cells |
| `sha1.fs` | package SHA1: streaming SHA-1 and saved contexts; 32-bit cells, for protocol compatibility |
| `kdf1.fs` | package KDF1: HMAC-SHA1, bounded PBKDF2 and SHA1-PRF; needs SHA1 |
| `aes128.fs` | package AES128: AES-128 blocks and RFC 3394 key unwrap |
| `ccm.fs` | package CCM: authenticated encryption, 13-byte nonce and 8-byte tag; needs AES128 and 32-bit cells |
| `text.fs` | package TEXT: `2SWAP /STRING S= -LEADING -TRAILING SPLIT PLACE +PLACE N>S FAIL` |
| `idle.fs` | package IDLE: words that run while the console waits (`IDLE+`, `IDLE-`, `IDLE-RUN`, `.IDLE`; KEY runs them), `.STOPPED`, and `-BREAK` `+BREAK` `NO-BREAK`: Ctrl-C waits between them (on a board). In the image of the ESP32 boards |
| `slip.fs` | package SLIP: byte frames after RFC 1055 (for the ROM bootloader of the ESP32) |
| `md5.fs` | package MD5: the digest MD5 (RFC 1321); 32-bit cells |
| `font8.fs` | package FONT8: a font of 8 x 8 pixels for the characters 32 to 126 (public domain); `>GLYPH` |
| `fbcon.fs` | package FBCON: text on a framebuffer with one cell for each pixel; `FB-START`, `FB-EMIT`, `FB-PAGE` |
| `test/*.fs` | their tests (`tools/test.fs` runs them; `sha256`, `md5`, `n3d` only with 32-bit cells; the clock of `kelon168` too) |

| Folder | Packages for one platform or one topic |
| --- | --- |
| `dos/` | MS-DOS: the package DOS, the HAL words of `targets/dos` (`SYSCALL`, `REGS`, `DOS`, the ports `PC@ PC! PW@ PW!`); its test runs on DOS only |
| `pc/` | a PC without an operating system: the package BIOS, the HAL words of `targets/pc-bios` (`SYSCALL`, `REGS`, `BIOS`, the ports `PC@ PC! PW@ PW!`); it is in the image of the PC, and its test runs there only. Also `asm.fs`, the package ASM: `CODE` words on a PC, with DOS or without (`CODE`, `END-CODE`, `NEXT,`, `>CODE`; needs TEXT and I8086); its test runs on DOS and on the PC |
| `esp32/` | the ESP32 chips (ESP32-S3, ESP32): the common drivers for the board, the layer and the drivers of each chip in its own folder (`esp32s3/`, `esp32/`), the ROM bootloader and its image format for the host (`lib/esp32/README.md`) |
| `irremote/` | IR remote controls, on every system: IRCODE, the pulse distance code, bytes to times in us and back (`SHAPE`, `>TIMINGS`, `TIMINGS>`); KELON168, the frame of Hisense air conditioners (21 bytes; DG11R2-01, RCH-R0Y3: `AC-POWER!` ... `AC-TIMINGS`, `AC-DECODE`, `.AC`); RTY3, the frame of the remote control RCH-RTY3-0 (23 bytes, measured; the same words and `AC-FLAP!`, `AC-CHECK?`; `apps/aircon/` uses it). Their tests are in `test/` of this folder (`lib/test/`) |
| `linux/` | Linux: the program file (ELF), in the 64-bit form (aarch64) and in the 32-bit form (i386); the calls of the kernel with `SYSCALL` for each processor (LINUX-A64, LINUX-X64, LINUX-I386: the layer SYSTEM of `net/`); a TAP device as the LINK of our TCP/IP (TAP) |
| `macos/` | macOS: the program file (Mach-O) with its signature; the calls of the kernel with `SYSCALL` (MACOS: the layer SYSTEM of `net/`) |
| `net/` | the network: TCP, UDP and DNS with the same words on each system (NET, INET, DNS); under them a layer for the system: POSIX-NET for macOS and Linux (`host.fs` loads all), or our own TCP/IP in Forth (ARP, IPV4, UDP, TCP, DHCP, LINK-NET) over Ethernet frames (the contract LINK: NO-LINK, TAP); `tcpip.fs` loads it (`lib/net/README.md`) |
| `windows/` | Windows: PE, the program file (PE32, the format of each program of Windows) with its imports, for the build on the host; also the 64-bit form PE32+ for UEFI (`EFI-START`, `targets/uefi-x64`); WINDOWS, the functions of the DLLs of Windows for an application (`LIBRARY`, `WINAPI:`, `Z"`, `>CHIP`, `FIND-FUNCTION`, `SYSCALL`; `targets/win32/README.md`); its test runs on Windows only |
| `rpi/` | the Raspberry Pi: the chip BCM2711 (mailbox, framebuffer) |

Load a package with the package FILES. After the load, the package is
in the search order:

    ALSO FILES  S" lib/vm.fs" INCLUDED

A program starts with only FORTH in the search order (doc/vm.md,
section 10), also when packages are in its image: `ALSO name` gives the
words of a package. `ORDER` shows the search order, `VOCS` all
vocabularies (host/README.md).

## Packages: exported words and helpers

A package has two vocabularies. `p` holds the words the package gives
to its users (the *exported* words). `p.I` holds its helpers (I:
internals). After the load, only `p` is in the search order. So a
helper does not hide an older word with the same name, and a user sees
only the words of the interface.

The frame of a package file (`lib/package.fs`; the package FILES
loads it):

    PACKAGE IHEX  ALSO FILES     \ IHEX, IHEX.I; FILES is used
    \ helpers
    PUBLIC
    \ exported words
    PRIVATE
    \ more helpers
    ...
    END-PACKAGE

- `PACKAGE name` makes the vocabularies `name` and `name.I` and puts
  `INTERNALS name name.I` into the search order. New words go to
  `name.I`.
- The name of a package has at most 29 characters: a name has at most
  31, and `.I` takes two of them.
- `PUBLIC` and `PRIVATE` change where the next words go. A file
  changes between the two as often as it must.
- `ALSO` after `PACKAGE` adds the packages that this one uses. They
  come before the own words in the search order: a name there hides a
  helper with the same name.
- `END-PACKAGE` puts the search order and the vocabulary for new words
  back as they were before `PACKAGE`, and adds the package.
- A package file can load another package file: 4 inside each other.
- A test of a helper adds it again: `ALSO IHEX.I`.
- No exported word has the name of the package: it would hide the
  vocabulary, and `ALSO FONT8` would run the word.
- The search order has 16 places.

`HIDE name` (base) takes one word out of its wordlist after its use,
as `PRIVATIZE` in pForth or `HIDE` in SwiftForth. For a package, the
two vocabularies are the better way.

Each package uses this frame; only `host/files.fs` and `lib/package.fs` have it by hand, because they come first.

Coming later from archive/m0-m3/forth: the chip `atmega328p.fs` and the
board `uno.fs` (Arduino Uno), in a folder such as `lib/avr/`.

## Protocol arithmetic (N3d)

These packages are plain Forth. They do not start a radio, read saved
credentials, or write flash. `lib/net/wpakey.fs` puts WPA2-Personal
key derivation over them (see `lib/net/README.md`).

Load SHA1 before KDF1, BYTES and KDF1 before WPAKEY, and AES128 before CCM. Each package has one
working state: use it from one foreground caller, not from an interrupt
or timer handler. SHA-1 is here for the existing protocol, not for new
signatures. AES uses byte lookup tables; these words are not a
constant-time, side-channel-hardened cryptographic library.

### SHA1

| Word | Effect |
| --- | --- |
| `SHA1-START ( -- )` | start a digest |
| `SHA1-ADD ( a n -- )` | add bytes, including zero bytes; n must not be negative |
| `SHA1-END ( -- a )` | the 20 digest bytes, high byte first |
| `SHA1-DIGEST ( a n -- digest )` | digest of one byte string |
| `SHA1-CONTEXT ( -- n )` | 96 bytes for a saved working state |
| `SHA1-SAVE ( a -- )`, `SHA1-RESTORE ( a -- )` | copy the working state to/from a caller buffer |
| `SHA1-CLEAR ( -- )` | erase the working state, schedule and digest |

The byte count has two 32-bit words. A digest is in a shared buffer;
copy it before the next SHA1 or KDF1 operation. A saved context contains
message bytes too: erase it when no longer needed.

### KDF1

| Word | Effect |
| --- | --- |
| `HMAC1-START ( key kn -- )` | prepare the key and start an HMAC |
| `HMAC1-ADD ( a n -- )` | add message bytes |
| `HMAC1-END ( -- digest )` | finish; 20 bytes in the SHA1 digest buffer |
| `HMAC1-DIGEST ( key kn a n -- digest )` | one HMAC |
| `HMAC1-CLEAR ( -- )` | erase HMAC key contexts and the SHA1 state |
| `PBKDF2-START ( key kn salt sn count out n -- )` | start derivation into a caller buffer; sn 0..64, count positive, n 1..65535 |
| `PBKDF2-STEP ( budget -- done )` | do at most min(budget, 8) HMAC rounds; budget must be positive; true when complete |
| `PBKDF2-CANCEL ( -- )` | erase an incomplete output and private state; return to idle |
| `PBKDF2 ( key kn salt sn count out n -- )` | blocking form; use STEP on a board |
| `SHA1-PRF ( key kn label ln data dn out n -- )` | HMAC over label, zero byte, data, one-byte counter starting at 0; n 1..5120 |

PBKDF2 copies the salt and prepared key at START. They need not remain
in the input buffers. Its private contexts allow another SHA1/HMAC
operation between STEP calls. A second START while it is working is an
error; STEP before START is an error. STEP after completion returns true.
Completion erases private key, salt and round buffers. The complete
output belongs to the caller and is not erased by CANCEL.

Use disjoint input/output buffers for PRF. Output buffers must have the
specified size. Invalid lengths and state errors use `ABORT"` with a
message. Erase caller keys, digests and complete outputs after use;
clearing a package cannot erase copies kept by the caller.

### AES128 and CCM

| Word | Effect |
| --- | --- |
| `AES128-KEY ( a -- )` | prepare a 16-byte key |
| `AES128-ENCRYPT ( in out -- )`, `AES128-DECRYPT ( in out -- )` | one 16-byte block; in/out may be the same buffer |
| `AES128-UNWRAP ( in n out -- n' )` | RFC 3394; n a multiple of 8, 24..2056; out needs n-8 bytes; in-place is allowed |
| `AES128-CLEAR ( -- )` | erase the key schedule and working block |
| `CCM-ENCRYPT ( nonce aad an payload pn tag -- )` | change payload in place and write 8 tag bytes |
| `CCM-DECRYPT ( nonce aad an payload pn tag -- )` | change payload in place, verify all 8 tag bytes |

CCM uses the current AES128 key, exactly 13 nonce bytes and an 8-byte
tag. pn is 0..65535, an is 0..65279 (the short AAD length form of
RFC 3610). Empty payload/AAD work. Nonce, AAD, payload and tag must not
overlap. A sender must never reuse a nonce with the same key. CCM does
not manage packet numbers or reject replays: the protocol must do that.

An unwrap integrity failure erases out and throws **-410**. A CCM tag
failure erases payload and throws **-411**, never returning unverified
plaintext as a successful result. Input errors have explicit messages.
CCM clears its private working buffers after success or a tag failure;
the AES key stays until AES128-CLEAR.

### Tests and board memory

In `ucf`, with the C bootstrap VM:

    build/ucf-c -32 host/files.fs lib/test/n3d.fs </dev/null

Expected: sha1 **6/0**, kdf1 **17/0**, aes128 **9/0**, ccm **14/0**,
wpakey **5/0** (51 tests). The same vectors pass on the JavaScript VM
and native macOS. `tools/test.fs` includes them; the N3d checkpoint was
**11030/0** (32 bit), before the additional N3e tests,
and the unchanged **2742/0** (16 bit).
The vectors come from FIPS 180-4/197, RFC 2202/6070/3394/3610 and the
local Node crypto oracle. Every input is public or synthetic.

On slow hosts, define `KDF-SHORT` before the loader: only the four long
4096-round derivations are then left out, giving **47/0** on macOS awk.
The individual vector files need their packages, BYTES and the tester
loaded first. They have no file operations and run unchanged on a board.

WROOM: `lib/esp32/esp32/test/n3d.fs` loads into RAM, runs the long cases
in short lines with a 10-ms timer, resets and closes term. Check that
the port is free with `lsof` before running it. **52/0** on 09 October:
max STEP **49 ms** (8 rounds, requested/checked below 100 ms);
14884 timer calls in 148850 ms, within 50 ms of the expected count.
These are measurements, not a timing guarantee for all chips.
STEP is a foreground operation, not a sub-millisecond NETIF handler.

| Package | Dictionary bytes | RAM data bytes |
| --- | ---: | ---: |
| SHA1 | 2452 | 224 |
| KDF1 | 3740 | 840 |
| AES128 | 3828 | 528 |
| CCM | 1956 | 112 |
| WPAKEY | 804 | 108 |
| Total, without the existing BYTES | 12780 | 1812 |

After these packages plus BYTES and small measurement words, before
the test data: **82908/60876** dictionary/RAM bytes free. No PHY or
radio was loaded. Flash stayed unchanged.
