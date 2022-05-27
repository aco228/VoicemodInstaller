using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Infrastructure.Http;
using VoicemodPowertools.Services.Gitlab;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Infrastructure.Gitlab;

public class GitlabHttpClient : RequestClient, IGitlabHttpClient
{
    private readonly IGitlabAuthorization _authorization;
    
    public GitlabHttpClient(
        IGitlabAuthorization gitlabAuthorization,
        IConfiguration configuration)
    {
        _authorization = gitlabAuthorization;
        _authorization.StateHasChanges += StorageHandlerOnStateHasChanges;

        LoadToken();
        SetBaseString(configuration.GetValue<string>("GitlabApiBaseUrl"));
    }

    private void StorageHandlerOnStateHasChanges()
    {
        LoadToken();
    }

    protected override void OnDispose()
    {
        _authorization.StateHasChanges -= StorageHandlerOnStateHasChanges;
    }

    private void LoadToken()
    {
        var storage = _authorization.GetCurrent();
        if (!storage.IsTokenValid())
            return;
        
        AddAuthorization(storage.Token);
    }
    
}