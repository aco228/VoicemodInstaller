using VoicemodPowertools.Core.Domain.Gitlab.Projects;

namespace VoicemodPowertools.Services.Gitlab;

public interface IGitlabProjectsService
{
    public Task<ProjectResponse> GetProject(long projectId);
}