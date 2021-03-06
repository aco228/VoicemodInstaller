using Microsoft.Extensions.Configuration;
using VoicemodPowertools.Core.Services.Gitlab;
using VoicemodPowertools.Core.Services.Http;
using VoicemodPowertools.Domain.Gitlab.Jobs;

namespace VoicemodPowertools.Core.Infrastructure.Gitlab.Jobs;

public class GitlabJobDownloader : IGitlabJobDownloader
{
    private readonly IGitlabAuthorization _gitlabAuthorization;
    private readonly IConfiguration _configuration;
    private readonly IGitlabSecretsService _gitlabSecrets;
    private readonly IDownloadClient _downloadClient;
    
    public GitlabJobDownloader(
        IGitlabAuthorization gitlabAuthorization,
        IConfiguration configuration,
        IGitlabSecretsService gitlabSecretsService,
        IDownloadClient downloadClient)
    {
        _gitlabAuthorization = gitlabAuthorization;
        _configuration = configuration;
        _gitlabSecrets = gitlabSecretsService;
        _downloadClient = downloadClient;
    }
    
    public Task Download(GitlabJobDownloadRequest request)
    {
        var gitlabBaseUrl = _configuration.GetValue<string>("GitlabApiBaseUrl");
        var uri = $"{gitlabBaseUrl}projects/{_gitlabSecrets.Get().ProjectId}/jobs/{request.JobId}/artifacts";

        _downloadClient.AddHeader("Authorization", $"Bearer {_gitlabAuthorization.GetCurrent().Token}");
        return _downloadClient.Download(
            uri,
            request.DownloadFile,
            request.Folder,
            request.Unzip,
            request.OpenFolderOnDownload);
    }
}