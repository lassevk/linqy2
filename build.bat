@echo off

setlocal

if exist Linqy\bin rd /s /q Linqy\bin
if errorlevel 1 goto error

nuget restore
if errorlevel 1 goto error

msbuild Linqy\Linqy.csproj /target:Clean,Rebuild /p:Configuration=Debug
if errorlevel 1 goto error

exit /B 0

:error
exit /B 1
