using VoicemodPowertools.Domain;
using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Services.Application.InternalConsole;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Application.InternalConsole;

public class InternalSetVersion : IInternalSetVersion
{
    private readonly IStorageFileManager _storageFile;

    public InternalSetVersion(IStorageFileManager storageFileManager)
    {
        _storageFile = storageFileManager;
    }
    
    public async Task Execute(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No version");
            Environment.Exit(1);
        }

        var version = args.FirstOrDefault();
        
        if (string.IsNullOrEmpty(version) || !CheckIfVersionStringIsCorrect(version))
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
        _storageFile.Write(
            ProgramConstants.FileLocations.Zip.Application,
            ProgramConstants.FileLocations.ApplicationSecretsFile, 
            internalApplication);
        
        Environment.Exit(0);
    }

    private bool CheckIfVersionStringIsCorrect(string input)
    {
        string[] split = input.Split('.');
        if (split.Length != 3)
            return false;

        if (split[0][0] != 'v')
            return false;

        if (!int.TryParse(split[1], out var v1))
            return false;

        if (!int.TryParse(split[2], out var v2))
            return false;

        return true;
    }
}