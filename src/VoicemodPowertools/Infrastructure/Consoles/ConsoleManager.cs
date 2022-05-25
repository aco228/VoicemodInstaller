using ConsoleImplementation;
using Humanizer;
using VoicemodPowertools.Domain;
using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Services.Application;
using VoicemodPowertools.Services.Gitlab;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Infrastructure.Consoles;

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

    protected override async Task OnRun()
    {
        Console.Title = "Voicemod | Powertools";
        Console.WriteLine("Voicemod | Powertools");
        
        var storageService = _serviceProvider.GetService<IGeneralStorageService>();
        var fileManager = _serviceProvider.GetService<IStorageFileManager>();
        var storageData = storageService.GetCurrent();

        var gitlabSecrets = fileManager.Read<GitlabSecrets>(ProgramConstants.FileLocations.GitlabSecretsFile);
        if (!gitlabSecrets.IsValid() && !_args.GetValue(ProgramConstants.IgnoreAttribute, false))
        {
            Console.WriteLine("Program corrupted!");
            Environment.Exit(1);            
        }

        var versionStorage =
            fileManager.Read<InternalApplicationData>(ProgramConstants.FileLocations.ApplicationSecretsFile);
        if (versionStorage != null)
        {
            Console.WriteLine($"Version {versionStorage.Version}");
            Console.WriteLine($"Built {versionStorage.BuiltAt.Humanize()}");
        }
        
        var gitlabAuth = storageData.Get<GitlabAuthorization>();
        if (gitlabAuth.IsValid())
        {   
            var refreshToken = _serviceProvider.GetService<IGitlabRefreshToken>();
            await refreshToken.RefreshToken();
            
            Console.WriteLine($"Authenticated as {gitlabAuth.Username}");
            Console.WriteLine();
        }

        var internalApplicationData = storageData.Get<InternalApplicationData>();
        if (internalApplicationData != null)
            internalApplicationData.Print();
        
        await ProcessCommand(_args);
        if (_args.Length >= 1 && _args.GetValue("close", false))
            Environment.Exit(0);
    }

    protected override async Task OnCommand(ArgumentCommand command, string[] args)
    {
        if (command.RequireAuth)
        {
            var gitlablAuth = _serviceProvider.GetService<IGitlabAuthorization>();
            if (!gitlablAuth.IsAuthorized())
            {
                Console.WriteLine("You are not logged in! Please log in with 'login' command.");
                return;
            }
        }
        
        var service = _serviceProvider.GetService(command.Application) as IConsoleApplication;
        if (service == null)
        {
            Console.WriteLine($"Internal Error. Could not find service {command.Application.ToString()}");
            return;
        }

        await service.Execute(args);
        Console.WriteLine();
    }

}