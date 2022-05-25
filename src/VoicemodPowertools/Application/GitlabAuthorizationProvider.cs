using VoicemodPowertools.Domain;
using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Services.Gitlab;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Application;

public class GitlabAuthorizationProvider : IGitlabAuthorization
{
    private IGeneralStorageService _storageService;
    
    public event IGitlabAuthorization.OnStorageChange? StateHasChanges;
    
    public GitlabAuthorizationProvider(IGeneralStorageService generalStorageService)
    {
        _storageService = generalStorageService;
    }

    public GitlabAuthorization GetCurrent()
        => _storageService.Get<GitlabAuthorization>() ?? new GitlabAuthorization();

    public bool IsAuthorized()
        => GetCurrent().IsValid();

    public void Save(GitlabAuthorization auth)
    {
        _storageService.Save(auth);
        StateHasChanges?.Invoke();
    }

    public void Clear()
    {
        _storageService.Save(new GitlabAuthorization());
        StateHasChanges?.Invoke();   
    }
}