using VoicemodPowertools.Domain.Gitlab.Jobs;

namespace VoicemodPowertools.Services.Gitlab;

public interface IGitlabJobService
{
    Task<JobResponse[]> GetProjectJobs(long projectId);
}