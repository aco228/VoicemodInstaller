using System.Diagnostics;
using VoicemodPowertools.Core.Infrastructure;
using VoicemodPowertools.Domain;
using VoicemodPowertools.Services.Application.DownloadsConsole;

namespace VoicemodPowertools.Application.DownloadConsole;

public class OpenDownloadFolder : IOpenDownloadFolder
{
    public async Task Execute(string[] args)
    {
        var downloadsDirectors = new DirectoryInfo(ProgramConstants.DownloadsFolderName);
        if (!downloadsDirectors.Exists)
        {
            Console.WriteLine("Downloads folder does not exists yet. Maybe you had no downloads, or you cleared it.");
            Console.WriteLine($"It should be in: {downloadsDirectors.FullName}");
            return;
        }
        
        Process.Start("explorer.exe", downloadsDirectors.FullName);
    }
}