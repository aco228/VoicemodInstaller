using VoicemodPowertools.Domain.Gitlab.Jobs;

namespace VoicemodPowertools.Core.Services.Gitlab;

public interface IGitlabJobDownloader
{
    Task Download(GitlabJobDownloadRequest request);
}