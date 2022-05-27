using VoicemodPowertools.Domain.Storage.Entries;

namespace VoicemodPowertools.Services.Gitlab;

public interface IGitlabAuthorization
{   
    delegate void OnStorageChange();
    event OnStorageChange StateHasChanges;  
    GitlabAuthorization? GetCurrent();
    bool IsAuthorized();
    void Save(GitlabAuthorization auth);
    void Clear();
}