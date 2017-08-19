@echo off

setlocal

for /f "tokens=*" %%i in ('where git.exe') do set GIT_CONSOLE=%%i
if "%GIT_CONSOLE%" == "" goto NO_GIT
if "%SIGNINGKEYS%" == "" goto setup

set RELEASE_KEY=USE_RELEASE_KEY

copy "%SIGNINGKEYS%\Lasse V. Karlsen Private.snk" "%PROJECT%\Lasse V. Karlsen.snk"

call project.bat
call build.bat
if errorlevel 1 goto error

"%GIT_CONSOLE%" checkout "%PROJECT%\Lasse V. Karlsen.snk"
if errorlevel 1 goto error

echo=
echo================================================
set /P PUSHYESNO=Push package to nuget? [y/N]
if "%PUSHYESNO%" == "Y" GOTO PUSH
if "%PUSHYESNO%" == "y" GOTO PUSH
exit /B 0

:PUSH
nuget push %PROJECT%.%VERSION%%SUFFIX%.nupkg -Source https://www.nuget.org/api/v2/package
if errorlevel 1 goto error
"%GIT_CONSOLE%" tag version/%VERSION%%SUFFIX%
if errorlevel 1 goto error
start "" "https://www.nuget.org/packages/%PROJECT%/"
exit /B 0

:NO_DOTCOVER
echo Unable to locate 'dotcover.exe', is it on the path?
goto error

:NO_NUNIT
echo Unable to locate "nunit3-console.exe", is it on the path?
goto error

:NO_GIT
echo Unable to locate "git.exe", is it on the path?
goto error

:error
goto exitwitherror

:setup
echo Requires SIGNINGKEYS environment variable to be set
goto exitwitherror

:exitwitherror
"%GIT_CONSOLE%" checkout "%PROJECT%\Lasse V. Karlsen.snk"
exit /B 1