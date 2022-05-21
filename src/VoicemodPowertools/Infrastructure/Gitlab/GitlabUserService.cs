using VoicemodPowertools.Domain.Gitlab.Models;
using VoicemodPowertools.Services.Gitlab;

namespace VoicemodPowertools.Infrastructure.Gitlab;

public class GitlabUserService : IGitlabUserService
{
    private readonly IGitlabHttpClient _client;
    
    public GitlabUserService(IGitlabHttpClient gitlabHttpClient)
    {
        _client = gitlabHttpClient;
    }

    public Task<GitlabUser> GetUser()
    {
        _client.LoadToken(); // as we will receive them after client has been initialized
        return _client.Get<GitlabUser>("user");
    }
}