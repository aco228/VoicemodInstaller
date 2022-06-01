using VoicemodPowertools.Core.Domain.Gitlab.Models;
using VoicemodPowertools.Core.Services.Gitlab;

namespace VoicemodPowertools.Core.Infrastructure.Gitlab;

public class GitlabUserService : IGitlabUserService
{
    private readonly IGitlabHttpClient _client;
    
    public GitlabUserService(IGitlabHttpClient gitlabHttpClient)
    {
        _client = gitlabHttpClient;
    }

    public Task<GitlabUser> GetUser()
        => _client.Get<GitlabUser>("user");
}