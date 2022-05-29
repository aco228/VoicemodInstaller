# VoicemodInstaller

Internal application for reinstallation of INT versions

## Installation

You have to install dotnet 6 on your machine. 

- Check what is version of your windows (x64 or x86)
- Download and install `ASP.NET Core Runtime 6.0.x` and `.NET Desktop Runtime 6.0.x` from this link https://dotnet.microsoft.com/en-us/download/dotnet/6.0
- Pick x64 or x86 based on version of your windows

![image](https://user-images.githubusercontent.com/35331284/170881283-f0fef88b-cd1a-47ad-80b9-9146cb16f61d.png)

After dotnet is installed, donwload latest version for [releases](https://github.com/aco228/VoicemodInstaller/releases) on your machine (you can add folder on which you extracted application on system PATH variable, for easy access).

## Usage

- Start `voicemod-pow.exe` you donwloaded from [releases](https://github.com/aco228/VoicemodInstaller/releases)
- To access all avaliable command, enter `--help`
- Use `login` command to give access to your gitlab account

## Downloading specific version on your machine

- To get all avaliable version of application, use command `versions`.
  - Use `--count=5` or any number to limit number of versions


![image](https://user-images.githubusercontent.com/35331284/170882208-0a85b954-e45e-4559-9576-fcc317be0079.png)

- To downnload specific version on your machine, use command `download [JobId]` (where `JobId` is the value from `versions` command)
  - Use `--open` to open folder where file is downloaded, after download is completed

## Reinstallation

- For reinstallation of application use `install` command
- As a first parameter you can use `JobId` (information you get from `versions` command), to install specific version (RECOMENDED!)
- If you do not specify `JobId`, app will download and install latest avaliable version.

Flow of reinstallation:
- If you use command `install 123` where 123 is JobId you got from `versions` command:
  - You will automatically get window for Voicemod reinstallation
  - In meantime, version you specified will be downloaded
  - After you finish uninstallation process, you will get new window to install version you just downloaded


