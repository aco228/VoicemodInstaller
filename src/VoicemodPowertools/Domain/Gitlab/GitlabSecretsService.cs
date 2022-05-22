using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Domain.Storage;

public class GitlabSecretsService : IGitlabSecretsService
{
    private readonly string STORAGE_NAME = "st.srg";
    
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

        _secrets = _storageHandler.GetStorageFile<GitlabSecrets>(STORAGE_NAME);
        return _secrets;
    }

    public void Save(GitlabSecrets secrets)
    {
        _secrets = secrets;
        _storageHandler.SaveStorageFile(STORAGE_NAME, secrets);
    }
    
    
}