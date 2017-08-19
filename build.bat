@echo off

setlocal

for /f "tokens=*" %%i in ('where nunit3-console.exe') do set NUNIT_CONSOLE=%%i
for /f "tokens=*" %%i in ('where dotcover.exe') do set DOTCOVER_CONSOLE=%%i

echo %DOTCOVER_CONSOLE%

if "%DOTCOVER_CONSOLE%" == "" goto NO_DOTCOVER
if "%NUNIT_CONSOLE%" == "" goto NO_NUNIT

call project.bat

if exist *.nupkg del *.nupkg
if errorlevel 1 goto error

for /d %%f in (*.*) do (
    if exist "%%f\bin" rd /s /q "%%f\bin"
    if errorlevel 1 goto error
    if exist "%%f\obj" rd /s /q "%%f\obj"
    if errorlevel 1 goto error
)
if errorlevel 1 goto error

nuget restore
if errorlevel 1 goto error

msbuild "%PROJECT%.sln" /target:Clean,Rebuild /p:Configuration=%CONFIGURATION% /p:Version=%VERSION%%SUFFIX% /p:AssemblyVersion=%VERSION% /p:FileVersion=%VERSION% /p:DefineConstants="%CONFIGURATION%;%RELEASE_KEY%" /verbosity:minimal
if errorlevel 1 goto error

set TESTDLL=%CD%\%PROJECT%.Tests\bin\%CONFIGURATION%\%PROJECT%.Tests.dll
if exist "%TESTDLL%" "%DOTCOVER_CONSOLE%" analyze /Output="%PROJECT%-CodeCoverage.html" /ReportType="HTML" /TargetExecutable="%NUNIT_CONSOLE%" /TargetArguments="%TESTDLL% --work=""%CD%"""
if errorlevel 1 goto error

copy %PROJECT%\bin\%CONFIGURATION%\%PROJECT%*.nupkg .\
if errorlevel 1 goto error

exit /B 0

:NO_DOTCOVER
echo Unable to locate 'dotcover.exe', is it on the path?
goto error

:NO_NUNIT
echo Unable to locate "nunit3-console.exe", is it on the path?
goto error

:error
exit /B 1
