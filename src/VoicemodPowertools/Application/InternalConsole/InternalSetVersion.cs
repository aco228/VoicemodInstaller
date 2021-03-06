using VoicemodPowertools.Core.Domain.InternalStorage.Entries;
using VoicemodPowertools.Core.Services.InternalStorage;
using VoicemodPowertools.Domain;
using VoicemodPowertools.Core.Infrastructure;
using VoicemodPowertools.Services.Application.InternalConsole;

namespace VoicemodPowertools.Application.InternalConsole;

public class InternalSetVersion : IInternalSetVersion
{
    private readonly IStorageManager _storageManager;

    public InternalSetVersion(IStorageManager storageManagerManager)
    {
        _storageManager = storageManagerManager;
    }
    
    public async Task Execute(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No version");
            Environment.Exit(1);
        }

        var version = args.FirstOrDefault().GetVersion();
        if (string.IsNullOrEmpty(version))
        {
            Console.WriteLine("Version wrong format");
            Environment.Exit(1);
        }

        var internalApplication = new InternalApplicationData
        {
            Version = version,
            BuiltAt = DateTime.Now,
        };
        
        Console.WriteLine($"Version set to ${version}");
        _storageManager.Write(
            ProgramConstants.File.App.Zip,
            ProgramConstants.File.App.ApplicationSecretsFile, 
            internalApplication);
        
        Environment.Exit(0);
    }
}