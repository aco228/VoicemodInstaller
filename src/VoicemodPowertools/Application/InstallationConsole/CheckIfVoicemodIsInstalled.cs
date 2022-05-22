using VoicemodPowertools.Services.Application;
using VoicemodPowertools.Services.Application.InstallationConsole;

namespace VoicemodPowertools.Application.InstallationConsole;

public class CheckIfVoicemodIsInstalled : ICheckIfVoicemodIsInstalled
{
    private readonly IVoicemodRegistryCheck _voicemodRegistryCheck;
    
    public CheckIfVoicemodIsInstalled(IVoicemodRegistryCheck voicemodRegistryCheck)
    {
        _voicemodRegistryCheck = voicemodRegistryCheck;
    }
    
    public async Task Execute(string[] args)
    {
        var registryResponse = _voicemodRegistryCheck.GetRegistry();
        if (registryResponse != null)
        {
            Console.WriteLine($"Voicemod is installed, on location {registryResponse.Location}");
            
        }
        else
        {
            Console.WriteLine("Voicemod is NOT installed");
        }
    }
}