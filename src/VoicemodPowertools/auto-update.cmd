@echo off

echo Updating...
timeout 1

if exist ./Downloads/_current/voicemod-pow.exe (
    echo Update exists
    GOTO :REPLACE
) 
GOTO :EXIT

:REPLACE
echo moving from %~dp0Downloads\_current\
echo to %~dp0

robocopy "Downloads/_current/" "./" *.* /S /MOVE /is /it
RMDIR "Downloads/_current/" /S /Q
::xcopy "Downloads/_current/" "./" /k/r/e/i/s/c/h/f/o/x/y

:EXIT

timeout 1
echo Installation completed.. You can open `voicemod-pow` again

EXIT