# FileReadTest
Checks if files are readable

## About

This tool provides a very cheap method to check the harddrive surface.
You specify a directory and the tool simply tries to read all files in the directory and its children.

The method is way faster than a surface analysis but will not scan free space and it can't read files,
that are open for writing by other applications.

## SSD

Don't use the tool on SSD drives.
SSD Drives tend to swap blocks of data on the disk to ensure they wear out evenly,
this is called wear leveling.
Because of this process file blocks can be swapped every time data is written to the disk.
The tool will not damage the drive but it also will not provide any useful information.

## What do on errors

If the error is simply a "permission denied" type error you don't have to do anything.
You can try to run the tool as administrator to get more access rights.

If a file is unreadable, I recommend you run this command (replace `X:` with your drive):

    CHKDSK.EXE X: /F /V /R /X

### Explanation

- **X:** This is the drive to scan.
- **/F** Attempts to fix errors on the disk
- **/V** Prints more details
- **/R** Performs surface analysis (checks physical disk health)
- **/X** Forces dismount of drive if applications will not yield their handles.

## Use cases

There are no use-cases. If you think your disk is defective, just replace it.
You might want to rip out some components of this tool however,
for example the background directory tree scanner.
