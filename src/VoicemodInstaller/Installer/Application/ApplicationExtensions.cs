using Installer.Application.ConsoleApplications;
using Installer.Application.ConsoleApplications.ApplicationConsole;
using Installer.Application.ConsoleApplications.GitlabConsole;
using Installer.Services.Application;
using Installer.Services.Application.ConsoleApplications.ApplicationConsole;
using Installer.Services.Application.ConsoleApplications.GitlabConsole;
using GitlabUser = Installer.Domain.Gitlab.Models.GitlabUser;
using IGitlabLogin = Installer.Services.Application.IGitlabLogin;

namespace Installer.Application;

public static class ApplicationExtensions
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IGitlabLogin, GitlabLogin>();
        services.AddTransient<IGitlabRefreshToken, GitlabRefreshToken>();

        services.AddTransient<IConsoleHelp, ConsoleHelp>();
        services.AddTransient<ICloseApplication, CloseApplication>();
        services.AddTransient<IGitlabRedirectToLogin, GitlabRedirectToLogin>();
        services.AddTransient<IGitlabLogout, GitlabLogout>();
        services.AddTransient<IGetCurrentGitlabUser, GetCurrentGetCurrentGitlabUser>();
    }
}