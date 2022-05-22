using System.Diagnostics;
using VoicemodPowertools.Infrastructure.Consoles;
using VoicemodPowertools.Services.Application;
using VoicemodPowertools.Services.Application.InstallationConsole;

namespace VoicemodPowertools.Application.InstallationConsole;

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
        
        if (args.GetValue("wait", false))
            for (;;)
            {
                var uf = new FileInfo($"{registry.Location}/unins000.exe");
                if (!uf.Exists)
                {
                    Console.WriteLine("Voicemod is unistalled");
                    return;
                }
                    
                Thread.Sleep(2000);
            }
            
    }
}