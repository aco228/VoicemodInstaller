using VoicemodPowertools.Core.Domain.Gitlab.Jobs;
using VoicemodPowertools.Core.Infrastructure;
using VoicemodPowertools.Core.Services.Gitlab;
using VoicemodPowertools.Infrastructure;
using VoicemodPowertools.Infrastructure.Consoles;
using VoicemodPowertools.Services.Application.DownloadsConsole;

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
                job = await _jobService.GetJobs(projectId, 1, args.GetValue("develop", false)).FirstOrDefault();

            if (job.ArtifactsFile == null)
            {
                Console.WriteLine("Artifact is expired!");
                return;
            }
            
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
        catch(Exception ex)
        {
            Console.WriteLine("Error getting/downloading latest desktop version");
            ConsoleDebug.WriteLine(ex.ToString());
        }
        
    }
}