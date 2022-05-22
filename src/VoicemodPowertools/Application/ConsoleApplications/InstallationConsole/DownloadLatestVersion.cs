using Humanizer;
using VoicemodPowertools.Domain;
using VoicemodPowertools.Domain.Gitlab.Jobs;
using VoicemodPowertools.Infrastructure.Consoles;
using VoicemodPowertools.Infrastructure.Installation;
using VoicemodPowertools.Services.Application;
using VoicemodPowertools.Services.Gitlab;

namespace VoicemodPowertools.Application.ConsoleApplications.InstallationConsole;

public class DownloadLatestVersion : IDownloadLatestVersion
{
    private readonly IConfiguration _configuration;
    private readonly IGitlabJobService _jobService;
    private readonly IGitlabAuthorization _authorization;
    
    public DownloadLatestVersion(
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
        var unzip = args.GetValue("unzip", false);
        var develop = args.GetValue("develop", false);
        var open = args.GetValue("open", false);
        var projectId = _configuration.GetValue<long>("GitlabVoicemodDesktopPID");
        var gitlabBaseUrl = _configuration.GetValue<string>("GitlabApiBaseUrl");

        try
        {
            JobResponse job = null;
            await foreach (var jobEntry in _jobService.GetJobs(projectId, 1, develop))
                job = jobEntry;
            
            if (job == null)
            {
                Console.WriteLine("Could not load job for some reason :(");
                return;
            }
            
            Console.WriteLine("\tFound version:");
            Console.WriteLine($"\t\t{"JobId", 5} - {job.Id}");
            Console.WriteLine($"\t\t{"UserId", 5} - {job.User.Username}");
            Console.WriteLine($"\t\t{"Branch", 5} - {job.Reference}");
            Console.WriteLine($"\t\t{"Created", 5} - {job.Finished.Humanize()}");
            Console.WriteLine();

            if (new DirectoryInfo($"Downloads/{job.Id}").Exists 
                || (new FileInfo($"Downloads/{job.Id}.zip")).Exists)
            {
                Console.WriteLine($"Version {job.Id} is already downloaded. Check it here:");
                Console.WriteLine((new DirectoryInfo($"Downloads/{job.Id}")).FullName);
                return;
            }
            
            var url = $"{gitlabBaseUrl}projects/{projectId}/jobs/{job.Id}/artifacts";
            var downloadManager = new DownloadManager(
                url, 
                job.Id.ToString(), 
                ProgramConstants.DownloadsFolderName, 
                unzip,
                open);
            
            if (!downloadManager.StartDownload(_authorization.GetAuthorization()))
            {
                Console.WriteLine("Not downloaded for some reason :(");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine("Error getting/downloading latest desktop version");
        }
    }
}