using VoicemodPowertools.Infrastructure.Installation;
using VoicemodPowertools.Services.Application;

namespace VoicemodPowertools.Domain.Installation;

public static class ApplicationExtensions
{
    public static void RegisterInstallationServices(this IServiceCollection service)
    {
        service.AddTransient<IVoicemodRegistryCheck, VoicemodRegistryCheck>();
    }
}