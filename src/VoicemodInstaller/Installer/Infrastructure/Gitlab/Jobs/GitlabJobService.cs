using Installer.Domain.Gitlab.Jobs;
using Installer.Domain.Gitlab.Jobs.Models;

namespace Installer.Infrastructure.Gitlab.Jobs;

public class GitlabJobService : IGitlabJobService
{
    
    
    public Task<JobResponse[]> GetProjectJobs(long projectId)
    {
        throw new NotImplementedException();
    }
}