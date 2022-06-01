using Microsoft.Extensions.DependencyInjection;
using VoicemodPowertools.Core.Services.Github;

namespace VoicemodPowertools.Core.Infrastructure.Github;

public static class _ServiceExtensions
{
    public static void RegisterGithubServices(this IServiceCollection service)
    {
        service.AddTransient<IGithubReleaseService, GithubReleaseService>();
    }
}