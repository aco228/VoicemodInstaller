using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Infrastructure.Consoles;
using VoicemodPowertools.Services.Application.InternalConsole;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Application.InternalConsole;

public class InternalSetVersion : IInternalSetVersion
{
    private readonly IStorageHandler _storageHandler;

    public InternalSetVersion(IStorageHandler storageHandler)
    {
        _storageHandler = storageHandler;
    }
    
    public async Task Execute(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No version");
            return;
            
        }

        var version = args.FirstOrDefault();

        var internalApplication = new InternalApplicationData
        {
            Version = version,
            BuiltAt = DateTime.Now,
        };
        
        Console.WriteLine($"Version set to ${version}");
        _storageHandler.Save(internalApplication);
    }
}