namespace Installer.Services.Storage;

public interface IStorageData
{
    bool ContainsKey(string key);
    T? GetIfExists<T>(string key) where T : class;
    T Get<T>(string key) where T : class;
}