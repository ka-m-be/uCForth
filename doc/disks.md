# Disks: uCForth on a USB stick, a floppy disk or an SD card

Three targets start from a disk:

| Target | What goes on the disk | How |
| --- | --- | --- |
| a PC with a BIOS (`targets/pc-bios`) | the file `build/ucf-pc.img`, from the first sector on: the disk then has the boot sector, uCForth and an empty FAT12 file system | write the image (section 2) |
| the Raspberry Pi 4 and 400 (`targets/rpi4`) | files on a FAT32 file system: the firmware of the Pi, `config.txt`, `build/kernel8.img` | format the card, then copy the files (section 4) |
| a PC or a Mac with UEFI (`targets/uefi-x64`) | files on a FAT32 file system: `build/ucf-x64.efi` as `EFI/BOOT/BOOTX64.EFI`, and the folders of `ucf` for `INCLUDE` | format the stick, then copy the files (section 5) |

A USB stick, an SD card and a hard disk take the same steps; a floppy
disk too, in a USB floppy disk drive.

## 1 Find the disk

Writing an image removes all data on the disk. With the wrong name, it
removes another disk, perhaps the disk of your system. So first find
the name, before and after you put the stick in:

- **macOS:** `diskutil list`. The stick is `external, physical`, for
  example `/dev/disk4`, with its size.
- **Linux:** `lsblk`. The stick is a disk such as `/dev/sdb` (not a part
  of it, such as `/dev/sdb1`), with its size.
- **Windows:** `diskpart`, then `list disk`: the stick is the disk with
  its size.

The examples below take `/dev/disk4` and `/dev/sdb`: put in your name.

## 2 Write an image (the PC)

**macOS** (`rdisk` is the same disk without the cache of macOS: much
faster):

    diskutil unmountDisk /dev/disk4
    sudo dd if=build/ucf-pc.img of=/dev/rdisk4 bs=1m
    diskutil eject /dev/disk4

`dd` says at the end `1474560 bytes transferred`. After this, macOS
sees a disk with the name UCFORTH when you put the stick in again.

**Linux** (if the system mounted the stick, first `sudo umount
/dev/sdb1`):

    sudo dd if=build/ucf-pc.img of=/dev/sdb bs=1M conv=fsync

**Windows** has no program on board that writes an image to a disk.
Write the stick on a Mac or under Linux; or with a program for this,
for example Rufus (in its "DD" mode) or Win32 Disk Imager.

**A floppy disk:** in a USB floppy disk drive as above. Under Linux with
a floppy disk drive in the PC: `sudo dd if=build/ucf-pc.img of=/dev/fd0`.

The image has 1.44 MB. On a larger stick the rest stays as it was: the
file system of the image covers only the first 1.44 MB.

## 3 Start the PC from the disk

- Put the disk in, then switch the PC on.
- The boot menu of the BIOS chooses the disk: a key right after the
  start, often F12, F11, F8 or Esc (the screen says which). Or the order
  of the disks in the setup of the BIOS (often Del or F2).
- A PC with UEFI (most PCs since about 2012): its setup must have
  "CSM" or "legacy boot" on and "secure boot" off. A PC with UEFI only
  (no CSM) does not start uCForth from this disk: it takes the stick of
  section 5.
- Some BIOSes show a USB stick as a floppy disk (USB-FDD), others as a
  hard disk (USB-HDD); the boot sector reads both.

The screen then shows the prompt `ok>`. targets/pc-bios/README.md has
more: the console, COM1, `FREEZE` (it writes to the same disk). Test an
image in QEMU first: its section "Run in QEMU".

## 4 Copy files to a card (the Pi)

The Pi reads files from the first part of the card, with FAT32.

**macOS** (MBR: the form of the partition table that the Pi reads):

    diskutil eraseDisk FAT32 UCF MBRFormat /dev/disk4

The card then is `/Volumes/UCF`. **Linux:** one partition of the type
FAT32 (`sudo fdisk /dev/sdb`: `o`, `n`, `p`, `1`, Enter, Enter, `t`,
`c`, `w`), then `sudo mkfs.vfat -F 32 /dev/sdb1`, and mount it.
**Windows:** the Explorer formats a card up to 32 GB with FAT32
(right click, "Format").

Then copy the files: `start4.elf` and `fixup4.dat` (from the firmware
of the Pi, folder `boot` of github.com/raspberrypi/firmware),
`targets/rpi4/config.txt` and `build/kernel8.img`
(targets/rpi4/README.md). Eject the card before you take it out.

## 5 A USB stick for UEFI (a PC or a Mac with Intel)

UEFI starts a program file from a stick with FAT32: no image, no boot
sector. Format the stick as for the Pi (section 4, with MBR), then copy
the program to the folder `EFI/BOOT` with the name `BOOTX64.EFI`, and the
folders of `ucf` to the root:

    diskutil eraseDisk FAT32 UCFORTH MBRFormat /dev/disk4
    mkdir -p /Volumes/UCFORTH/EFI/BOOT
    cp build/ucf-x64.efi /Volumes/UCFORTH/EFI/BOOT/BOOTX64.EFI
    cp -R forth lib host tools asm examples /Volumes/UCFORTH/
    diskutil eject /dev/disk4

Start the PC from the stick with its boot menu (section 3); "secure
boot" must be off. targets/uefi-x64/README.md has more: the Mac with
Intel, `FREEZE` (it writes into `BOOTX64.EFI` on the stick), a test in
QEMU.
