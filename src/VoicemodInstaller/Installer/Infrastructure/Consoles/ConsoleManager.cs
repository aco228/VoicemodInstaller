using ConsoleImplementation;
using Installer.Domain.Storage.Entries;
using Installer.Services.Application.ConsoleApplications;
using Installer.Services.Storage;

namespace Installer.Infrastructure.Consoles;

public partial class ConsoleManager : ConsoleManagerBase
{
    private static ConsoleManager _instance = null;
    public static ConsoleManager Current => _instance;
    
    private readonly string[] _args;
    private readonly IServiceProvider _serviceProvider;
    

    public ConsoleManager(
        string[] args, 
        IServiceProvider serviceProvider)
    {
        _args = args;
        _serviceProvider = serviceProvider;
        _instance = this;
    }

    protected override void OnRun()
    {
        Console.Title = "Voicemod | Powertools";
        var storageHandler = _serviceProvider.GetService<IStorageHandler>();
        var storageData = storageHandler.GetCurrent();
        var gitlabAuth = storageData.Get<GitlabAuthorization>();
        if (gitlabAuth.IsValid())
        {
            Console.WriteLine($"Authenticated as {gitlabAuth.Username}");
        }

        ProcessCommand(_args);
    }

    protected override async Task OnCommand(ArgumentCommand command, string[] args)
    {
        var service = _serviceProvider.GetService(command.Application) as IConsoleApplication;
        if (service == null)
        {
            Console.WriteLine($"Internal Error. Could not find service {command.Application.ToString()}");
            return;
        }

        await service.Execute(args);
    }
}