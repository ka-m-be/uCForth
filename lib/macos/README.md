# lib/macos: packages for macOS

| File | Contents |
| --- | --- |
| `macho.fs` | package MACHO: a program file for macOS on Apple silicon, with an ad-hoc code signature, for `targets/macos-a64/build.fs`; with `MACHO-X86-64` the same for a Mac with Intel (`targets/macos-x64/build.fs`); needs IMAGE and SHA256 |
| `system.fs` | package MACOS: `SYSCALL` (HAL 100) and `SYSCALL:`, the calls of the kernel for the network (`SYS-SOCKET` ...) and their numbers; at the end `SYSTEM`. For each Mac: the HAL of `macos-x64` adds the class of the call (`lib/net/README.md`) |

The test of MACOS is the test of the network (`lib/net/test/net.fs`).
The test of MACHO is the program itself: macOS starts it, and
`codesign -v -v build/ucf-macos-a64` says "valid on disk"
(`targets/macos-a64/README.md`, `targets/macos-x64/README.md`).
