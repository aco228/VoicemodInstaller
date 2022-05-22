using Humanizer;
using VoicemodPowertools.Infrastructure.Consoles;
using VoicemodPowertools.Services.Application.ConsoleApplications.InstallationConsole;
using VoicemodPowertools.Services.Gitlab;

namespace VoicemodPowertools.Application.ConsoleApplications.InstallationConsole;

public class GitlabPrintVersions : IGitlabPrintVersions
{
    private readonly IConfiguration _configuration;
    private readonly IGitlabJobService _jobService;

    public GitlabPrintVersions(
        IGitlabJobService gitlabJobService,
        IConfiguration configuration)
    {
        _configuration = configuration;
        _jobService = gitlabJobService;
    }
    
    public async Task Execute(string[] args)
    {
        var projectId = _configuration.GetValue<long>("GitlabVoicemodDesktopPID");
        
        var count = args.GetValue("count", 5);
        if (count > 50) count = 50;

        var onlyDevelop = args.GetValue("develop", false);

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