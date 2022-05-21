using VoicemodPowertools.Domain.Storage.Entries;

namespace VoicemodPowertools.Services.Gitlab;

public interface IGitlabAuthorization
{
    bool IsAuthorized();
    GitlabAuthorization? GetAuthorization();
    void Save(GitlabAuthorization auth);
}