using VoicemodPowertools.Domain.Gitlab.Jobs;
using VoicemodPowertools.Services.Gitlab;

namespace VoicemodPowertools.Infrastructure.Gitlab.Jobs;

public class GitlabJobService : IGitlabJobService
{
    
    
    public Task<JobResponse[]> GetProjectJobs(long projectId)
    {
        throw new NotImplementedException();
    }
}