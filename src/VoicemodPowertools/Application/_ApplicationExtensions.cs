using VoicemodPowertools.Application.ConsoleApplications.ApplicationConsole;
using VoicemodPowertools.Application.ConsoleApplications.GitlabConsole;
using VoicemodPowertools.Application.ConsoleApplications.InstallationConsole;
using VoicemodPowertools.Infrastructure.Installation;
using VoicemodPowertools.Services.Application;
using VoicemodPowertools.Services.Application.ConsoleApplications.ApplicationConsole;
using VoicemodPowertools.Services.Application.ConsoleApplications.GitlabConsole;
using VoicemodPowertools.Services.Application.ConsoleApplications.InstallationConsole;
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
    }
}