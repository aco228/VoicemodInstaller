using VoicemodPowertools.Application.ApplicationConsole;
using VoicemodPowertools.Application.DownloadConsole;
using VoicemodPowertools.Application.GitlabConsole;
using VoicemodPowertools.Application.InstallationConsole;
using VoicemodPowertools.Infrastructure.Installation;
using VoicemodPowertools.Services.Application;
using VoicemodPowertools.Services.Application.ApplicationConsole;
using VoicemodPowertools.Services.Application.DownloadsConsole;
using VoicemodPowertools.Services.Application.GitlabConsole;
using VoicemodPowertools.Services.Application.InstallationConsole;
using VoicemodPowertools.Services.Gitlab;
using IGitlabLogin = VoicemodPowertools.Services.Application.IGitlabLogin;

namespace VoicemodPowertools.Application;

public static class ApplicationExtensions
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IGitlabLogin, GitlabLogin>();
        services.AddTransient<IGitlabRefreshToken, GitlabRefreshToken>();
        services.AddTransient<IGitlabAuthorization, GitlabAuthorizationProvider>();

        services.AddTransient<IConsoleHelp, ConsoleHelp>();
        services.AddTransient<ICloseApplication, CloseApplication>();
        services.AddTransient<IGitlabRedirectToLogin, GitlabRedirectToLogin>();
        services.AddTransient<IGitlabLogout, GitlabLogout>();
        services.AddTransient<IGetCurrentGitlabUser, GetCurrentGetCurrentGitlabUser>();
        services.AddTransient<ICheckIfVoicemodIsInstalled, CheckIfVoicemodIsInstalled>();
        services.AddTransient<IUnistallVoicemod, UnistallVoicemod>();
        services.AddTransient<IGitlabPrintVersions, GitlabPrintVersions>();
        services.AddTransient<IDownloadJobArchive, DownloadJobArchive>();
        services.AddTransient<IDownloadLatestVersion, DownloadLatestVersion>();
        services.AddTransient<IClearDownloadFolder, ClearDownloadFolder>();
        services.AddTransient<IOpenDownloadFolder, OpenDownloadFolder>();
        services.AddTransient<IInstallVoicemod, InstallVoicemod>();
    }
}