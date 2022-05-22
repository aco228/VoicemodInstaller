using VoicemodPowertools.Domain.Gitlab.Jobs;

namespace VoicemodPowertools.Services.Gitlab;

public interface IGitlabJobDownloader
{
    Task Download(GitlabJobDownloadRequest request);
}