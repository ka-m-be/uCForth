# Docker: tests on other processors

Docker runs Linux programs in containers. On a Mac, Docker Desktop has
its own Linux in a virtual machine, and each container uses the kernel
of this Linux. With the option `--platform`, a container holds the
programs of another processor, and Docker runs them with an emulator.
So one Mac tests uCForth for Linux on many processors.

Docker runs only Linux programs. For a kernel without an operating
system (a Raspberry Pi, a board) or for another operating system, see
`doc/qemu.md`.

All examples here run in the folder `ucf`.

## Start Docker

    open -a Docker
    docker info --format '{{.OperatingSystem}} {{.Architecture}}'

After some seconds the output is:

    Docker Desktop aarch64

`aarch64` is the processor of the Linux of Docker Desktop: 64-bit ARM
on a Mac with Apple silicon.

## The processors

Docker Desktop lists the platforms that it can run:

    docker buildx inspect desktop-linux | grep Platforms

Output:

    Platforms:        linux/arm64, linux/amd64, linux/amd64/v2, linux/riscv64, linux/ppc64le, linux/s390x, linux/386, linux/arm/v7, linux/arm/v6

A test with Alpine, a small Linux distribution. `uname -m` gives the
processor as Linux sees it:

    for p in linux/arm64 linux/amd64 linux/386 linux/arm/v7 linux/riscv64 linux/ppc64le linux/s390x; do
      printf '%-14s ' $p; docker run --rm --platform $p alpine:3.22 uname -m
    done

Output (the messages of the first download not shown):

    linux/arm64    aarch64
    linux/amd64    x86_64
    linux/386      i686
    linux/arm/v7   armv7l
    linux/riscv64  riscv64
    linux/ppc64le  ppc64le
    linux/s390x    s390x

| Platform | Processor |
| --- | --- |
| `linux/arm64` | 64-bit ARM; on Apple silicon without an emulator, at full speed |
| `linux/amd64` | 64-bit x86 (PC) |
| `linux/386` | 32-bit x86 |
| `linux/arm/v7` | 32-bit ARM (for example Raspberry Pi OS with 32 bits) |
| `linux/riscv64` | 64-bit RISC-V |
| `linux/ppc64le` | 64-bit POWER, little endian |
| `linux/s390x` | IBM Z, big endian |

The first run on a platform loads the Alpine image for it (about 4 MB).
Docker keeps one platform for each image name: after a change of the
platform, it loads the image again.

## The folder `ucf` in a container

`-v "$PWD":/ucf:ro` shows the current folder of the Mac in the
container as `/ucf`, read only. `-w /ucf` makes it the current folder
in the container.

Example: the awk bootstrap VM (`bootstrap/awk/`) with the awk of
BusyBox (in Alpine), on an emulated IBM Z:

    docker run --rm --platform linux/s390x -v "$PWD":/ucf:ro -w /ucf alpine:3.22 \
      sh -c 'LC_ALL=C awk -f bootstrap/awk/ucf.awk -- forth/test/tester.fs forth/test/t0.fs </dev/null | tail -2'

Output:

    t0: 183 tests, 0 errors
    ok> 

The same test takes different times (Mac M4):

| Where | Time |
| --- | --- |
| macOS, the awk of the Mac | 2.6 s |
| `linux/arm64` (no emulator, awk of BusyBox) | 7 s |
| `linux/amd64` | 12 s |
| `linux/riscv64` | 88 s |
| `linux/s390x` | 86 s |

An emulated processor is slow. For a long test, take `linux/arm64`
or `linux/amd64`, and the other processors for a short test.

## A uCForth program for Linux

`build/ucf-linux-a64` (`targets/linux-a64/`) needs no library, so it
runs in each Linux container for arm64. All tests:

    docker run --rm --platform linux/arm64 -v "$PWD":/ucf:ro debian:bookworm-slim \
      sh -c 'cp -r /ucf /tmp/u && cd /tmp/u && chmod +x build/ucf-linux-a64 &&
             echo "ALSO FILES  INCLUDE tools/test.fs" | build/ucf-linux-a64'

The output ends with:

    all: 12186 tests, 0 errors

The tests write files, and the folder of the Mac is read only in the
container. So the command first copies the folder to `/tmp/u`. With
`alpine:3.22` instead of `debian:bookworm-slim`, the result is the same.

`build/ucf-linux-i386` (`targets/linux-i386/`), uCForth for Linux on a
PC with 32 bits, runs in the same way with `--platform linux/386` (in
QEMU, about 2 s):

    docker run --rm --platform linux/386 -v "$PWD":/u -w /u debian:bookworm-slim \
      sh -c 'echo "ALSO FILES  INCLUDE tools/test.fs" | ./build/ucf-linux-i386'

A serial line in a container: `socat` makes two connected pseudo
terminals (an image with it: `printf 'FROM debian:bookworm-slim\nRUN
apt-get update && apt-get install -y --no-install-recommends socat\n' |
docker build --platform linux/386 -t ucf-i386 -`; the same with
`linux/arm64`, and `linux/amd64` as `ucf-x64`).

`build/ucf-linux-x64` (`targets/linux-x64/`), uCForth for Linux on a PC
with 64 bits, runs with `--platform linux/amd64` (with Rosetta, less
than a second):

    docker run --rm --platform linux/amd64 -v "$PWD":/u -w /u debian:trixie-slim \
      sh -c 'echo "ALSO FILES  INCLUDE tools/test.fs" | ./build/ucf-linux-x64'

## An image with more programs

A container starts from an image. Docker Hub has images for many Linux
distributions (`debian`, `ubuntu`, `alpine` ...). An image with more
programs comes from a Dockerfile: its text can come from `printf`, so
the project needs no file for it. Example: QEMU for all processors
(`doc/qemu.md`):

    printf 'FROM debian:trixie-slim\nRUN apt-get update && apt-get install -y --no-install-recommends qemu-system-arm qemu-system-x86 qemu-system-misc\n' |
      docker build -t ucf-qemu -

Another example: Wine for programs for Windows with 32 bits
(`targets/win32/README.md`, section "Test"). It needs the platform
`linux/386`: Docker Desktop runs `linux/amd64` with Rosetta, and Rosetta
runs no 32-bit x86 code; `linux/386` runs in QEMU.

    printf 'FROM debian:trixie-slim\nRUN apt-get update && apt-get install -y --no-install-recommends wine wine32 socat\n' |
      docker build --platform linux/386 -t ucf-wine386 -

## Limits

- Docker Desktop on the Mac gives no USB device to a container. A test
  with a serial line (a board) needs the uCForth program for the Mac
  (`targets/macos-a64/`, `build/ucf`).
- Each container has the kernel of Docker Desktop (Linux for arm64),
  also an emulated one. A test in Docker does not test the kernel of a
  distribution, only the program.
- Images need space. `docker system df` shows how much;
  `docker image rm alpine:3.22` removes an image.
