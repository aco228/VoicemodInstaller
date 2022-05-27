@echo off

echo Updating...
timeout 3

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

timeout 2
echo Exiting ombre
voicemod-pow

exit