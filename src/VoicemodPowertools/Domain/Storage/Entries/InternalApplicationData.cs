namespace VoicemodPowertools.Domain.Storage.Entries;

[Serializable]
public class InternalApplicationData : StorageEntryBase
{
    public string Version { get; set; }
    public DateTime BuiltAt { get; set; }
}