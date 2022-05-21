using Installer.Domain.Storage;

namespace Installer.Services.Storage;

public interface IStorageHandler
{
    bool StorageExists();
    T? Get<T>() where T : StorageEntryBase;
    void Save<T>(T enty) where T : StorageEntryBase;
    StorageData GetCurrent();
}