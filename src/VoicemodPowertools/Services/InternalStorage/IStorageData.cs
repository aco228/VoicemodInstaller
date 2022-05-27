using VoicemodPowertools.Domain.Storage;

namespace VoicemodPowertools.Services.InternalStorage;

public interface IStorageData
{
    bool ContainsKey<T>() where T : StorageEntryBase;
    T? Get<T>() where T : StorageEntryBase;
    void Add<T>(T entry) where T : StorageEntryBase;
}