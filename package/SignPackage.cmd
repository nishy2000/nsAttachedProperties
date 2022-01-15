@echo off
REM Copyright 2020 nishy software
SETLOCAL enabledelayedexpansion
SET BAT_NAME=%~n0%~x0
SET TARGET_FILE1=%1
SET CERT_SUBJECT_NAME=nishy software

IF "x-" == "x-%TARGET_FILE1%" GOTO HELP

REM  Signe packages
ECHO =======================
ECHO Signe packages
ECHO -----------------------
FOR %%i in (%*) do (
    echo sign: %%~ni%%~xi
    nuget.exe sign "%%i" -Verbosity quiet -CertificateSubjectName "%CERT_SUBJECT_NAME%" -Timestamper "http://sha256timestamp.ws.symantec.com/sha256/timestamp"
)
ECHO =======================

GOTO END

:ERROR
ECHO ERROR occurred
COLOR C1
PAUSE
COLOR F
GOTO END

:HELP
ECHO %BAT_NAME% NugetPackageFiles
ECHO Each file in NugetPackageFiles ends with .nupkg.

:END
ENDLOCAL
EXIT /B