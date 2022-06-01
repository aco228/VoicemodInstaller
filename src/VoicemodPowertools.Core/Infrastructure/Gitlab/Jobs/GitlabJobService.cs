using VoicemodPowertools.Core.Domain.Gitlab.Jobs;
using VoicemodPowertools.Core.Services.Gitlab;
using VoicemodPowertools.Domain.Gitlab.Jobs;

namespace VoicemodPowertools.Core.Infrastructure.Gitlab.Jobs;

public class GitlabJobService : IGitlabJobService
{
    private readonly IGitlabHttpClient _httpClient;
    
    public GitlabJobService(IGitlabHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<List<JobResponse>> GetProjectJobs(GitlabJobsRequest request)
        => _httpClient.Get<List<JobResponse>>($"projects/{request.ProjectId}/jobs?scope=success&page={request.Page}&per_page={request.PerPage}");

    public Task<JobResponse> GetProjectJob(long projectId, long jobId)
        => _httpClient.Get<JobResponse>($"projects/{projectId}/jobs/{jobId}");

    public async IAsyncEnumerable<JobResponse> GetJobs(long projectId, int count = 1, bool onlyDevelop = false, bool onlyNonExpired = true)
    {
        var currentPage = 1;
        var found = 0;
        
        for (;;)
        {
            if (found >= count)
                break;
                
            var response = await GetProjectJobs(new()
            {
                ProjectId = projectId,
                Page = currentPage,
                PerPage = 100,
            });
                
            if(response.Count == 0) break;

            var qaIntResponse = response.Where(x => x.Name.Equals("3.QA Integration"));
            if (onlyDevelop) qaIntResponse = qaIntResponse.Where(x => x.Reference.Equals("develop"));

            foreach (var version in qaIntResponse)
            {
                if (onlyNonExpired && version.ArtifactsFile == null)
                    continue;
                
                yield return version;
                if(++found >= count) break;
            }

            currentPage++;
        }
    }
}