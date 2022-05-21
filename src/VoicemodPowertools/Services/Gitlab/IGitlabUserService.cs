using VoicemodPowertools.Domain.Gitlab.Models;

namespace VoicemodPowertools.Services.Gitlab;

public interface IGitlabUserService
{
    public Task<GitlabUser> GetUser();
}