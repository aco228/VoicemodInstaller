using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Infrastructure.Http;
using VoicemodPowertools.Services.Gitlab;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Infrastructure.Gitlab;

public class GitlabHttpClient : RequestClient, IGitlabHttpClient
{
    private readonly IStorageHandler _storageHandler;
    
    public GitlabHttpClient(
        IStorageHandler storageHandler,
        IConfiguration configuration)
    {
        _storageHandler = storageHandler;
        _storageHandler.StateHasChanges += StorageHandlerOnStateHasChanges;

        LoadToken();
        SetBaseString(configuration.GetValue<string>("GitlabApiBaseUrl"));
    }

    private void StorageHandlerOnStateHasChanges()
    {
        LoadToken();
    }

    protected override void OnDispose()
    {
        _storageHandler.StateHasChanges -= StorageHandlerOnStateHasChanges;
    }

    private void LoadToken()
    {
        var storage = _storageHandler.Get<GitlabAuthorization>();
        if (!storage.IsTokenValid())
            return;
        
        AddAuthorization(storage.Token);
    }
    
}