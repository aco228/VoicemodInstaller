using VoicemodPowertools.Application;
using VoicemodPowertools.Domain.Gitlab.Jobs;
using VoicemodPowertools.Infrastructure.Gitlab.Authorization;
using VoicemodPowertools.Infrastructure.Gitlab.Jobs;
using VoicemodPowertools.Infrastructure.Gitlab.Projects;
using VoicemodPowertools.Services.Gitlab;

namespace VoicemodPowertools.Infrastructure.Gitlab;

public static class ServiceExtensions
{
    public static void RegisterGitlabServices(this IServiceCollection services)
    {
        services.AddSingleton<IGitlabAuthorization, GitlabAuthorizationProvider>();
        
        services.AddTransient<IGitlabHttpClient, GitlabHttpClient>();
        services.AddTransient<IGitlabAuthorizationService, GitlabAuthorizationService>();
        services.AddTransient<IGitlabJobService, GitlabJobService>();
        services.AddTransient<IGitlabUserService, GitlabUserService>();
        services.AddTransient<IGitlabJobDownloader, GitlabJobDownloader>();
        services.AddTransient<IGitlabProjectsService, GitlabProjectsService>();
    }

}