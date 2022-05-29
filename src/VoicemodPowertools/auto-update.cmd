@echo off

echo Updating...
timeout 1

if exist [DOWNLOAD_FILE_LOCATION] (
    echo Update exists
    GOTO :REPLACE
) 
GOTO :EXIT

:REPLACE

robocopy "[DOWNLOAD_DIRECTORY]/" "[CURRENT_DIRECTORY]" *.* /S /MOVE /is /it
RMDIR "[DOWNLOAD_DIRECTORY]/" /S /Q

:EXIT

timeout 1
echo Installation completed.. You can open `voicemod-pow` again

EXIT