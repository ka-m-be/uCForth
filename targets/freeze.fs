\ freeze.fs - FREEZE for a target without files
\
\ A board has no files. So its image is the system before the package
\ FILES: put this file before host/files.fs on the command line, for
\ example
\   ... -32 [APP ...] targets/freeze.fs host/files.fs targets/rpi4/build.fs
\ FREEZE saves the image to ucf.hex; the build reads it from there.

FREEZE
