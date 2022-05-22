using Humanizer;
using VoicemodPowertools.Infrastructure.Consoles;
using VoicemodPowertools.Services.Application.DownloadsConsole;
using VoicemodPowertools.Services.Gitlab;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Application.DownloadConsole;

public class GitlabPrintVersions : IGitlabPrintVersions
{
    private readonly IGitlabJobService _jobService;
    private readonly IGitlabSecretsService _gitlabSecrets;

    public GitlabPrintVersions(
        IGitlabJobService gitlabJobService,
        IGitlabSecretsService gitlabSecretsService)
    {
        _gitlabSecrets = gitlabSecretsService;
        _jobService = gitlabJobService;
    }
    
    public async Task Execute(string[] args)
    {
        var count = args.GetValue("count", 5);
        if (count > 50) count = 50;

        var onlyDevelop = args.GetValue("develop", false);
        var projectId = _gitlabSecrets.Get().ProjectId;

        try
        {
            Console.WriteLine($"{"JobId", 10} - {"Branch", 60} - {"User", 20} -- Date");
            Console.WriteLine($"---------------------------------------------------------------------------------------------------------------------");
            await foreach (var version in _jobService.GetJobs(projectId, count, onlyDevelop))
                Console.WriteLine($"{version.Id, 10} - {version.Reference, 60} - {version.User.Username, 20} -- {version.Finished.Humanize()}");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error trying to print versions");
        }
    }
}