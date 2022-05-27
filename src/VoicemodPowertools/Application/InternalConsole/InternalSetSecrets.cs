using System.Text.RegularExpressions;
using VoicemodPowertools.Domain;
using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Infrastructure;
using VoicemodPowertools.Infrastructure.Consoles;
using VoicemodPowertools.Services.Application.InternalConsole;
using VoicemodPowertools.Services.InternalStorage;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Application.InternalConsole;

public class InternalSetSecrets : IInternalSetSecrets
{
    private readonly IStorageManager _fileManager;

    public InternalSetSecrets(IStorageManager storageManager)
    {
        _fileManager = storageManager;
    }
    
    public async Task Execute(string[] args)
    {
        try
        {
            var gitlabSecrets = new GitlabSecrets
            {
                ClientId = args.GetValue("clientId", string.Empty),
                ClientSecret = args.GetValue("clientSecret", string.Empty),
                ProjectId = args.GetValue("projectId", (long) -1),
            };

            if (!gitlabSecrets.IsValid())
            {
                Console.WriteLine("No secrets offered");
                Environment.Exit(1);
            }

            var version = args.GetValue("version", string.Empty).GetVersion();
            if (string.IsNullOrEmpty(version))
            {
                Console.WriteLine("No version offered");
                Environment.Exit(1);
            }

            gitlabSecrets.Print();
            _fileManager.Write(
                ProgramConstants.File.App.Zip,
                ProgramConstants.File.App.GitlabSecretsFile, 
                gitlabSecrets);

            var internalApplication = new InternalApplicationData
            {
                Version = version,
                BuiltAt = DateTime.Now,
            };
            
            Console.WriteLine($"Version set to ${version}");
            _fileManager.Write(
                ProgramConstants.File.App.Zip,
                ProgramConstants.File.App.ApplicationSecretsFile, 
                internalApplication);
            
            Thread.Sleep(3500);

            var file = new FileInfo(ProgramConstants.File.App.Zip);
            if (file.Exists)
            {
                Console.WriteLine($"FILE EXISTS {file.FullName}");
            }
            
            Environment.Exit(0);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Exception {ex}");
        }
    }

}