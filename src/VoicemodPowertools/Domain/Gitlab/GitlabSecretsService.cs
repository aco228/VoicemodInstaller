using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Domain.Storage;

public class GitlabSecretsService : IGitlabSecretsService
{
    private IStorageFileManager _fileManager;
    private GitlabSecrets _secrets = null;
    
    public GitlabSecretsService(IStorageFileManager fileManager)
    {
        _fileManager = fileManager;
    }

    public GitlabSecrets Get()
    {
        if (_secrets != null)
            return _secrets;

        _secrets = _fileManager.Read<GitlabSecrets>(ProgramConstants.FileLocations.GitlabSecretsFile);
        return _secrets;
    }

    public void Save(GitlabSecrets secrets)
    {
        _secrets = secrets;
        _fileManager.Write(ProgramConstants.FileLocations.GitlabSecretsFile, secrets);
    }
    
    
}