﻿using ConsoleImplementation;
using VoicemodPowertools.Services.Application.ConsoleApplications.ApplicationConsole;
using VoicemodPowertools.Services.Application.ConsoleApplications.GitlabConsole;
using VoicemodPowertools.Services.Application.ConsoleApplications.InstallationConsole;

namespace VoicemodPowertools.Infrastructure.Consoles;

public partial class ConsoleManager : ConsoleManagerBase
{
    protected override List<ArgumentGroup> GetGroupsAndCommands()
    {
        return new List<ArgumentGroup>()
        {
            new()
            {
                Name = "Gitlab Authentication",
                Commands = new()
                {
                    new()
                    {
                        Command = "login", Application = typeof(IGitlabRedirectToLogin),
                        Description = "Login using gitlab account"
                    },
                    new ()
                    {
                        Command = "user", Application = typeof(IGetCurrentGitlabUser), RequireAuth = true,
                        Description = "Get informations about currently logged in user"
                    },
                    new ()
                    {
                        Command = "logout", Application = typeof(IGitlabLogout), RequireAuth = true,
                        Description = "Revoke tokens from gitlab"
                    },
                }
            },
            new()
            {
                Name = "Installation",
                Commands = new()
                {
                    new ()
                    {
                        Command = "is-installed", Application = typeof(ICheckIfVoicemodIsInstalled),
                        Description = "Check if voicemod is currently installed"
                    },
                    new ()
                    {
                        Command = "uninstall", Application = typeof(IUnistallVoicemod),
                        Description = "Trigger setup for Voicemod Desktop uninstallation"
                    },
                    new()
                    {
                        Command = "versions", Application = typeof(IGitlabPrintVersions), RequireAuth = true,
                        Description = "Get all available versions for download from gitlab",
                        DescriptionMultiline = new()
                        {
                            "Use `--count=[NUMBER]' for number of versions you want to print (default=5, maximum=50)",
                            "Use `--develop=true' if you want to get only versions for develop (default=FALSE)",
                        },
                    },
                    new()
                    {
                        Command = "download-version", Application = typeof(IDownloadJobArchive), RequireAuth = true,
                        Description = "Download job archive",
                        DescriptionMultiline = new()
                        {
                          "[REQUIRED] As parameter send job id! You can get job id from `versions` command",
                          "Use `--unzip` if you want to unzip file when downloaded (FALSE by default)",
                        },
                    }
                }
            },
            new()
            {
                Name = "Application",
                Commands = new()
                {
                    new ()
                    {
                      Command  = "close", Application = typeof(ICloseApplication),
                      Description = "Close application"
                    },
                    new ()
                    {
                        Command = "--help", Application = typeof(IConsoleHelp),
                        Description = "Get all avaliable commands",
                    }
                }
            }
        };
    }
}