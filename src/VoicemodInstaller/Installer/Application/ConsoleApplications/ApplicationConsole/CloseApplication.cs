using Installer.Services.Application.ConsoleApplications.ApplicationConsole;

namespace Installer.Application.ConsoleApplications.ApplicationConsole;

public class CloseApplication : ICloseApplication
{
    public async Task Execute(string[] args)
    {
        Environment.Exit(0);
    }
}