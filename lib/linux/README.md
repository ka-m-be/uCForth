# lib/linux: packages for Linux

| File | Contents |
| --- | --- |
| `elf.fs` | package ELF: the headers of an ELF program file (64-bit form), for `targets/linux-a64/build.fs` |
| `a64.fs`, `x64.fs`, `i386.fs` | packages LINUX-A64, LINUX-X64, LINUX-I386: `SYSCALL` (HAL 100) and `SYSCALL:`, the calls of the kernel for the network (`SYS-SOCKET` ...) and their numbers on aarch64, x86-64 and i386; at the end `SYSTEM` (`lib/net/README.md`) |
| `tap.fs` | package TAP: the contract LINK of our TCP/IP over a TAP device of the kernel (`TAP-OPEN`, `TAP-HOST`, `TAP-LOSE`); the tests of our TCP/IP against the kernel (`lib/net/test/tap.fs`, `lib/net/README.md`, "Test") |
| `test/elf.fs` | the test of ELF (32-bit cells; `tools/test.fs` runs it); the test of the three packages for the calls is the test of the network (`lib/net/test/net.fs`) |
