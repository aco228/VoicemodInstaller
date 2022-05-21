using Installer.Application.GitlabLoginApp;
using Installer.Application.GitlabRefreshTokenApp;

namespace Installer.Application;

public static class ApplicationExtensions
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IGitlabLogin, GitlabLogin>();
        services.AddTransient<IGitlabRefreshToken, GitlabRefreshToken>();
    }
}