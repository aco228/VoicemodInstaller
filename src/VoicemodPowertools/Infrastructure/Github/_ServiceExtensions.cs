using VoicemodPowertools.Services.Github;

namespace VoicemodPowertools.Infrastructure.Github;

public static class _ServiceExtensions
{
    public static void RegisterGithubServices(this IServiceCollection service)
    {
        service.AddTransient<IGithubReleaseService, GithubReleaseService>();
    }
}