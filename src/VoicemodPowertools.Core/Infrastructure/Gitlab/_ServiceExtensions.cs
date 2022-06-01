using Microsoft.Extensions.DependencyInjection;
using VoicemodPowertools.Core.Infrastructure.Gitlab.Authorization;
using VoicemodPowertools.Core.Infrastructure.Gitlab.Jobs;
using VoicemodPowertools.Core.Services.Gitlab;
using VoicemodPowertools.Infrastructure.Gitlab.Projects;
using VoicemodPowertools.Services.Gitlab;

namespace VoicemodPowertools.Core.Infrastructure.Gitlab;

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