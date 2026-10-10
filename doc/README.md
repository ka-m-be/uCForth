# doc: manuals

| File           | Contents                                                                                                                                                                                            |
| -------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `start.md`     | First steps: from the first prompt to uCForth on a board; all builds and checks                                                                                                                     |
| `philo.md`     | Why was this build, why this way, what will it become
| `forth.md`     | Forth basics for readers new to Forth: the stack, words, the dictionary, vocabularies; how to read source; all words of the vocabulary FORTH; what is special in uCForth; how Forth compiles itself |
| `words.md`     | The reference: all words of the vocabularies FORTH and INTERNALS in one list, with kind, place, stack effect and meaning                                                                            |
| `bootstrap.md` | From nothing to uCForth on a new computer: platform, bootstrap VM, image, target image                                                                                                              |
| `vm.md`        | The virtual machine: the contract for every bootstrap VM and kernel; uCForth next to other Forth systems                                                                                            |
| `genesis.md`   | Genesis: how a bootstrap VM compiles `forth/core.fs`                                                                                                                                                |
| `origins.md`   | Where the concepts of uCForth come from: Forth, Collapse OS, eForth, the tests of John Hayes                                                                                                        |
| `docker.md`    | Tests on other processors with Docker: Linux programs for arm64, x86, RISC-V, POWER, IBM Z                                                                                                          |
| `qemu.md`      | Tests on other computers and operating systems with QEMU: the Raspberry Pi 4, a PC with another system                                                                                              |

Planned: the chip notes of the ESP32-S3.

The manuals use relaxed Simplified Technical English. A reader with the
knowledge of a microcomputer handbook of the 1980s should understand them.
