using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Services.Gitlab;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Application;

public class GitlabAuthorizationProvider : IGitlabAuthorization
{
    private readonly IStorageHandler _storageHandler;
    
    public GitlabAuthorizationProvider(IStorageHandler storageHandler)
    {
        _storageHandler = storageHandler;
    }
    
    public bool IsAuthorized()
    {
        var auth = _storageHandler.Get<GitlabAuthorization>();
        return auth.IsValid();
    }

    public GitlabAuthorization? GetAuthorization()
    {
        return _storageHandler.Get<GitlabAuthorization>();
    }
}