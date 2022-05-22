namespace VoicemodPowertools.Domain.Storage.Entries;

[Serializable]
public class GitlabSecrets : StorageEntryBase
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public long ProjectId { get; set; }

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(ClientId) && !string.IsNullOrEmpty(ClientSecret) || ProjectId != 0;
    }
}