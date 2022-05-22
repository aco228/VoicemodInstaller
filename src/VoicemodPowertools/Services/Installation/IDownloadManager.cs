using VoicemodPowertools.Domain.Storage.Entries;

namespace VoicemodPowertools.Services.Application;

public interface IDownloadManager
{
    bool StartDownload(GitlabAuthorization gitlabAuthorization, int timeoutInMinutes = 3);
}