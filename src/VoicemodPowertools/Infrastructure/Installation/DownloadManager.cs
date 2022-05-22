using System.ComponentModel;
using System.IO.Compression;
using System.Net;
using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Services.Application;

namespace VoicemodPowertools.Infrastructure.Installation;

public class DownloadManager : IDownloadManager
{
    private readonly string _url;
    private readonly string _downloadDirectory;
    private readonly string _fileName;
    private bool _result = false;
    private bool _unzip = false;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0);

    private string FileName => $"{_fileName}.zip";
    private string DownloadPath => Path.Combine(_downloadDirectory, FileName);
    
    public DownloadManager(string url, string fileName, string downloadDirectory, bool unzip = false)
    {
        if (string.IsNullOrEmpty(url)) throw new ArgumentException("Url is empty");
        if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("FileName is empty");
        if (string.IsNullOrEmpty(downloadDirectory)) throw new ArgumentException("DownloadPath is empty");
        
        _url = url;
        _unzip = unzip;
        _fileName = fileName;
        _downloadDirectory = downloadDirectory;
    }
    
    public bool StartDownload(GitlabAuthorization gitlabAuthorization, int timeoutInMinutes = 3)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(DownloadPath));

            if (File.Exists(DownloadPath))
                File.Delete(DownloadPath);

            var directoryInfo = new DirectoryInfo(_downloadDirectory);
            if (Directory.Exists(Path.Combine(directoryInfo.FullName, _fileName)))
                Directory.Delete(Path.Combine(directoryInfo.FullName, _fileName), true);

            using var client = new WebClient();
            var ur = new Uri(_url);
            
            // client.Credentials = new NetworkCredential("username", "password");
            client.Headers.Add("Authorization", $"Bearer {gitlabAuthorization.Token}");
                
            client.DownloadProgressChanged += (sender, args) =>
            {
                Console.Write("\r     -->    {0}%.", args.ProgressPercentage);
            };

            client.DownloadFileCompleted += DownloadCompleted;
            
            Console.WriteLine(@"Downloading file:");
            client.DownloadFileAsync(ur, DownloadPath);
            _semaphore.Wait(timeoutInMinutes * 60 * 1000);
            return _result && (_unzip ? Directory.Exists(Path.Combine((new DirectoryInfo(_downloadDirectory)).FullName, _fileName)) : File.Exists(DownloadPath));
        }
        catch (Exception e)
        {
            Console.WriteLine("Was not able to download file!");
            Console.Write(e);
            return false;
        }
        finally
        {
            _semaphore.Dispose();
        }
    }
    
    private void DownloadCompleted(object sender, AsyncCompletedEventArgs args)
    {
        _result = !args.Cancelled;
        if (!_result)
        {
            Console.Write(args.Error.ToString());
        }

        var file = new FileInfo(DownloadPath);

        if (_unzip)
        {
            ZipFile.ExtractToDirectory(file.FullName, Path.Combine(file.Directory.FullName, _fileName));
            File.Delete(DownloadPath);
            Console.WriteLine("Unziped file");
        }
        
        Console.WriteLine(Environment.NewLine + $"Download finished! File located at {file.Directory.FullName}");
        
        _semaphore.Release();
    }
}