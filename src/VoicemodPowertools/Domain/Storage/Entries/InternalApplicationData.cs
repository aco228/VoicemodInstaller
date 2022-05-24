using Humanizer;

namespace VoicemodPowertools.Domain.Storage.Entries;

[Serializable]
public class InternalApplicationData : StorageEntryBase
{
    public string Version { get; set; }
    public DateTime BuiltAt { get; set; }

    public void Print()
    {
        Console.WriteLine($"Version: {Version}");
        Console.WriteLine($"Built: {BuiltAt.Humanize()}");
        Console.WriteLine();
    }
}