using VoicemodPowertools.Services.Application.ApplicationConsole;

namespace VoicemodPowertools.Application.ApplicationConsole;

public class CloseApplication : ICloseApplication
{
    public async Task Execute(string[] args)
    {
        Environment.Exit(0);
    }
}