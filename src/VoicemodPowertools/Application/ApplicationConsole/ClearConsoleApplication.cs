using VoicemodPowertools.Services.Application.ApplicationConsole;

namespace VoicemodPowertools.Application.ApplicationConsole;

public class ClearConsoleApplication : IClearConsoleApplication
{
    public async Task Execute(string[] args)
    {
        Console.Clear();
    }
}