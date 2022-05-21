using Installer.Services.Storage;

namespace Installer.Domain.Storage;

[Serializable]
public class StorageData : IStorageData
{
    public Dictionary<string, object> Data { get; set; } = new();

    public bool ContainsKey<T>() where T : StorageEntryBase
        => Data.ContainsKey(nameof(T));

    public T? Get<T>() where T : StorageEntryBase
        => ContainsKey<T>() ? GetFromData<T>() : default(T);

    private T GetFromData<T>() where T : StorageEntryBase
        => Data[nameof(T)] as T;

    public void Add<T>(T entry) where T : StorageEntryBase
    {
        var key = nameof(T);
        if (Data.ContainsKey(key))
            Data[key] = entry;
        else
            Data.Add(key, entry);
    }
}