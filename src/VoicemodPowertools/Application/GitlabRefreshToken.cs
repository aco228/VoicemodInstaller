using VoicemodPowertools.Services.Application;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Application;

public class GitlabRefreshToken : IGitlabRefreshToken
{
    private readonly IStorageHandler _storageHandler;
    
    public GitlabRefreshToken(IStorageHandler storageHandler)
    {
        _storageHandler = storageHandler;
    }
    
    public Task RefreshToken()
    {
        //var user = _storageHandler.Get<GitlabUser>();
        throw new NotImplementedException();
    }
}