using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Services.InternalStorage;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Domain.Storage;

public class GitlabSecretsService : IGitlabSecretsService
{
    private IStorageManager _fileManager;
    private GitlabSecrets _secrets = null;
    
    public GitlabSecretsService(IStorageManager fileManager)
    {
        _fileManager = fileManager;
    }

    public GitlabSecrets Get()
    {
        if (_secrets != null)
            return _secrets;

        _secrets = _fileManager.Read<GitlabSecrets>(
            ProgramConstants.File.App.Zip,
            ProgramConstants.File.App.GitlabSecretsFile);
        
        return _secrets;
    }

    public void Save(GitlabSecrets secrets)
    {
        _secrets = secrets;
        _fileManager.Write(
            ProgramConstants.File.App.Zip,
            ProgramConstants.File.App.GitlabSecretsFile, secrets);
    }
    
    
}