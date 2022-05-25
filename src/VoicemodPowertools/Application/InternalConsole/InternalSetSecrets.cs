using VoicemodPowertools.Domain;
using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Infrastructure.Consoles;
using VoicemodPowertools.Services.Application.InternalConsole;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Application.InternalConsole;

public class InternalSetSecrets : IInternalSetSecrets
{
    private readonly IStorageHandler _storageHandler;

    public InternalSetSecrets(IStorageHandler storageHandler)
    {
        _storageHandler = storageHandler;
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
            
            var version = args.GetValue("version", string.Empty);
            if (string.IsNullOrEmpty(version))
            {
                Console.WriteLine("No version offered");
                Environment.Exit(1);
            }

            gitlabSecrets.Print();
            _storageHandler.Save(gitlabSecrets);

            var internalApplication = new InternalApplicationData
            {
                Version = version,
                BuiltAt = DateTime.Now,
            };
            
            Console.WriteLine($"Version set to ${version}");
            _storageHandler.Save(internalApplication);
            
            Thread.Sleep(3500);

            var file = new FileInfo(ProgramConstants.SecretsFile);
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