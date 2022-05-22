using Humanizer;
using VoicemodPowertools.Domain;
using VoicemodPowertools.Domain.Gitlab.Jobs;
using VoicemodPowertools.Infrastructure.Consoles;
using VoicemodPowertools.Infrastructure.Installation;
using VoicemodPowertools.Services.Application;
using VoicemodPowertools.Services.Gitlab;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Application.DownloadConsole;

public class DownloadLatestVersion : IDownloadLatestVersion
{
    private readonly IGitlabJobService _jobService;
    private readonly IGitlabSecretsService _gitlabSecrets;
    private readonly IGitlabJobDownloader _jobDownloader;
    
    public DownloadLatestVersion(
        IGitlabSecretsService gitlabSecretsService,
        IGitlabJobService gitlabJobService,
        IGitlabJobDownloader jobDownloader)
    {
        _jobService = gitlabJobService;
        _gitlabSecrets = gitlabSecretsService;
        _jobDownloader = jobDownloader;
    }
    
    public async Task Execute(string[] args)
    {
        var develop = args.GetValue("develop", false);
        var projectId = _gitlabSecrets.Get().ProjectId;

        try
        {
            Console.WriteLine("Looking for latest version..");
            
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

            await _jobDownloader.Download(new()
            {
                JobId = job.Id,
                Unzip = args.GetValue("unzip", false),
                OpenFolderOnDownload = args.GetValue("open", false),
            });
        }
        catch(Exception ex)
        {
            Console.WriteLine("Error getting/downloading latest desktop version");
        }
    }
}