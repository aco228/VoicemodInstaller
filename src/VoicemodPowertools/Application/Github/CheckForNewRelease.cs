using System.Diagnostics;
using VoicemodPowertools.Domain;
using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Infrastructure;
using VoicemodPowertools.Services.Github;
using VoicemodPowertools.Services.Http;
using VoicemodPowertools.Services.InternalStorage;

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

            var executionFile = $"current_{ProgramConstants.NameOfAutoInstallBat}";
            File.Delete(executionFile.GetAbsolutPath());
            File.Copy(autoupdateFile.Name.GetAbsolutPath(), executionFile.GetAbsolutPath());
            
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

            Process.Start(executionFile);
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