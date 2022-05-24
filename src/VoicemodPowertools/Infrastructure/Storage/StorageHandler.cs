using VoicemodPowertools.Domain;
using VoicemodPowertools.Domain.Storage;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Infrastructure.Storage;

public class StorageHandler : IStorageHandler
{
    private static StorageData _storage = null;

    public event IStorageHandler.OnStorageChange? StateHasChanges;
    
    public bool StorageExists() => File.Exists(ProgramConstants.SecretsFile);
    
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

        _storage = GetStorageFile<StorageData>(ProgramConstants.SecretsFile);
        return _storage;
    }

    private void Save(StorageData storageData)
    {
        _storage = storageData;
        SaveStorageFile<StorageData>(ProgramConstants.SecretsFile, _storage);
    }

    public T GetStorageFile<T>(string fileName) where T : class
    {
        if (!File.Exists(fileName)) return default;
        return StorageHandlerExtensions.ReadFile<T>(fileName);
    }

    public void SaveStorageFile<T>(string fileName, T file) where T : class
        => StorageHandlerExtensions.WriteFile(fileName, file);
}