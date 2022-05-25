using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using VoicemodPowertools.Domain.Gitlab.Jobs;
using VoicemodPowertools.Services.Gitlab;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Infrastructure.Gitlab.Jobs;

public class GitlabJobDownloader : IGitlabJobDownloader
{
    private readonly IGitlabAuthorization _gitlabAuthorization;
    private readonly IConfiguration _configuration;
    private readonly IGitlabSecretsService _gitlabSecrets;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0);
    
    public GitlabJobDownloader(
        IGitlabAuthorization gitlabAuthorization,
        IConfiguration configuration,
        IGitlabSecretsService gitlabSecretsService)
    {
        _gitlabAuthorization = gitlabAuthorization;
        _configuration = configuration;
        _gitlabSecrets = gitlabSecretsService;
    }
    
    public async Task Download(GitlabJobDownloadRequest request)
    {
        try
        {
            PrepareDirectory(request);

            using var client = new WebClient();

            var gitlabBaseUrl = _configuration.GetValue<string>("GitlabApiBaseUrl");
            var uri = new Uri(
                $"{gitlabBaseUrl}projects/{_gitlabSecrets.Get().ProjectId}/jobs/{request.JobId}/artifacts");
            client.Headers.Add("Authorization", $"Bearer {_gitlabAuthorization.GetCurrent().Token}");
            client.DownloadProgressChanged += (sender, args) =>
            {
                Console.Write("\r     -->    {0}%.", args.ProgressPercentage);
            };
            client.DownloadFileCompleted += (sender, args) =>
            {
                if (args.Cancelled)
                {
                    Console.WriteLine("Cancelled!");
                    return;
                }

                var file = new FileInfo(request.DownloadPath);
                if (file.Length == 0)
                {
                    _semaphore.Release();
                    Console.WriteLine("Looks like file is corrupted, as its downloaded size is 0");
                    return;
                }
                
                var openDirectory = file.Directory.FullName;

                if (request.Unzip)
                {
                    openDirectory = Path.Combine(file.Directory.FullName, request.DownloadFileName);
                    ZipFile.ExtractToDirectory(file.FullName,openDirectory);
                    File.Delete(request.DownloadPath);
                    Console.WriteLine("File is unzipped");
                }

                if (request.OpenFolderOnDownload)
                    Process.Start("explorer.exe", openDirectory);

                _semaphore.Release();
                Console.WriteLine(Environment.NewLine + $"Download finished:  {file.Directory.FullName}");
            };

            Console.WriteLine(@"Downloading file:");
            client.DownloadFileAsync(uri, request.DownloadPath);
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

    private void PrepareDirectory(GitlabJobDownloadRequest request)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(request.DownloadPath));
        
        if (File.Exists(request.DownloadPath))
            File.Delete(request.DownloadPath);
        
        var directoryInfo = new DirectoryInfo(request.Folder);
        if (Directory.Exists(Path.Combine(directoryInfo.FullName, request.DownloadFileName)))
            Directory.Delete(Path.Combine(directoryInfo.FullName, request.DownloadFileName), true);
    }

}