using System.Diagnostics;
using System.IO.Compression;
using Humanizer;
using VoicemodPowertools.Domain;
using VoicemodPowertools.Domain.Gitlab.Jobs;
using VoicemodPowertools.Infrastructure.Consoles;
using VoicemodPowertools.Services.Application.InstallationConsole;
using VoicemodPowertools.Services.Gitlab;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Application.InstallationConsole;

public class InstallVoicemod : IInstallVoicemod
{

    private readonly IGitlabSecretsService _gitlabSecrets;
    private readonly IGitlabJobService _jobService;
    private readonly IGitlabJobDownloader _jobDownloader;
    private readonly IUnistallVoicemod _unistallVoicemod;

    public InstallVoicemod(
        IGitlabJobService jobService,
        IGitlabSecretsService gitlabSecretsService,
        IGitlabJobDownloader jobDownloader,
        IUnistallVoicemod unistallVoicemod)
    {
        _gitlabSecrets = gitlabSecretsService;
        _jobService = jobService;
        _jobDownloader = jobDownloader;
        _unistallVoicemod = unistallVoicemod;
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

            if (job == null)
            {
                Console.WriteLine("Could not find version :( ");
                return;
            }

            if (CheckIfJobIsAlreadyDownloaded(job))
            {
                Console.WriteLine($"Looks like this version `{job.Id}` is already downloaded");
                Console.Write("Are you sure you want to continue (Y): ");
                var response = Console.ReadLine();
                if (string.IsNullOrEmpty(response) || !response.ToLower().Equals("y"))
                {
                    Console.WriteLine("You declined re-installation!");
                    return;
                }
            }

            var jobDownloader = DownloadJobAndGetExecutable(job);
            var unistallVoicemod = _unistallVoicemod.Execute(new[] {"--wait"});

            Task.WaitAll(jobDownloader, unistallVoicemod);
            Thread.Sleep(1000);

            if (string.IsNullOrEmpty(jobDownloader.Result))
            {
                Console.WriteLine("Could not find executable file location. Aborting");
                return;
            }

            var openWebpageProcess = new ProcessStartInfo
            {
                FileName = jobDownloader.Result,
                UseShellExecute = true
            };
            Process.Start(openWebpageProcess);
        }
        catch (Exception ex)
        {
            Console.WriteLine("There was an error executing");
        }
    }

    private bool CheckIfJobIsAlreadyDownloaded(JobResponse job)
    {
        var folderPath = new DirectoryInfo(ProgramConstants.DownloadsFolderName).FullName;
        
        // We have zip, so we need to unzip it and get a location
        var zipFile = new FileInfo(Path.Combine(folderPath, $"{job.Id}.zip"));
        if (zipFile.Exists)
            return true;
        
        // We have downloaded and unziped this job
        var zipDirectory = new DirectoryInfo(Path.Combine(folderPath, job.Id.ToString()));
        if (zipDirectory.Exists)
            return true;

        return false;
    }

    private async Task<string> DownloadJobAndGetExecutable(JobResponse job)
    {
        Console.WriteLine("\tFound version:");
        Console.WriteLine($"\t\t{"JobId", 5} - {job.Id}");
        Console.WriteLine($"\t\t{"UserId", 5} - {job.User.Username}");
        Console.WriteLine($"\t\t{"Branch", 5} - {job.Reference}");
        Console.WriteLine($"\t\t{"Created", 5} - {job.Finished.Humanize()}");
        Console.WriteLine();
        
        var folderPath = new DirectoryInfo(ProgramConstants.DownloadsFolderName).FullName;
        
        // We have zip, so we need to unzip it and get a location
        var zipFile = new FileInfo(Path.Combine(folderPath, $"{job.Id}.zip"));
        if (zipFile.Exists)
        {
            if (zipFile.Length == 0)
            {
                Console.WriteLine("File size is zero! ERROR!");
                throw new ArgumentException("FileSize is zero");
            }
            
            var newDirectoryLocation = Path.Combine(zipFile.Directory.FullName, job.Id.ToString());
            ZipFile.ExtractToDirectory(zipFile.FullName, newDirectoryLocation);
            File.Delete(zipFile.FullName);
            Console.WriteLine("There was zip file. Unziped");
            return GetExecutableInFile(newDirectoryLocation);
        }
        
        // We have downloaded and unziped this job
        var zipDirectory = new DirectoryInfo(Path.Combine(folderPath, job.Id.ToString()));
        if (zipDirectory.Exists)
        {
            Console.WriteLine("There was unziped file");
            return GetExecutableInFile(zipDirectory.FullName);
        }

        await _jobDownloader.Download(new()
        {
            JobId = job.Id,
            Unzip = true,
        });
        
        return GetExecutableInFile(zipDirectory.FullName);
    }

    private string GetExecutableInFile(string filePath)
        => Directory.GetFiles(Path.Combine(filePath, "VoicemodReleaseVersions"), "*.exe").FirstOrDefault();

}