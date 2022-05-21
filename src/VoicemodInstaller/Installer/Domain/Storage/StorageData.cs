using Installer.Services.Storage;

namespace Installer.Domain.Storage;

[Serializable]
public class StorageData : IStorageData
{
    public Dictionary<string, object> Data { get; set; } = new();

    public bool ContainsKey(string key)
        => Data.ContainsKey(key);

    public T? GetIfExists<T>(string key) where T : class
        => ContainsKey(key) ? Get<T>(key) : default(T);

    public T Get<T>(string key) where T : class
        => Data[key] as T;
}