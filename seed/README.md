# seed: reference images

A reference image is the memory of the virtual machine after a bootstrap
VM compiled `forth/core.fs` (core) or `forth/core.fs` and `forth/base.fs`
(base). Each bootstrap VM must build exactly these images, byte for byte.

| File | Contents |
| --- | --- |
| `core16.hex`, `core32.hex` | the image after core.fs, 16 or 32 bit cells: the *seed* |
| `base16.hex`, `base32.hex` | the image after base.fs |

The format is Intel HEX with one comment line first (`doc/vm.md`,
section 10): text, so each computer can read, send and store it.

## What the images are for

- **The test of a VM.** A bootstrap VM compiles `core.fs` and `base.fs`
  and compares its memory with these files (option `-check`). One wrong
  primitive or one wrong step of Genesis gives other bytes. So "the
  same" tells that the VM keeps the contract of `doc/vm.md`.
- **A start without Genesis.** A small bootstrap VM (MS-DOS, later a
  C64) cannot read `core.fs`: it has no Genesis. It has a loader for
  Intel HEX. It loads the seed `core16.hex`, and the seed then compiles
  `base.fs` and each other file itself. `bootstrap/dos/` does this; its
  own `seed.hex` is the same seed built with more primitives, for speed.
- **A start without a build.** Each VM starts each image at once:
  `-image seed/base16.hex` gives the prompt, and no VM has to compile
  `core.fs` and `base.fs` first.
- **The proof of the circle.** uCForth builds the same images again
  with the tool `tools/genesis.fs`. So the images do not depend on the
  bootstrap VM that built them (the fixed point, below).

## Why this works

- An image holds no machine code and no address of the real computer.
  A word in it is a list of addresses of the VM, and a code field holds
  the number of a primitive (`doc/vm.md`, section 6). So the image does
  not depend on the processor, and not on the language of the VM.
- All VMs have the same cell size, the same byte order and the same VM
  addresses (the memory map of `doc/vm.md`, section 3). So each address
  in the image is right in each VM.
- `doc/genesis.md` fixes each step of Genesis: the order of the first
  headers, the constants, the lines of `core.fs`. So each VM writes the
  same bytes.
- The images use only the 32 mandatory primitives and the HAL (option
  `-pure` of a bootstrap VM). Each VM has them, so each VM can run the
  images.

**The memory of a VM.** A VM is free in the place where it keeps the
memory of the virtual machine: an array in bash, a block of `calloc` in
C, a segment of a PC, the RAM of a chip from a base address on. The
image does not see this place.

The VM addresses (the memory map) are not free: the image holds them
(the constants `S0`, `TIB` and `DICT-END`, each link, each xt). A VM
with another memory map builds other bytes, and it cannot load these
images. So each VM has the standard map of `doc/vm.md`, section 3.
A VM with another map must document it, and it needs reference images
of its own.

## Check and build

Check a bootstrap VM against these images:

    bash bootstrap/bash/ucf.sh -check
    bash bootstrap/bash/ucf.sh -32 -check

uCForth itself builds the same images with the tool `tools/genesis.fs`
(tools/README.md). This is the fixed point: the images do not depend
on the bootstrap VM that built them.

    LC_ALL=C awk -f bootstrap/awk/ucf.awk -- host/files.fs tools/genesis.fs

Make them again after a change of core.fs or base.fs (all bootstrap VMs
must then agree). The C bootstrap VM writes the same files in 0.1 s
(`build/ucf-c` instead of `bash bootstrap/bash/ucf.sh`); the bash
bootstrap VM takes 2.5 min for each:

    bash bootstrap/bash/ucf.sh -pure -dump-core seed/core16.hex -dump seed/base16.hex < /dev/null
    bash bootstrap/bash/ucf.sh -32 -pure -dump-core seed/core32.hex -dump seed/base32.hex < /dev/null
