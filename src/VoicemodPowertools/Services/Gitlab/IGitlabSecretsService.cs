using VoicemodPowertools.Domain.Storage.Entries;

namespace VoicemodPowertools.Services.Storage;

public interface IGitlabSecretsService
{
    GitlabSecrets Get();
    void Save(GitlabSecrets secrets);
}