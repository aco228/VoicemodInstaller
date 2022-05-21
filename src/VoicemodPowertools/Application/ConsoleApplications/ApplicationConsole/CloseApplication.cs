using VoicemodPowertools.Services.Application.ConsoleApplications.ApplicationConsole;

namespace VoicemodPowertools.Application.ConsoleApplications.ApplicationConsole;

public class CloseApplication : ICloseApplication
{
    public async Task Execute(string[] args)
    {
        Environment.Exit(0);
    }
}