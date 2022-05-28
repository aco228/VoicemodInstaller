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

:EXIT

timeout 1
cls
echo Installation completed.. You can open `voicemod-pow` again

exit