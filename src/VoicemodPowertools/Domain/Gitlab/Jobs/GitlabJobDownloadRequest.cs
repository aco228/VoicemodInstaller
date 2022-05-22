namespace VoicemodPowertools.Domain.Gitlab.Jobs;

public record GitlabJobDownloadRequest
{
    public long JobId { get; set; }
    public string Folder { get; set; } = ProgramConstants.DownloadsFolderName;
    public bool Unzip { get; set; } = false;
    public bool OpenFolderOnDownload { get; set; } = false;

    public string DownloadFileName => JobId.ToString();
    public string DownloadFile => DownloadFileName + ".zip";
    public string DownloadPath => Path.Combine(Folder, DownloadFile);
}