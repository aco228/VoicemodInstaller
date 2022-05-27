using VoicemodPowertools.Domain;
using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Infrastructure;
using VoicemodPowertools.Services.Github;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Application.Github;

public interface ICheckForNewRelease
{
    Task Run();
}

public class CheckForNewRelease : ICheckForNewRelease
{
    private readonly IGithubReleaseService _githubReleaseService;
    private readonly IStorageFileManager _storageFileManager;
    
    public CheckForNewRelease(
        IGithubReleaseService githubReleaseService,
        IStorageFileManager storageFileManager)
    {
        _githubReleaseService = githubReleaseService;
        _storageFileManager = storageFileManager;
    }
    
    public async Task Run()
    {
        try
        {
            var latestRelease = await _githubReleaseService.GetLatestRelease();
            Console.WriteLine("Latest release: " + latestRelease.Version);
            
            var versionStorage = _storageFileManager.Read<InternalApplicationData>(
                ProgramConstants.FileLocations.Zip.Application,
                ProgramConstants.FileLocations.ApplicationSecretsFile);

            var weightLatestVersion = GetVersionWeight(latestRelease.Version);
            var weightCurrentVersion = GetVersionWeight(versionStorage.Version);
            
            if (weightLatestVersion <= weightCurrentVersion)
            {
                ConsoleDebug.WriteLine("We have current version, no need to update");
                return;
            }
            
            
            
            ConsoleDebug.WriteLine("We have new version");
        }
        catch (Exception ex)
        {
            ConsoleDebug.WriteLine("Error checking for new version");
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