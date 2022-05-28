using VoicemodPowertools.Domain.Gitlab.Projects;
using VoicemodPowertools.Services.Gitlab;

namespace VoicemodPowertools.Infrastructure.Gitlab.Projects;

public class GitlabProjectsService : IGitlabProjectsService
{
    private readonly IGitlabHttpClient _client;
    
    public GitlabProjectsService(IGitlabHttpClient httpClient)
    {
        _client = httpClient;
    }

    public Task<ProjectResponse> GetProject(long projectId)
        => _client.Get<ProjectResponse>($"projects/{projectId}");
}