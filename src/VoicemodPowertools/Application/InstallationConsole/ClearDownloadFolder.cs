using VoicemodPowertools.Domain;
using VoicemodPowertools.Services.Application;

namespace VoicemodPowertools.Application.InstallationConsole;

public class ClearDownloadFolder : IClearDownloadFolder
{
    public async Task Execute(string[] args)
    {
        var directoryPath = new DirectoryInfo(ProgramConstants.DownloadsFolderName);
        if (!directoryPath.Exists)
        {
            Console.WriteLine($"Folder `{ProgramConstants.DownloadsFolderName}` does not exists");
            return;
        }
        
        directoryPath.Delete(true);
        Console.WriteLine($"Folder `{directoryPath.FullName}` deleted");
    }
}