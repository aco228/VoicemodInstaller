using System.Diagnostics;
using VoicemodPowertools.Services.Application;
using VoicemodPowertools.Services.Application.ConsoleApplications.InstallationConsole;

namespace VoicemodPowertools.Application.ConsoleApplications.InstallationConsole;

public class UnistallVoicemod : IUnistallVoicemod
{
    private readonly IVoicemodRegistryCheck _voicemodRegistry;
    
    public UnistallVoicemod(IVoicemodRegistryCheck voicemodRegistryCheck)
    {
        _voicemodRegistry = voicemodRegistryCheck;
    }
    
    public async Task Execute(string[] args)
    {
        var registry = _voicemodRegistry.GetRegistry();
        if (registry == null)
        {
            Console.WriteLine("Based on registry, voicemod is not installed");
            return;
        }

        var uninstallFile = new FileInfo($"{registry.Location}/unins000.exe");
        if (!uninstallFile.Exists)
        {
            Console.WriteLine("Could not find uninstallation file");
            return;
        }
        
        var openWebpageProcess = new ProcessStartInfo
        {
            FileName = uninstallFile.FullName,
            UseShellExecute = true
        };
        Process.Start(openWebpageProcess);
    }
}