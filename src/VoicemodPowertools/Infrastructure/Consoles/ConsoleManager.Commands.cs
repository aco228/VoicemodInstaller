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
                    new () { Command = "in", Application = typeof(IInstallVoicemod), Description = "Shortcut for `install`", RequireAuth = true, }, 
                    new ()
                    {
                        Command = "install", Application = typeof(IInstallVoicemod), RequireAuth = true,
                        Description = "Install version of voicemod",
                        DescriptionMultiline = new List<string>
                        {
                          "First `uninstall` will be triggered if there is some version currently installed",
                          "After you go through uninstall, and download new version, you will automatically start new installation",
                          "In first argument you can send JobId for specific version you want to download/install",
                          "To get available versions, use `versions` command",
                          "Use `--develop` flag if you want to download latest version from develop branch",
                        },
                    },
                    new ()
                    {
                        Command = "is-installed", Application = typeof(ICheckIfVoicemodIsInstalled),
                        Description = "Check if voicemod is currently installed"
                    },
                    new () { Command = "un", Application = typeof(IUnistallVoicemod), Description = "Shortcut for `uninstall`" },
                    new ()
                    {
                        Command = "uninstall", Application = typeof(IUnistallVoicemod),
                        Description = "Trigger setup for Voicemod Desktop uninstallation",
                        DescriptionMultiline = new()
                        {
                            "Use `--wait` for program to wait until voicemod is uninstalled",
                        },
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
                            "Use `--develop' if you want to get only versions for develop (default=FALSE)",
                        },
                    },
                    new()
                    {
                        Command = "d", Application = typeof(IDownloadVersion), RequireAuth = true, Description = "Shortcut for `download-version`",
                    },
                    new()
                    {
                        Command = "download", Application = typeof(IDownloadVersion), RequireAuth = true,
                        Description = "Download job archive",
                        DescriptionMultiline = new()
                        {
                            "Will look only for jobs with state `3.QA Integration`",
                            "[IMPORTANT] Use first parameter as number if you want to download specific version",
                            "            You can find specific version for download with `versions` command",
                            "            If version is not provided, it will find last version avaliable",
                            "Use `--unzip` if you want to unzip file when downloaded (TRUE by default)",
                            "Use `--open` if you want to open folder where file is downloaded",
                            "Use `--develop` if you want to download last version with develop brant",
                            "                it will be used only if you did not provide specific version",
                            "                with first parameter",
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