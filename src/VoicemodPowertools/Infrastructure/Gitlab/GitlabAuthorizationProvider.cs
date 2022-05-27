using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Services.Gitlab;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Infrastructure.Gitlab;

public class GitlabAuthorizationProvider : IGitlabAuthorization
{
    private IStoreService _storeService;
    
    public event IGitlabAuthorization.OnStorageChange? StateHasChanges;
    
    public GitlabAuthorizationProvider(IStoreService storeService)
    {
        _storeService = storeService;
    }

    public GitlabAuthorization GetCurrent()
        => _storeService.Get<GitlabAuthorization>() ?? new GitlabAuthorization();

    public bool IsAuthorized()
        => GetCurrent().IsValid();

    public void Save(GitlabAuthorization auth)
    {
        _storeService.Save(auth);
        StateHasChanges?.Invoke();
    }

    public void Clear()
    {
        _storeService.Save(new GitlabAuthorization());
        StateHasChanges?.Invoke();   
    }
}