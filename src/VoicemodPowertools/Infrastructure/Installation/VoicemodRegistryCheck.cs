using VoicemodPowertools.Domain.Installation;
using VoicemodPowertools.Services.Application;
using static Microsoft.Win32.Registry;

namespace VoicemodPowertools.Infrastructure.Installation;

public class VoicemodRegistryCheck : IVoicemodRegistryCheck
{
    private readonly string REGISTRY = @"SOFTWARE\Voicemod\Voicemod Desktop"; 
    
    public VoicemodRegistryResponse? GetRegistry()
    {
#pragma warning disable CA1416
        var key = LocalMachine.OpenSubKey(REGISTRY);
#pragma warning restore CA1416

        if (key == null)
            return null;

        var location = key.GetValue("InstallPath")?.ToString();
        if (string.IsNullOrEmpty(location))
            return null;
        
        return new()
        {
            Location = location,
        };
    }
}