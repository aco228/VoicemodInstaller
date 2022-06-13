using VoicemodPowertools.Core.Domain.InternalStorage.Entries;

namespace VoicemodPowertools.Core.Services.Gitlab;

public interface IGitlabAuthorization
{   
    delegate void OnStorageChange();
    event OnStorageChange StateHasChanges;  
    GitlabAuthorization? GetCurrent();
    bool IsAuthorized();
    void Save(GitlabAuthorization auth);
    void Clear();
}