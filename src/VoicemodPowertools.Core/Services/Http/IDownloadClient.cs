namespace VoicemodPowertools.Core.Services.Http;

public interface IDownloadClient
{
    void AddHeader(string key, string value);

    Task Download(
        string url,
        string nameOfTheFileForDownload,
        string pathWhereToDownload,
        bool unzip,
        bool openOnDownload);
}