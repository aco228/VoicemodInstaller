using Installer.Domain.Storage;
using Installer.Services.Storage;

namespace Installer.Infrastructure.Storage;

public class StorageHandler : IStorageHandler
{
    private readonly string STORAGE_NAME = "st.rg";
    private static StorageData _storage = null;

    public bool StorageExists() => File.Exists(STORAGE_NAME);
    
    public T? Get<T>() where T : StorageEntryBase
    {
        var storage = GetCurrent();
        return storage.GetIfExists<T>(nameof(T));
    }
    
    public void Save<T>(T enty) where T : StorageEntryBase
    {
        var storage = GetCurrent();
        var keyName = nameof(T);
        if (storage.ContainsKey(keyName))
            storage.Data[keyName] = enty;
        else
            storage.Data.Add(nameof(T), enty);
        Save(storage);
    }
    
    public StorageData GetCurrent()
    {
        if (_storage != null)
            return _storage;

        if (!StorageExists())
        {
            _storage = new StorageData();
            Save(_storage);
            return _storage;
        }
        
        _storage = StorageHandlerExtensions.ReadFromBinaryFile<StorageData>(STORAGE_NAME);
        return _storage;
    }

    private void Save(StorageData storageData)
    {
        _storage = storageData;
        StorageHandlerExtensions.WriteToBinaryFile(STORAGE_NAME, _storage);
    }
}