using VoicemodPowertools.Infrastructure.Consoles;
using VoicemodPowertools.Services.Application.DownloadsConsole;
using VoicemodPowertools.Services.Gitlab;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Application.DownloadConsole;

public class DownloadJobArchive : IDownloadJobArchive
{
    private readonly IGitlabJobService _jobService;
    private readonly IGitlabSecretsService _gitlabSecrets;
    private readonly IGitlabJobDownloader _jobDownloader;
    
    public DownloadJobArchive(
        IGitlabSecretsService gitlabSecretsService,
        IGitlabJobService gitlabJobService,
        IGitlabJobDownloader jobDownloader)
    {
        _gitlabSecrets = gitlabSecretsService;
        _jobService = gitlabJobService;
        _jobDownloader = jobDownloader;
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

        var projectId = _gitlabSecrets.Get().ProjectId;

        try
        {
            var job = await _jobService.GetProjectJob(projectId, jobId);
            if (job == null)
            {
                Console.WriteLine($"There is no job with id={jobId}");
                return;
            }

            await _jobDownloader.Download(new()
            {
                JobId = job.Id,
                Unzip = args.GetValue("unzip", true),
                OpenFolderOnDownload = args.GetValue("open", false),
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading archive for JobId={jobId}");
        }
    }
}