using VoicemodPowertools.Core.Domain.InternalStorage;

namespace VoicemodPowertools.Core.Services.InternalStorage;

public interface IStorageData
{
    bool ContainsKey<T>() where T : StorageEntryBase;
    T? Get<T>() where T : StorageEntryBase;
    void Add<T>(T entry) where T : StorageEntryBase;
}