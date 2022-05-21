using Installer.Domain.Gitlab.Jobs.Models;

namespace Installer.Domain.Gitlab.Jobs;

public interface IGitlabJobService
{
    Task<JobResponse[]> GetProjectJobs(long projectId);
}