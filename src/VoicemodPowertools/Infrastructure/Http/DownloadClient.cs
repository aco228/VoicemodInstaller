using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using VoicemodPowertools.Services.Http;

namespace VoicemodPowertools.Infrastructure.Http;

public class DownloadClient : IDownloadClient
{
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0);
    private Dictionary<string, string> _headers = new();

    public void AddHeader(string key, string value)
    {
        _headers.TryAdd(key, value);
    }
    
    public async Task Download(
        string url, 
        string nameOfTheFileForDownload,
        string pathWhereToDownload,
        bool unzip,
        bool openOnDownload)
    {
        try
        {
            using var client = new WebClient();
            var uri = new Uri(url);
            var (fileName, extension) = GetFilenameInformations(nameOfTheFileForDownload);
            var downloadPath = Path.Combine(pathWhereToDownload, nameOfTheFileForDownload);
            PrepareDirectory(downloadPath);

            foreach (var header in _headers)
                client.Headers.Add(header.Key, header.Value);
            
            client.DownloadProgressChanged += ClientOnDownloadProgressChanged;
            client.DownloadFileCompleted += (sender, args) =>
            {
                if (args.Cancelled)
                {
                    Console.WriteLine("Cancelled!");
                    return;
                }

                var file = new FileInfo(downloadPath);
                if (file.Length == 0)
                {
                    _semaphore.Release();
                    Console.WriteLine("Looks like file is corrupted, as its downloaded size is 0");
                    return;
                }
                
                var directoryWhereFileIsDownloaded = file.Directory.FullName;

                if (unzip)
                {
                    ZipFile.ExtractToDirectory(file.FullName, Path.Combine(directoryWhereFileIsDownloaded, fileName));
                    File.Delete(downloadPath);
                    Console.WriteLine("File is unzipped");
                }

                if (openOnDownload)
                    Process.Start("explorer.exe", directoryWhereFileIsDownloaded);

                _semaphore.Release();
                Console.WriteLine(Environment.NewLine + $"Download finished:  {file.Directory.FullName}");
            };

            Console.WriteLine(@"Downloading file:");
            client.DownloadFileAsync(uri, downloadPath);
            await _semaphore.WaitAsync(3 * 60 * 1000);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private void ClientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        Console.Write("\r     -->    {0}%.", e.ProgressPercentage);
    }

    private void PrepareDirectory(string downloadPath)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(downloadPath));
        
        if (File.Exists(downloadPath))
            File.Delete(downloadPath);
        
        var directoryInfo = new DirectoryInfo(downloadPath);
        if (Directory.Exists(directoryInfo.FullName))
            Directory.Delete(directoryInfo.FullName, true);
    }

    private Tuple<string, string> GetFilenameInformations(string fileName)
    {
        var split = fileName.Split('.');
        if (split.Length != 2) throw new ArgumentException("File split length is not 2");
        return new(split[0], split[1]);
    }
}