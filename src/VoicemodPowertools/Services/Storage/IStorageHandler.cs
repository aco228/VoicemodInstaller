using VoicemodPowertools.Domain.Storage;

namespace VoicemodPowertools.Services.Storage;

public interface IStorageHandler
{
    delegate void OnStorageChange();
    event OnStorageChange StateHasChanges;  
    bool StorageExists();
    T? Get<T>() where T : StorageEntryBase;
    void Save<T>(T enty) where T : StorageEntryBase;
    StorageData GetCurrent();
    T GetStorageFile<T>(string fileName) where T : class;
    void SaveStorageFile<T>(string fileName, T file) where T : class;
}