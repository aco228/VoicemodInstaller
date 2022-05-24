using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Domain.Storage;

[Serializable]
public class StorageData : IStorageData
{
    public Dictionary<string, object> Data { get; set; } = new();

    public bool ContainsKey<T>() where T : StorageEntryBase
        => Data.ContainsKey(typeof(T).Name);

    public T? Get<T>() where T : StorageEntryBase
        => ContainsKey<T>() ? GetFromData<T>() : default(T);

    private T GetFromData<T>() where T : StorageEntryBase
        => Data[typeof(T).Name] as T;

    public void Add<T>(T entry) where T : StorageEntryBase
    {
        var key = typeof(T).Name;
        if (Data.ContainsKey(key))
            Data[key] = entry;
        else
            Data.Add(key, entry);
    }
}