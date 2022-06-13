using VoicemodPowertools.Core.Domain.Gitlab.Jobs;
using VoicemodPowertools.Domain.Gitlab.Jobs;

namespace VoicemodPowertools.Core.Services.Gitlab;

public interface IGitlabJobService
{
    /// <summary>
    /// https://docs.gitlab.com/ee/api/jobs.html#list-project-jobs
    /// </summary>
    Task<List<JobResponse>> GetProjectJobs(GitlabJobsRequest request);
    
    Task<JobResponse> GetProjectJob(long projectId, long jobId);

    IAsyncEnumerable<JobResponse> GetJobs(long projectId, int count = 1, bool onlyDevelop = false, bool onlyNonExpired = true);

}
