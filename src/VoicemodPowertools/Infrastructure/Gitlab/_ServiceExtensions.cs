using VoicemodPowertools.Infrastructure.Gitlab.Authorization;
using VoicemodPowertools.Infrastructure.Gitlab.Jobs;
using VoicemodPowertools.Services.Gitlab;

namespace VoicemodPowertools.Infrastructure.Gitlab;

public static class ServiceExtensions
{
    public static void RegisterGitlabServices(this IServiceCollection services)
    {
        services.AddTransient<IGitlabHttpClient, GitlabHttpClient>();
        services.AddTransient<IGitlabAuthorizationService, GitlabAuthorizationService>();
        services.AddTransient<IGitlabJobService, GitlabJobService>();
        services.AddTransient<IGitlabUserService, GitlabUserService>();
        services.AddTransient<IGitlabJobDownloader, GitlabJobDownloader>();
    }
}