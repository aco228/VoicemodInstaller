using ConsoleImplementation;
using VoicemodPowertools.Services.Application;
using VoicemodPowertools.Services.Application.ApplicationConsole;
using VoicemodPowertools.Services.Application.DownloadsConsole;
using VoicemodPowertools.Services.Application.GitlabConsole;
using VoicemodPowertools.Services.Application.InstallationConsole;

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
                    new () { Command = "un", Application = typeof(IUnistallVoicemod), Description = "Shortcut for `uninstall`" },
                    new ()
                    {
                        Command = "uninstall", Application = typeof(IUnistallVoicemod),
                        Description = "Trigger setup for Voicemod Desktop uninstallation"
                    },
                }
            },
            new()
            {
              Name  = "Download",
              Commands = new()
              {
                    new()
                    {
                        Command = "versions", Application = typeof(IGitlabPrintVersions), RequireAuth = true,
                        Description = "Get all available versions for download from gitlab",
                        DescriptionMultiline = new()
                        {
                            "Will look only for jobs with state `3.QA Integration`",
                            "Use `--count=[NUMBER]' for number of versions you want to print (default=5, maximum=50)",
                            "Use `--develop=true' if you want to get only versions for develop (default=FALSE)",
                        },
                    },
                    new()
                    {
                        Command = "d-v", Application = typeof(IDownloadJobArchive), RequireAuth = true, Description = "Shortcut for `download-version`",
                    },
                    new()
                    {
                        Command = "download-version", Application = typeof(IDownloadJobArchive), RequireAuth = true,
                        Description = "Download job archive",
                        DescriptionMultiline = new()
                        {
                            "Will look only for jobs with state `3.QA Integration`",
                            "[REQUIRED] As parameter send job id! You can get job id from `versions` command",
                            "Use `--unzip` if you want to unzip file when downloaded (FALSE by default)",
                            "Use `--open` if you want to open folder where file is downloaded"
                        },
                    },
                    new()
                    {
                        Command = "d-l", Application = typeof(IDownloadLatestVersion), RequireAuth = true, Description = "Shortcut for `download-latest`",
                    },
                    new()
                    {
                        Command = "download-latest", Application = typeof(IDownloadLatestVersion), RequireAuth = true,
                        Description = "Download latest version from gitlab",
                        DescriptionMultiline = new()
                        {
                            "Will look only for jobs with state `3.QA Integration`",
                            "Use `--develop` if you only want to download one from develop branch (FALSE by default)`",
                            "Use `--unzip` if you want to unzip file when downloaded (FALSE by default)",
                            "Use `--open` if you want to open folder where file is downloaded"
                        },
                    },
                    new()
                    {
                        Command = "o-d", Application = typeof(IOpenDownloadFolder), Description = "Shortcut for `downloads`",
                    },
                    new()
                    {
                        Command = "downloads", Application = typeof(IOpenDownloadFolder), RequireAuth = false, 
                        Description = "Open downloads folder to check all downloaded versions",
                    },
              },
            },
            new()
            {
                Name = "Application",
                Commands = new()
                {
                    new ()
                    {
                        Command  = "c", Application = typeof(ICloseApplication),
                        Description = "Close application"
                    },
                    new ()
                    {
                        Command  = "close", Application = typeof(ICloseApplication),
                        Description = "Close application"
                    },
                    new ()
                    {
                        Command  = "clear", Application = typeof(IClearDownloadFolder),
                        Description = "Clear downloads folder and all of its contents"
                    },
                    new ()
                    {
                        Command  = "empty", Application = typeof(IClearDownloadFolder),
                        Description = "Clear downloads folder and all of its contents"
                    },
                    new ()
                    {
                        Command = "h", Application = typeof(IConsoleHelp),
                        Description = "Get all avaliable commands",
                    },
                    new ()
                    {
                        Command = "--help", Application = typeof(IConsoleHelp),
                        Description = "Get all avaliable commands",
                    },
                }
            }
        };
    }
}