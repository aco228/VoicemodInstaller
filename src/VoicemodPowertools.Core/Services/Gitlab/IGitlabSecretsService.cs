using VoicemodPowertools.Core.Domain.InternalStorage.Entries;

namespace VoicemodPowertools.Core.Services.Gitlab;

public interface IGitlabSecretsService
{
    GitlabSecrets Get();
    void Save(GitlabSecrets secrets);
}