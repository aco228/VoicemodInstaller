using Installer.Domain.Gitlab.Models;
using Installer.Services.Storage;

namespace Installer.Application.GitlabRefreshTokenApp;

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