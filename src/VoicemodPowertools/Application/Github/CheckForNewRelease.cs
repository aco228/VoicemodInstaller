using System.Diagnostics;
using VoicemodPowertools.Core.Domain.InternalStorage.Entries;
using VoicemodPowertools.Core.Infrastructure;
using VoicemodPowertools.Core.Services.Github;
using VoicemodPowertools.Core.Services.Http;
using VoicemodPowertools.Core.Services.InternalStorage;
using VoicemodPowertools.Domain;
using VoicemodPowertools.Infrastructure;

namespace VoicemodPowertools.Application.Github;

public interface ICheckForNewRelease
{
    Task Run();
}

public class CheckForNewRelease : ICheckForNewRelease
{
    private readonly IGithubReleaseService _githubReleaseService;
    private readonly IStorageManager _storageManager;
    private readonly IDownloadClient _downloadClient;
    
    public CheckForNewRelease(
        IGithubReleaseService githubReleaseService,
        IStorageManager storageManager,
        IDownloadClient downloadClient)
    {
        _githubReleaseService = githubReleaseService;
        _storageManager = storageManager;
        _downloadClient = downloadClient;
    }
    
    public async Task Run()
    {
        try
        {
            if (Program.OnDebug)
            {
                ConsoleDebug.WriteLine("Ignoring auto update on debug mode");
                return;   
            }
            
            var autoupdateFile = new FileInfo(ProgramConstants.NameOfAutoInstallBat.GetAbsolutPath());
            if (!autoupdateFile.Exists)
            {
                Console.WriteLine("ERROR!!! = Missing bat file for auto update");
                return;
            }
            
            CreateCurrentBatchFile(autoupdateFile);

            var currentDownloadDirectory = new DirectoryInfo(ProgramConstants.DownloadsAutoUpdateDirectory);
            if (currentDownloadDirectory.Exists)
                currentDownloadDirectory.Delete(true);

            var latestRelease = await _githubReleaseService.GetLatestRelease();
            
            var versionStorage = _storageManager.Read<InternalApplicationData>(
                ProgramConstants.File.App.Zip,
                ProgramConstants.File.App.ApplicationSecretsFile);

            var weightLatestVersion = GetVersionWeight(latestRelease.Version);
            var weightCurrentVersion = GetVersionWeight(versionStorage.Version);
            
            if (weightLatestVersion <= weightCurrentVersion)
            {
                ConsoleDebug.WriteLine("We have current version, no need to update");
                return;
            }

            await _downloadClient.Download(
                latestRelease.Assets.FirstOrDefault().Url,
                $"{ProgramConstants.AutoUpdate.NameOfTheFile}.zip",
                ProgramConstants.DownloadsFolderName,
                true,
                false);

            var psi = new ProcessStartInfo
            {
                FileName = ProgramConstants.NameOfCurrentAutoInstallBat.GetAbsolutPath(),
                Verb = "runas",
            };
            Process.Start(psi);
            
            Console.WriteLine("We have new version");
            Console.WriteLine("APPLICATION WILL CLOSE AND REOPEN");
            Environment.Exit(0);
        }
        catch (Exception ex)
        {
            Console.WriteLine("---> Error checking for new version");
            ConsoleDebug.WriteLine(ex.ToString());
        }
    }

    private void CreateCurrentBatchFile(FileInfo autoUpdateBatchFile)
    {
        File.Delete(ProgramConstants.NameOfCurrentAutoInstallBat.GetAbsolutPath());

        var content = File.ReadAllText(autoUpdateBatchFile.FullName)
            .Replace("[DOWNLOAD_FILE_LOCATION]", Path.Combine(ProgramConstants.DownloadsFolderName, ProgramConstants.AutoUpdate.NameOfTheFile, "voicemod-pow.exe"))
            .Replace("[DOWNLOAD_DIRECTORY]", Path.Combine(ProgramConstants.DownloadsFolderName, ProgramConstants.AutoUpdate.NameOfTheFile))
            .Replace("[CURRENT_DIRECTORY]", autoUpdateBatchFile.Directory.FullName);
        
        File.WriteAllText(ProgramConstants.NameOfCurrentAutoInstallBat.GetAbsolutPath(), content);
    }

    private int GetVersionWeight(string version)
    {
        var split = version.Substring(1).Split('.');
        if(split.Length != 3)
            throw new ArgumentException("Version split length is not 3");

        var multiplier = 1;
        var result = 0;
        
        for (var i = 2; i >= 0; i--)
        {
            if (!int.TryParse(split[i], out var integerValue))
                throw new ArgumentException($"Could not parse part of version to int {split[i]}");
            
            result += (integerValue * multiplier);
            multiplier *= 100;
        }

        return result;
    }
}