using VoicemodPowertools.Infrastructure.Consoles;
using VoicemodPowertools.Infrastructure.Installation;
using VoicemodPowertools.Services.Application.ConsoleApplications.InstallationConsole;
using VoicemodPowertools.Services.Gitlab;

namespace VoicemodPowertools.Application.ConsoleApplications.InstallationConsole;

public class DownloadJobArchive : IDownloadJobArchive
{
    private readonly IGitlabJobService _jobService;
    private readonly IConfiguration _configuration;
    private readonly IGitlabAuthorization _authorization;
    
    public DownloadJobArchive(
        IConfiguration configuration,
        IGitlabAuthorization gitlabAuthorization,
        IGitlabJobService gitlabJobService)
    {
        _configuration = configuration;
        _authorization = gitlabAuthorization;
        _jobService = gitlabJobService;
    }
    
    public async Task Execute(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("You must send JobId. Get JobId from `versions` command");
            return;
        }

        if (!long.TryParse(args[0], out var jobId))
        {
            Console.WriteLine("JobId must be long");
            return;
        }

        var unzip = args.GetValue("unzip", false);
        var projectId = _configuration.GetValue<long>("GitlabVoicemodDesktopPID");
        var gitlabBaseUrl = _configuration.GetValue<string>("GitlabApiBaseUrl");

        try
        {
            var job = await _jobService.GetProjectJob(projectId, jobId);
            if (job == null)
            {
                Console.WriteLine($"There is no job with id={jobId}");
                return;
            }

            var url = $"{gitlabBaseUrl}projects/{projectId}/jobs/{job.Id}/artifacts";
            var downloadManager = new DownloadManager(
                url,
                job.Id.ToString(),  
                $@"Downloads", 
                unzip);
            
            var downloaded = downloadManager.StartDownload(_authorization.GetAuthorization());

            if (!downloaded)
                Console.WriteLine("Error downloading");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading archive for JobId={jobId}");
        }
    }
}