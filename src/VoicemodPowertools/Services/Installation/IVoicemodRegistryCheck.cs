using VoicemodPowertools.Domain.Installation;

namespace VoicemodPowertools.Services.Application;

public interface IVoicemodRegistryCheck
{
    VoicemodRegistryResponse? GetRegistry();
}