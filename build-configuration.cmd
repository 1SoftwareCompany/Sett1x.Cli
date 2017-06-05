@echo off

%FAKE% %NYX% "target=clean" -st
%FAKE% %NYX% "target=RestoreNugetPackages" -st

IF NOT [%1]==[] (set RELEASE_NUGETKEY="%1")

SET SUMMARY="Pandora aims to externalize the application configuration. Usually in .NET projects the configuration is done in app/web.config with transformations."
SET DESCRIPTION="Pandora aims to externalize the application configuration. Usually in .NET projects the configuration is done in app/web.config with transformations. The problem arises when production configuration is needed which should not be part of the application repository because it is an OSS project for example. This is where Pandora comes. You can configure the application using the following structure and store these files in a separate repository"

%FAKE% %NYX% appName=Pandora.Cli appSummary=%SUMMARY% appDescription=%DESCRIPTION% nugetkey=%RELEASE_NUGETKEY% nugetPackageName=Pandora.Cli
