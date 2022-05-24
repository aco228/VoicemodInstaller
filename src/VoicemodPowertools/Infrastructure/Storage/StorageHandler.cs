using VoicemodPowertools.Domain.Storage;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Infrastructure.Storage;

public class StorageHandler : IStorageHandler
{
    private readonly string STORAGE_NAME = "st.rg";
    private static StorageData _storage = null;

    public event IStorageHandler.OnStorageChange? StateHasChanges;
    
    public bool StorageExists() => File.Exists(STORAGE_NAME);
    
    public T? Get<T>() where T : StorageEntryBase
    {
        var storage = GetCurrent();
        return storage.Get<T>();
    }
    
    public void Save<T>(T enty) where T : StorageEntryBase
    {
        var storage = GetCurrent();
        storage.Add<T>(enty);
        Save(storage);
        StateHasChanges?.Invoke();
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

        _storage = GetStorageFile<StorageData>(STORAGE_NAME);
        return _storage;
    }

    private void Save(StorageData storageData)
    {
        _storage = storageData;
        SaveStorageFile<StorageData>(STORAGE_NAME, _storage);
    }

    public T GetStorageFile<T>(string fileName) where T : class
        => StorageHandlerExtensions.ReadFile<T>(fileName);

    public void SaveStorageFile<T>(string fileName, T file) where T : class
        => StorageHandlerExtensions.WriteFile(fileName, file);
}