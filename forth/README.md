# forth: the portable system

| File | Contents |
| --- | --- |
| `core.fs` | Part 1 of the base vocabulary: the interpreter and the compiler. A bootstrap VM compiles it once with its Genesis (doc/genesis.md). |
| `base.fs` | Part 2: the rest of the base vocabulary. The interpreter `QUIT` compiles it. |
| `test/tester.fs` | The tester: `T{ ... -> ... }T` after John Hayes. |
| `test/t0.fs` | The conformance test: each virtual machine must pass it. |
| `test/hayes.fs` | The core tests of John Hayes, adapted for uCForth. |

These files are the same on every computer. They do not change for one
platform. Words that a machine can do faster are marked with `?:`: the
machine then supplies them as primitives (doc/vm.md, section 7).

Run all tests with the bash bootstrap VM (in the folder `ucf`, 7 to 9 minutes;
the awk bootstrap VM takes 13 s, the C bootstrap VM 0.07 s,
doc/start.md):

    bash bootstrap/bash/ucf.sh host/files.fs tools/test.fs </dev/null

Expected: `t0: 183 tests, 0 errors`, `hayes: 872 tests, 0 errors`, and at
the end `all: 1474 tests, 0 errors`.
