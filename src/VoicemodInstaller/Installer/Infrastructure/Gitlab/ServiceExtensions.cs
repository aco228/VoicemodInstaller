using Installer.Domain.Gitlab.Authorization;
using Installer.Domain.Gitlab.Jobs;
using Installer.Infrastructure.Gitlab.Authorization;
using Installer.Infrastructure.Gitlab.Jobs;
using Installer.Services.Gitlab;

namespace Installer.Infrastructure.Gitlab;

public static class ServiceExtensions
{
    public static void RegisterGitlabServices(this IServiceCollection services)
    {
        services.AddTransient<IGitlabHttpClient, GitlabHttpClient>();
        services.AddTransient<IGitlabAuthorizationService, GitlabAuthorizationService>();
        services.AddTransient<IGitlabJobService, GitlabJobService>();
        services.AddTransient<IGitlabUserService, GitlabUserService>();
    }
}