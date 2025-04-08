# Read key/value stored in consul from a batch file

```
@echo off
set result=
set command=One.Settix.Cli.exe get -a %application% -c %cluster% -m %computername% -o consul -h http://consul.local.com:8500/ -k THEKEY
for /f "delims=" %%i in ('%command%') do SET "result=%%i"

echo %result%
```

## This branch contains the latest version of Sett1x.Cli, which uses NET9. To build and deploy this tool, we have a Dockerfile, which must be executed locally and uploaded manually to Docker Hub. <https://hub.docker.com/r/1softwareorg/sett1x.cli>

HardFork
========
Sett1x.Cli is a hard fork from the Pandora.Cli library. At the time of the fork Pandora.Cli was v2. Sett1x.Cli will have its own versioning. In order to avoid conflicts all namespaces will be renamed and a separate nuget package is published `Sett1x.Cli`