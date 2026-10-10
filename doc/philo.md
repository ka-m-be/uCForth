# Philosophy and thoughs - not yet ready for repo

This text attempts to give some answers, why uCForth was build, why this way and not another, the intentions and some philosophy.

First: Don't take this project too seriously, and especially not the *post-apocalypse* approach. I don't think civilization will collapse (here), and if it does, programming microcontrollers will probably be a topic of lesser importance for a long time. This is more like the McGyver thinking - yes, I'm that old: Could you build a rocket when all you have is a rubber band, a pencil and a swiss army knife.

Second: I like to know how things work underneath. While I'm not keen to learn and memorize forever all the bits and registers of every processor since 8080, I really enjoy to take complex things apart and dive deep for a specific purpose.

Third: I like the whole concept of Forth and the way it is built and works. It only needs a very small, machine dependent base, and above, it's all Forth. There's no secret code, no tools doing stuff you can't retrace and comprehend.

The main purpose of this project is not a fancy or productive application, although there were some useful tools built on the way. uCForth is more a living and working library of computing since the days of home computers like C64, up to current SoC microcontrollers like ESP32 or modern Macs or x64 PCs and their operating systems. Everyone can dive into the subdirectories and try out how a boot image for e.g. a Raspberry Pi is made byte for byte (`targets/rpi4`).

**Will this project be maintained?**

Yes and no. It is not a real product. More a bookshelf. There is no roadmap, milestones or relaases. I may occasionally add code that comes from currents interests, like the IR remote for the air condition.

**Why does this look so LLM generated?**

Because much of it is. Mostly Claude Code with Opus 5.5. I provided the ideas, specs and directions, Claude pulled the infos from everywhere (cf. `doc/references.md`) and built the code, the tests, the documentation. Then I checked everything, made corrections and requested optimizations and changes. I've worked professionally for years with manuals, data sheets, register tables, etc, so I probably could have written all this myself - given three or four years of hundred percent of my spare time. Claude Code provides me with astonishingly good Forth code and leaves me all the fun to read it and learn.
