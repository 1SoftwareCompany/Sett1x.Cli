# Read key/value stored in consul from a batch file

```
@echo off
set result=
set command=Elders.Pandora.Cli.exe get -a %application% -c %cluster% -m %computername% -o consul -h http://consul.local.com:8500/ -k THEKEY
for /f "delims=" %%i in ('%command%') do SET "result=%%i"

echo %result%
```

## This branch contains the latest version of Pandora.Cli, which uses NET9. To build and deploy this tool, we have a Dockerfile, which must be executed locally and uploaded manually to Docker Hub. <https://hub.docker.com/r/1softwareorg/pandora.cli>
