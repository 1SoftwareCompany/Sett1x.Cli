@echo off

SETLOCAL

SET NUGET="%LocalAppData%\NuGet\NuGet.exe"
SET FAKE="%LocalAppData%\FAKE\tools\Fake.exe"
SET NYX="%LocalAppData%\Nyx\tools\build_next.fsx"
SET GITVERSION="%LocalAppData%\GitVersion.CommandLine\tools\GitVersion.exe"
SET MSBUILD14_TOOLS_PATH="%ProgramFiles(x86)%\MSBuild\14.0\bin\MSBuild.exe"
SET MSPEC="%LocalAppData%\Machine.Specifications.Runner.Console\tools\mspec-clr4.exe"

IF NOT EXIST %MSBUILD14_TOOLS_PATH% (
  echo In order to run this tool you need either Visual Studio 2015 or
  echo Microsoft Build Tools 2015 tools installed.
  echo.
  echo Visit this page to download either:
  echo.
  echo http://www.visualstudio.com/en-us/downloads/visual-studio-2015-downloads-vs
  echo.
)

echo Downloading NuGet.exe...
IF NOT EXIST "%LocalAppData%\NuGet" md "%LocalAppData%\NuGet"
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile '%NUGET%'"

echo Downloading latest version of NuGet.Core...
IF NOT EXIST "%LocalAppData%\NuGet.Core" %NUGET% "install" "NuGet.Core" "-OutputDirectory" "%LocalAppData%" "-ExcludeVersion" "-Version" "2.11.1"

echo Downloading FAKE...
IF NOT EXIST "%LocalAppData%\FAKE" %NUGET% "install" "FAKE" "-OutputDirectory" "%LocalAppData%" "-ExcludeVersion" "-Version" "4.32.0"

echo Downloading GitVersion.CommandLine...
IF NOT EXIST "%LocalAppData%\GitVersion.CommandLine" %NUGET% "install" "GitVersion.CommandLine" "-OutputDirectory" "%LocalAppData%" "-ExcludeVersion" "-Version" "3.6.1"

echo Downloading Machine.Specifications.Runner.Console...
IF NOT EXIST "%LocalAppData%\Machine.Specifications.Runner.Console" %NUGET% "install" "Machine.Specifications.Runner.Console" "-OutputDirectory" "%LocalAppData%" "-ExcludeVersion"

echo Downloading Nyx...
%NUGET% "install" "Nyx" "-OutputDirectory" "%LocalAppData%" "-ExcludeVersion" "-PreRelease"

%FAKE% %NYX% "target=clean" -st
%FAKE% %NYX% "target=RestoreNugetPackages" -st

IF NOT [%1]==[] (set RELEASE_NUGETKEY="%1")

SET SUMMARY="Pandora aims to externalize the application configuration. Usually in .NET projects the configuration is done in app/web.config with transformations."
SET DESCRIPTION="Pandora aims to externalize the application configuration. Usually in .NET projects the configuration is done in app/web.config with transformations. The problem arises when production configuration is needed which should not be part of the application repository because it is an OSS project for example. This is where Pandora comes. You can configure the application using the following structure and store these files in a separate repository"

%FAKE% %NYX% appName=Pandora.Cli appSummary=%SUMMARY% appDescription=%DESCRIPTION% nugetkey=%RELEASE_NUGETKEY% nugetPackageName=Pandora.Cli