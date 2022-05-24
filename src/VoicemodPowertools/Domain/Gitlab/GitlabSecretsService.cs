using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Domain.Storage;

public class GitlabSecretsService : IGitlabSecretsService
{   
    private readonly IStorageHandler _storageHandler;
    private GitlabSecrets _secrets = null;
    
    public GitlabSecretsService(IStorageHandler storageHandler)
    {
        _storageHandler = storageHandler;
    }

    public GitlabSecrets Get()
    {
        if (_secrets != null)
            return _secrets;

        _secrets = _storageHandler.Get<GitlabSecrets>();
        return _secrets;
    }

    public void Save(GitlabSecrets secrets)
    {
        _secrets = secrets;
        _storageHandler.Save(secrets);
    }
    
    
}