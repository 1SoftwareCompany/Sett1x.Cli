# Read key/value stored in consul from a batch file

```
@echo off
set result=
set command=Elders.Pandora.Cli.exe get -a %application% -c %cluster% -m %computername% -o consul -h http://consul.local.com:8500/ -key THEKEY
for /f "delims=" %%i in ('%command%') do SET "result=%%i"

echo %result%
```
