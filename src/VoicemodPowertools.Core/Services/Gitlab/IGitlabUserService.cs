using VoicemodPowertools.Core.Domain.Gitlab.Models;

namespace VoicemodPowertools.Core.Services.Gitlab;

public interface IGitlabUserService
{
    public Task<GitlabUser> GetUser();
}