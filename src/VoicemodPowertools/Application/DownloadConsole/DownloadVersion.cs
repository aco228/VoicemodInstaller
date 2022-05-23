using VoicemodPowertools.Domain.Gitlab.Jobs;
using VoicemodPowertools.Infrastructure.Consoles;
using VoicemodPowertools.Services.Application.DownloadsConsole;
using VoicemodPowertools.Services.Gitlab;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Application.DownloadConsole;

public class DownloadVersion : IDownloadVersion
{
    private readonly IGitlabSecretsService _gitlabSecrets;
    private readonly IGitlabJobService _jobService;
    private readonly IGitlabJobDownloader _jobDownloader;
    
    public DownloadVersion(
        IGitlabSecretsService gitlabSecretsService,
        IGitlabJobService gitlabJobService,
        IGitlabJobDownloader gitlabJobDownloader)
    {
        _gitlabSecrets = gitlabSecretsService;
        _jobService = gitlabJobService;
        _jobDownloader = gitlabJobDownloader;
    }
    
    public async Task Execute(string[] args)
    {
        try
        {
            var projectId = _gitlabSecrets.Get().ProjectId;

            Console.WriteLine("Looking for version...");
            JobResponse job = null;
            if (args.Length >= 1 && long.TryParse(args[0], out var jobId))
                job = await _jobService.GetProjectJob(projectId, jobId);
            else
                await foreach (var ejob in _jobService.GetJobs(projectId, 1, args.GetValue("develop", false)))
                    job = ejob;

            job.Print();
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
                Unzip = args.GetValue("unzip", true),
                OpenFolderOnDownload = args.GetValue("open", false),
            });
        }
        catch
        {
            Console.WriteLine("Error getting/downloading latest desktop version");
        }
        
    }
}