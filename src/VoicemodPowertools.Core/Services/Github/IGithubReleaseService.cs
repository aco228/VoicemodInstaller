using VoicemodPowertools.Core.Domain.Github;

namespace VoicemodPowertools.Core.Services.Github;

public interface IGithubReleaseService
{
    Task<GithubReleaseResponse> GetLatestRelease();
}