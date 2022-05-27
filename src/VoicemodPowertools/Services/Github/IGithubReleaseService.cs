using VoicemodPowertools.Domain.Github;

namespace VoicemodPowertools.Services.Github;

public interface IGithubReleaseService
{
    Task<GithubReleaseResponse> GetLatestRelease();
}