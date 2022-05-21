using Installer.Domain.Storage.Entries;
using Installer.Infrastructure.Http;
using Installer.Services.Gitlab;
using Installer.Services.Storage;

namespace Installer.Infrastructure.Gitlab;

public class GitlabHttpClient : RequestClient, IGitlabHttpClient
{
    private readonly IStorageHandler _storageHandler;
    
    public GitlabHttpClient(
        IStorageHandler storageHandler,
        IConfiguration configuration)
    {
        _storageHandler = storageHandler;

        LoadToken();
        SetBaseString(configuration.GetValue<string>("GitlabApiBaseUrl"));
    }

    public void LoadToken()
    {
        var storage = _storageHandler.Get<GitlabAuthorization>();
        if (storage == null)
            return;
        
        AddAuthorization(storage.Token);
    }
}